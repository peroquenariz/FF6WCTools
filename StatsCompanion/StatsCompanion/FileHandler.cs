using System;
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

        private string _lastLoadedSeed;
        private string[] _validSeedPrefixes = {
            "ff6wc_",
            "preset_",
            "standard_",
            "chaos_",
            "true_chaos_"
        };
        private readonly int[] _seedInfoLines = { 0, 1, 6, 8 };
        private DateTime _lastDirectoryRefresh;

        public string AppDirectory { get => _appDirectory; }
        public string RunsDirectory { get => _runsDirectory; }
        public string CrashlogDirectory { get => _crashlogDirectory; }
        public string LastLoadedSeed { get => _lastLoadedSeed; }
        public DateTime LastDirectoryRefresh { get => _lastDirectoryRefresh; }
        public TimeSpan RefreshInterval { get => _refreshInterval; }

        public FileHandler(string seedDirectory)
        {
            _appDirectory = Directory.GetCurrentDirectory();
            _runsDirectory = $"{_appDirectory}\\runs";
            _crashlogDirectory = $"{_appDirectory}\\crashlog";
            _seedDirectory = seedDirectory;

            _lastLoadedSeed = "";
            _lastDirectoryRefresh = DateTime.Now;

            CheckDirectory(RunsDirectory);
            CheckDirectory(CrashlogDirectory);
        }

        /// <summary>
        /// Checks if the directories exist, if not, create them.
        /// </summary>
        private void CheckDirectory (string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
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
        /// Scans the seed folder and looks for the zip file with the last creation date that matches WC naming.
        /// Opens the txt inside the seed zip file and gets seed information.
        /// </summary>
        public bool UpdateLastSeed(out string[] seedInfo)
        {
            _lastDirectoryRefresh = DateTime.Now;
            seedInfo = new string[9];
            bool seedFound = false;
            
            // Scan directory
            DirectoryInfo directory = new(@_seedDirectory);
            FileInfo[] files = directory.GetFiles("*.zip").OrderBy(f => f.CreationTime).ToArray();

            if (files.Length == 0)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 7;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"No seeds found in the seed directory: {_seedDirectory}".PadRight(60));
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("".PadRight(60));
                }
                _lastLoadedSeed = "";
                return false;
            }
            
            FileInfo lastCreatedZip = files[0];
            for (int i = files.Length-1; i >= 0 && seedFound == false; i--)
            {
                string filename = files[i].Name;
                foreach (var prefix in _validSeedPrefixes)
                {
                    if (filename.StartsWith(prefix))
                    {
                        lastCreatedZip = files[i];
                        seedFound = true;
                        break;
                    }
                }
            }

            if (!seedFound)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 7;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"No seeds found in the seed directory: {_seedDirectory}".PadRight(60));
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("".PadRight(60));
                }
                _lastLoadedSeed = "";
                return false;
            }

            if (lastCreatedZip.Name != _lastLoadedSeed)
            {
                _lastLoadedSeed = lastCreatedZip.Name;
                ZipArchive seedZip = ZipFile.OpenRead(lastCreatedZip.FullName);
                ZipArchiveEntry seedTxt = seedZip.GetEntry(lastCreatedZip.Name.Remove(lastCreatedZip.Name.Length - 3, 3) + "txt")!;

                if (seedTxt != null)
                {
                    using (Stream seedTextStream = seedTxt.Open())
                    {
                        StreamReader reader = new(seedTextStream);
                        for (int i = 0; i < 9; i++)
                        {
                            seedInfo[i] = reader.ReadLine()!;
                        }
                        reader.Close();
                    }
                }

                seedZip.Dispose();
                
                Console.CursorLeft = 0;
                Console.CursorTop = 7;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Loaded seed: {lastCreatedZip.Name}".PadRight(60));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                for (int i = 0; i < _seedInfoLines.Length; i++)
                {
                    Console.WriteLine(seedInfo[_seedInfoLines[i]]); // TODO: check the beginning of each string instead
                }
            }
            return true;
        }
    }
}