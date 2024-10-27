using System.Collections.Generic;

namespace FF6WCToolsLib;

public readonly struct SeedData
{
    public FileHandler.SeedDirectoryStatus Status { get; }
    public string? Filename { get; }
    public string?[]? SeedInfo { get; }
    public List<string>? SeedInfoLines { get; }

    /// <summary>
    /// No seed or info has been found.
    /// </summary>
    public SeedData(FileHandler.SeedDirectoryStatus status)
    {
        Status = status;
        Filename = null;
        SeedInfo = null;
        SeedInfoLines = null;
    }

    /// <summary>
    /// Seed hes been found but accompanying txt isn't present.
    /// </summary>
    public SeedData(FileHandler.SeedDirectoryStatus status, string filename)
    {
        Status = status;
        Filename = filename;
        SeedInfo = null;
        SeedInfoLines = null;
    }

    /// <summary>
    /// Seed and its info has been found.
    /// </summary>
    public SeedData(FileHandler.SeedDirectoryStatus status, string filename, string?[] seedInfo, List<string> seedInfoLines)
    {
        Status = status;
        Filename = filename;
        SeedInfo = seedInfo;
        SeedInfoLines = seedInfoLines;
    }
}