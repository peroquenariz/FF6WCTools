using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FF6WCToolsLib;

/// <summary>
/// Handles all file operations.
/// </summary>
public class FileHandler
{
    private readonly TimeSpan _refreshInterval = new(0,0,2);
    private readonly List<string> _directoryList;
    private readonly string _appDirectory;
    private readonly string _runsDirectory;
    private readonly string _crashlogDirectory;
    private string _seedDirectory;

    private readonly string[] _validSeedPrefixes = {
        "ff6wc_",
        "preset_",
        "ff6_wc_",
        "manually",
        "standard_",
        "chaos_",
        "true_chaos_",
        "blamethebot",
    };
    
    private readonly List<string> _seedInfoLines = new List<string>(){
        "Version",
        "Generated",
        "Seed",
        "Hash",
    };

    private readonly List<string> _validSeedFileExtensions = new()
    {
        ".zip",
        ".smc",
        ".txt",
    };
    
    private string _lastLoadedSeed;
    private DateTime _lastDirectoryRefresh;
    private SeedDirectoryStatus _seedDirStatus;
    private bool _isSeedDirectoryValid;
    private SeedData _seedData;

    public event EventHandler<SeedInfoEventArgs>? OnShowSeedInfoStatus;

    public enum SeedDirectoryStatus
    {
        /// <summary>
        /// Seed directory path empty.
        /// </summary>
        NONE,
        
        /// <summary>
        /// Seed directory path invalid.
        /// </summary>
        INVALID,
        
        /// <summary>
        /// Empty seed directory.
        /// </summary>
        NO_FILES_FOUND,
        
        /// <summary>
        /// No seeds in seed directory.
        /// </summary>
        NO_SEEDS_FOUND,

        /// <summary>
        /// Seed with matching .txt found.
        /// </summary>
        SEED_FOUND,

        /// <summary>
        /// Seed found but no matching .txt file available.
        /// </summary>
        SEED_FOUND_NO_INFO_AVAILABLE,
    }

    public char DirectorySeparator => Path.DirectorySeparatorChar;
    public bool CanRefresh => DateTime.Now - _lastDirectoryRefresh > _refreshInterval;
    public bool IsSeedDirectoryValid => _isSeedDirectoryValid;
    public string RunsDirectory => _runsDirectory;
    public string CrashlogDirectory => _crashlogDirectory;
    public string LastLoadedSeed => _lastLoadedSeed;
    public string SeedDirectory
    {
        get
        {
            return _seedDirectory;
        }
        set
        {
            _isSeedDirectoryValid = CheckSeedDirectory(value);
            if (_isSeedDirectoryValid)
            {
                _seedDirectory = value;
            }
            else
            {
                OnShowSeedInfoStatus?.Invoke(this, new SeedInfoEventArgs(new SeedData(_seedDirStatus)));
            }
        }
    }

    public FileHandler(bool requestRunsDirectory)
    {
        _appDirectory = Directory.GetCurrentDirectory();
        _runsDirectory = $"{_appDirectory}{DirectorySeparator}runs";
        _crashlogDirectory = $"{_appDirectory}{DirectorySeparator}crashlog";
        _seedDirectory = string.Empty;

        _directoryList = new List<string>() { _crashlogDirectory };
        if (requestRunsDirectory) _directoryList.Add(_runsDirectory);

        _lastLoadedSeed = "";
        _lastDirectoryRefresh = DateTime.Now;

        InitializeDirectories(_directoryList);
    }

    /// <summary>
    /// Checks if the directories exist, if not, create them.
    /// </summary>
    private void InitializeDirectories (List<string> directoryList)
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

    private bool CheckSeedDirectory(string seedDirectory)
    {
        bool isValid = true;
        _seedDirStatus = SeedDirectoryStatus.NO_FILES_FOUND;

        if (seedDirectory.Length == 0)
        {
            // Empty seed directory path
            _seedDirStatus = SeedDirectoryStatus.NONE;
            isValid = false;
        }
        else if (!Directory.Exists(seedDirectory))
        {
            // Invalid seed directory path
            _seedDirStatus = SeedDirectoryStatus.INVALID;
            isValid = false;
        }
        
        return isValid;
    }

    /// <summary>
    /// Scans the seed folder and looks for the zip or text file with the last creation date that matches WC naming.
    /// Reads the txt and gets seed information.
    /// </summary>
    /// <param name="seedInfoPrevious"></param>
    /// <param name="showData">Should the seed data be shown on screen?</param>
    /// <returns>An array with the first lines of the seed info txt.</returns>
    public string[] UpdateSeedInfo(string[] seedInfoPrevious, bool showData = true)
    {
        if (!_isSeedDirectoryValid && showData)
        {
            OnShowSeedInfoStatus?.Invoke(this, new SeedInfoEventArgs(_seedData));
            return seedInfoPrevious;
        }
        
        _lastDirectoryRefresh = DateTime.Now;
        string[] seedInfo = seedInfoPrevious;
        _seedDirStatus = SeedDirectoryStatus.NO_FILES_FOUND;

        // Scan directory
        DirectoryInfo directory = new(_seedDirectory);
        
        // Create an array of files, and order it by creation date.
        FileInfo[] files = directory.GetFiles("*.*").OrderBy(f => f.CreationTime).ToArray();

        // If there are no files in the directory
        if (files.Length == 0)
        {
            _seedData = new(_seedDirStatus);
            _lastLoadedSeed = "";
            if (showData) OnShowSeedInfoStatus?.Invoke(this, new SeedInfoEventArgs(_seedData));
            return seedInfo;
        }
        
        FileInfo lastCreatedFile = files[0];
        
        // Iterate the file array and check if there are any seeds
        for (int i = files.Length-1; i >= 0 && !(_seedDirStatus == SeedDirectoryStatus.SEED_FOUND); i--)
        {
            FileInfo file = files[i];
            foreach (string prefix in _validSeedPrefixes)
            {
                // If it matches one of the WC prefixes
                if (file.Name.StartsWith(prefix) && _validSeedFileExtensions.Contains(file.Extension))
                {
                    lastCreatedFile = file;
                    _seedDirStatus = SeedDirectoryStatus.SEED_FOUND;
                    break;
                }
            }
        }

        // If no seeds were found in the file array
        if (!(_seedDirStatus == SeedDirectoryStatus.SEED_FOUND))
        {
            _lastLoadedSeed = "";
            _seedDirStatus = SeedDirectoryStatus.NO_SEEDS_FOUND;
            _seedData = new(_seedDirStatus);
            if (showData) OnShowSeedInfoStatus?.Invoke(this, new SeedInfoEventArgs(_seedData));
            return seedInfo;
        }
        
        // If a valid seed filename was found
        if ((_seedDirStatus == SeedDirectoryStatus.SEED_FOUND) && lastCreatedFile.Name != _lastLoadedSeed)
        {
            // Is a different seed.
            _lastLoadedSeed = lastCreatedFile.Name;
            string entryName = "";
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(lastCreatedFile.Name);

            if (lastCreatedFile.Extension == ".zip")
            {
                ZipArchive seedZip = ZipFile.OpenRead(lastCreatedFile.FullName);
                // Compare replacing underscores with spaces
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
                    _seedData = ReadSeedData(filenameWithoutExtension, seedTxt, seedInfo);
                }

                seedZip.Dispose();
            }
            else if (lastCreatedFile.Extension == ".txt")
            {
                _seedData = ReadSeedData(filenameWithoutExtension, lastCreatedFile.FullName, seedInfo);
            }
            else if (lastCreatedFile.Extension == ".smc")
            {
                string filename = $"{filenameWithoutExtension}.txt";
                string txtPath = $"{lastCreatedFile.DirectoryName}{DirectorySeparator}{filename}";
                bool hasTxt = File.Exists(txtPath);
                if (hasTxt)
                {
                    _seedData = ReadSeedData(filenameWithoutExtension, txtPath, seedInfo);
                }
                else
                {
                    _seedDirStatus = SeedDirectoryStatus.SEED_FOUND_NO_INFO_AVAILABLE;
                    _seedData = new(_seedDirStatus, filenameWithoutExtension);
                    _lastLoadedSeed = "";
                }
            }

            if (showData) OnShowSeedInfoStatus?.Invoke(this, new SeedInfoEventArgs(_seedData));
        }
        
        return seedInfo;
    }

    private SeedData ReadSeedData(string filename, ZipArchiveEntry seedTxt, string[] seedInfo)
    {
        using (Stream seedTextStream = seedTxt.Open())
        {
            ReadSeedTextStream(seedTextStream, seedInfo);
        }

        return new(_seedDirStatus, filename, seedInfo, _seedInfoLines);
    }

    private SeedData ReadSeedData(string filename, string txtPath, string[] seedInfo)
    {
        using (Stream seedTextStream = File.OpenRead(txtPath))
        {
            ReadSeedTextStream(seedTextStream, seedInfo);
        }

        return new(_seedDirStatus, filename, seedInfo, _seedInfoLines);
    }

    /// <summary>
    /// Takes a text stream and writes the first 9 lines into a string array.
    /// </summary>
    /// <param name="seedTextStream">The text file stream.</param>
    /// <param name="seedInfo">The seed information array</param>
    private static void ReadSeedTextStream(Stream seedTextStream, string[] seedInfo)
    {
        StreamReader reader = new(seedTextStream);
        for (int i = 0; i < 9; i++)
        {
            seedInfo[i] = reader.ReadLine()!;
        }
        reader.Close();
    }

    /// <summary>
    /// Resets the name of the last loaded seed.
    /// </summary>
    public void ResetLastLoadedSeed()
    {
        _lastLoadedSeed = "";
    }

    /// <summary>
    /// Writes a crashlog file.
    /// </summary>
    /// <param name="crashTime">Timestamp of the crash.</param>
    /// <param name="crashlog">Crash log data.</param>
    /// <returns>A string with the crashlog path.</returns>
    public string WriteCrashlogFile(DateTime crashTime, string crashlog)
    {
        // Crash log path.
        string crashlogPath = $"{CrashlogDirectory}{DirectorySeparator}crashlog - {crashTime.ToString("yyyy_MM_dd - HH_mm_ss")}.txt";

        // Write crashlog txt
        WriteStringToFile(crashlogPath, crashlog);

        return crashlogPath;
    }
}

public class SeedInfoEventArgs : EventArgs
{
    public SeedData SeedData { get; }
    public SeedInfoEventArgs(SeedData seedData)
    {
        SeedData = seedData;
    }
}