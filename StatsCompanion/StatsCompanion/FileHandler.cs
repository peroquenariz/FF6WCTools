using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace StatsCompanion
{
    /// <summary>
    /// Handles all file operations.
    /// </summary>
    internal class FileHandler
    {
        private readonly TimeSpan _refreshInterval = new(0,0,2);
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
        private readonly string _appDirectory;
        private readonly string _runsDirectory;
        private readonly string _crashlogDirectory;
        private readonly string _seedDirectory;

        private readonly string[] _validSeedPrefixes = {
            "ff6wc_",
            "preset_",
            "manually",
            "standard_",
            "chaos_",
            "true_chaos_",
            "blamethebot"
        };
        private readonly List<string> _seedInfoLines = new(){
            "Version",
            "Generated",
            "Seed",
            "Hash"
        };
        private readonly List<string> _directoryList;
        
        private string _lastLoadedSeed;
        private DateTime _lastDirectoryRefresh;

        public string AppDirectory { get => _appDirectory; }
        public string RunsDirectory { get => _runsDirectory; }
        public string CrashlogDirectory { get => _crashlogDirectory; }
        public string LastLoadedSeed { get => _lastLoadedSeed; }
        public string SeedDirectory { get => _seedDirectory; }
        public DateTime LastDirectoryRefresh { get => _lastDirectoryRefresh; }
        public TimeSpan RefreshInterval { get => _refreshInterval; }

        public FileHandler(string seedDirectory)
        {
            _appDirectory = Directory.GetCurrentDirectory();
            _runsDirectory = $"{_appDirectory}\\runs";
            _crashlogDirectory = $"{_appDirectory}\\crashlog";
            _seedDirectory = seedDirectory;

            _directoryList = new() { _runsDirectory, _crashlogDirectory };
            
            _lastLoadedSeed = "";
            _lastDirectoryRefresh = DateTime.Now;

            CheckDirectory(_directoryList);
        }

        /// <summary>
        /// Checks if the directories exist, if not, create them.
        /// </summary>
        private void CheckDirectory (List<string> directoryList)
        {
            foreach (var directory in directoryList)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                } 
            }
        }

        /// <summary>
        /// Writes a string into a text file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="content">The string to write.</param>
        public static void WriteStringToFile (string path, string content)
        {
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Takes the run data and serializes the JSON with pretty format.
        /// </summary>
        /// <param name="runArguments">The run data.</param>
        /// <returns>A formatted JSON string, ready to write to a file.</returns>
        public string SerializeJson(Arguments runArguments)
        {
            string serializedJson = JsonSerializer.Serialize(runArguments, _jsonOptions);
            return serializedJson;
        }

        /// <summary>
        /// Scans the seed folder and looks for the zip or text file with the last creation date that matches WC naming.
        /// Reads the txt and gets seed information.
        /// </summary>
        /// <param name="seedInfo">The first lines of the seed txt. If no file is found, array will have null strings.</param>
        /// <returns>true if the directory is valid, otherwise false.</returns>
        public bool UpdateLastSeed(string[] seedInfoPrevious, out string[] seedInfo)
        {
            _lastDirectoryRefresh = DateTime.Now;
            seedInfo = seedInfoPrevious;
            bool seedFound = false;

            if (_seedDirectory.Length == 0)
            {
                Log.NoSeedDirectory();
                return false;
            }
            if (!Directory.Exists(_seedDirectory))
            {
                Log.InvalidSeedDirectory();
                return false;
            }

            // Scan directory
            DirectoryInfo directory = new(_seedDirectory);
            FileInfo[] files = directory.GetFiles("*.*").OrderBy(f => f.CreationTime).ToArray();

            if (files.Length == 0)
            {
                Log.NoSeedsFound();
                _lastLoadedSeed = "";
                return true;
            }
            
            FileInfo lastCreatedFile = files[0];
            for (int i = files.Length-1; i >= 0 && !seedFound; i--)
            {
                FileInfo file = files[i];
                foreach (string prefix in _validSeedPrefixes)
                {
                    if (file.Name.StartsWith(prefix))
                    {
                        lastCreatedFile = file;
                        seedFound = true;
                        break;
                    }
                }
            }

            if (!seedFound)
            {
                Log.NoSeedsFound();
                _lastLoadedSeed = "";
                return true;
            }
            
            if (lastCreatedFile.Name != _lastLoadedSeed)
            {
                _lastLoadedSeed = lastCreatedFile.Name;
                Stream seedTextStream;
                string entryName = "";
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(lastCreatedFile.Name);

                if (lastCreatedFile.Extension == ".zip")
                {
                    ZipArchive seedZip = ZipFile.OpenRead(lastCreatedFile.FullName);
                    // Compare replacing underscores with spaces
                    // Thanks Seedbot :)
                    // Nvm, it's Discord's fault lmao
                    foreach (var entry in seedZip.Entries)
                    {
                        string filenameNoSpaces = (filenameWithoutExtension + ".txt").Replace("_", " ");
                        string entryNoSpaces = entry.Name.Replace("_", " ");
                        if (entryNoSpaces == filenameNoSpaces)
                        {
                            entryName = entry.Name;
                            break;
                        }
                    }

                    ZipArchiveEntry? seedTxt = seedZip.GetEntry(entryName);

                    if (seedTxt != null)
                    {
                        using (seedTextStream = seedTxt.Open())
                        {
                            ReadSeedTextFile(seedTextStream, seedInfo);
                        }
                        Log.SeedInformation(filenameWithoutExtension, seedInfo, _seedInfoLines);
                    }

                    seedZip.Dispose();
                }
                else if (lastCreatedFile.Extension == ".txt")
                {
                    using (seedTextStream = File.OpenRead(lastCreatedFile.FullName))
                    {
                        ReadSeedTextFile(seedTextStream, seedInfo);
                    }
                    Log.SeedInformation(filenameWithoutExtension, seedInfo, _seedInfoLines);
                }
                else if (lastCreatedFile.Extension == ".smc")
                {
                    string txtPath = $"{lastCreatedFile.DirectoryName}\\{filenameWithoutExtension}.txt";
                    bool hasTxt = File.Exists(txtPath);
                    if (hasTxt)
                    {
                        using (seedTextStream = File.OpenRead(txtPath))
                        {
                            ReadSeedTextFile(seedTextStream, seedInfo);
                        }
                        Log.SeedInformation(filenameWithoutExtension, seedInfo, _seedInfoLines);
                    }
                    else
                    {
                        Log.NoMatchingSeedInfoFound(lastCreatedFile.Name);
                        _lastLoadedSeed = "";
                    }
                }
            }
            return true;
        }

        private static void ReadSeedTextFile(Stream seedTextStream, string[] seedInfo)
        {
            StreamReader reader = new(seedTextStream);
            for (int i = 0; i < 9; i++)
            {
                seedInfo[i] = reader.ReadLine()!;
            }
            reader.Close();
        }

        public void ResetLastLoadedSeed()
        {
            _lastLoadedSeed = "";
        }
    }
}