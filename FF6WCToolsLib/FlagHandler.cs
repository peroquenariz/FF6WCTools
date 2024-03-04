using System;
using System.Collections.Generic;
using System.Linq;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib;

/// <summary>
/// Provides methods to handle flagsets.
/// </summary>
public static class FlagHandler
{
    private static readonly List<string> _aestheticFlags = new() {
    "-cpal",
    "-cpor",
    "-cspr",
    "-cspp",
    "-name",
    };

    public static string GetFlagset(string flags)
    {
        string flagset = "Other";
        List<string> flagsList = GetFlagsList(flags);

        // Find a matching flagset.
        foreach (var flagsetEntry in FLAGSET_DICT)
        {
            List<string> entry = GetFlagsList(flagsetEntry.Value);
            if (flagsList.All(entry.Contains) && flagsList.Count == entry.Count)
            {
                flagset = flagsetEntry.Key;
                break;
            }
        }

        return flagset;
    }

    /// <summary>
    /// Takes a flagstring and splits it into individual flags.
    /// Ignores aesthetic flags.
    /// </summary>
    /// <param name="flags">The flagstring to parse.</param>
    /// <returns>A list of flags.</returns>
    public static List<string> GetFlagsList(string flags)
    {
        List<string> flagsList = new();

        int flagStartIndex = 0;

        // Index skips first character.
        for (int i = 1; i < flags.Length; i++)
        {
            // Look for a "-" character and split one character before.
            if (flags[i] == '-')
            {
                string flag = flags[flagStartIndex..(i - 1)];
                bool isAesthetic = false;
                
                // Ignore aesthetic flags.
                foreach (var aestheticFlag in _aestheticFlags)
                {
                    isAesthetic = flag.Contains(aestheticFlag);
                    if (isAesthetic) break;
                }
                if (!isAesthetic) flagsList.Add(flag);

                flagStartIndex = i;
            }
            else if (flags[i] == flags.Length - 1)
            {
                // Reached the end of the flagstring, add the last flag.
                // Last flag is never aesthetic.
                flagsList.Add(flags[flagStartIndex..]);
            }
        }

        return flagsList;
    }
}
