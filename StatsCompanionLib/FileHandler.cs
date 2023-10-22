using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace StatsCompanionLib;

/// <summary>
/// Handles all file operations.
/// </summary>
public class FileHandler
{
    public event EventHandler? OnSeedDirectoryNotFound;
    public event EventHandler? OnSeedDirectoryInvalid;
    public event EventHandler? OnSeedNotFound;

    public event EventHandler<SeedInfoFoundEventArgs>? OnSeedInfoFound;
    public event EventHandler<SeedInfoNotFoundEventArgs>? OnSeedInfoNotFound;

    private const string JSON_TIMESTAMP_FORMAT = "yyyy_MM_dd - HH_mm_ss";

    private readonly TimeSpan _refreshInterval = new(0,0,2);
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
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
        "blamethebot",
    };
    private readonly List<string> _seedInfoLines = new List<string>(){
        "Version",
        "Generated",
        "Seed",
        "Hash",
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
    public char DirectorySeparator { get => Path.DirectorySeparatorChar; }

    public FileHandler(string seedDirectory)
    {
        _appDirectory = Directory.GetCurrentDirectory();
        _runsDirectory = $"{_appDirectory}{DirectorySeparator}runs";
        _crashlogDirectory = $"{_appDirectory}{DirectorySeparator}crashlog";
        _seedDirectory = seedDirectory;

        _directoryList = new List<string>() { _runsDirectory, _crashlogDirectory };
        
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
    public string SerializeJson(RunJson runArguments)
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
    public bool UpdateLastSeed(string[] seedInfoPrevious, out string[] seedInfo, bool writeSeedInfo = true)
    {
        _lastDirectoryRefresh = DateTime.Now;
        seedInfo = seedInfoPrevious;
        bool seedFound = false;

        // If a seed directory wasn't provided in the config file
        if (_seedDirectory.Length == 0)
        {
            if (writeSeedInfo) OnSeedDirectoryNotFound?.Invoke(this, EventArgs.Empty);
            return false;
        }

        // If the seed directory provided is invalid
        if (!Directory.Exists(_seedDirectory))
        {
            if (writeSeedInfo) OnSeedDirectoryInvalid?.Invoke(this, EventArgs.Empty);
            return false;
        }

        // Scan directory
        DirectoryInfo directory = new(_seedDirectory);
        
        // Create an array of files, and order it by creation date.
        FileInfo[] files = directory.GetFiles("*.*").OrderBy(f => f.CreationTime).ToArray();

        // If there are no files in the directory
        if (files.Length == 0)
        {
            if (writeSeedInfo) OnSeedNotFound?.Invoke(this, EventArgs.Empty);
            _lastLoadedSeed = "";
            return true;
        }
        
        FileInfo lastCreatedFile = files[0];
        
        // Iterate the file array and check if there are any seeds
        for (int i = files.Length-1; i >= 0 && !seedFound; i--)
        {
            FileInfo file = files[i];
            foreach (string prefix in _validSeedPrefixes)
            {
                // If it matches one of the WC prefixes
                if (file.Name.StartsWith(prefix))
                {
                    lastCreatedFile = file;
                    seedFound = true;
                    break;
                }
            }
        }

        // If no seeds were found in the file array
        if (!seedFound)
        {
            if (writeSeedInfo) OnSeedNotFound?.Invoke(this, EventArgs.Empty);
            _lastLoadedSeed = "";
            return true;
        }
        
        // If a valid seed filename was found
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
                    if (writeSeedInfo) OnSeedInfoFound?.Invoke(this, new SeedInfoFoundEventArgs(filenameWithoutExtension, seedInfo, _seedInfoLines));
                }

                seedZip.Dispose();
            }
            else if (lastCreatedFile.Extension == ".txt")
            {
                using (seedTextStream = File.OpenRead(lastCreatedFile.FullName))
                {
                    ReadSeedTextFile(seedTextStream, seedInfo);
                }
                if (writeSeedInfo) OnSeedInfoFound?.Invoke(this, new SeedInfoFoundEventArgs(filenameWithoutExtension, seedInfo, _seedInfoLines));
            }
            else if (lastCreatedFile.Extension == ".smc")
            {
                string txtPath = $"{lastCreatedFile.DirectoryName}{DirectorySeparator}{filenameWithoutExtension}.txt";
                bool hasTxt = File.Exists(txtPath);
                if (hasTxt)
                {
                    using (seedTextStream = File.OpenRead(txtPath))
                    {
                        ReadSeedTextFile(seedTextStream, seedInfo);
                    }
                    if (writeSeedInfo) OnSeedInfoFound?.Invoke(this, new SeedInfoFoundEventArgs(filenameWithoutExtension, seedInfo, _seedInfoLines));
                }
                else
                {
                    if (writeSeedInfo) OnSeedInfoNotFound?.Invoke(this, new SeedInfoNotFoundEventArgs(lastCreatedFile.Name));
                    _lastLoadedSeed = "";
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Takes a text stream and writes the first 9 lines into a string array.
    /// </summary>
    /// <param name="seedTextStream">The text file stream.</param>
    /// <param name="seedInfo">The seed information array</param>
    private static void ReadSeedTextFile(Stream seedTextStream, string[] seedInfo)
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
    /// Writes the run JSON file.
    /// </summary>
    /// <param name="endTime">Timestamp of the end of the run.</param>
    /// <param name="runJson">The run JSON data to write.</param>
    public void WriteJSONFile(DateTime endTime, RunJson runJson)
    {
        // Serialize the JSON.
        string jsonRunData = SerializeJson(runJson);

        // Create a timestamped filename.
        string jsonPath = $"{RunsDirectory}{DirectorySeparator}{endTime.ToString(JSON_TIMESTAMP_FORMAT)}.json";

        // Write to a .json file.
        WriteStringToFile(jsonPath, jsonRunData);
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

public class SeedInfoFoundEventArgs
{
    public string Filename { get; }
    public string[] SeedInfo { get; }
    public List<string> SeedInfoLines { get; }
    
    public SeedInfoFoundEventArgs(string filename, string[] seedInfo, List<string> seedInfoLines)
    {
        Filename = filename;
        SeedInfo = seedInfo;
        SeedInfoLines = seedInfoLines;
    }
}

public class SeedInfoNotFoundEventArgs
{
    public string Filename { get; }

    public SeedInfoNotFoundEventArgs (string filename)
    {
        Filename = filename;
    }
}