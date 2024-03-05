using System;
using System.Collections.Generic;
using FF6WCToolsLib;
using StatsCompanionLib;

namespace StatsCompanionConsoleApp;

/// <summary>
/// Handles the console view.
/// </summary>
internal class ConsoleViewer
{
    private const int RIGHT_PADDING = 90;
    private const string WINDOW_TITLE = "Stats Companion";
    
    private readonly string? _appVersion;
    private readonly string? _libVersion;

    private static int _cursorTopPosition;

    public ConsoleViewer(string? appVersion, StatsCompanion statsCompanion, SniClient sniClient, FileHandler fileHandler)
    {
        Console.CursorVisible = false;
        Console.Title = WINDOW_TITLE;
        _appVersion = appVersion;
        _libVersion = statsCompanion.LibVersion;

        sniClient.OnConnectionError += SniClient_OnConnectionError;
        sniClient.OnConnectionSuccessful += SniClient_OnConnectionSuccessful;
        sniClient.OnCountdownTick += SniClient_OnCountdownTick;

        fileHandler.OnShowSeedInfoStatus += FileHandler_OnShowSeedInfoStatus;

        statsCompanion.OnExecutionLoopStart += StatsCompanion_OnExecutionLoopStart;
        statsCompanion.OnShowVersionDebug += StatsCompanion_OnShowVersionDebug;
        statsCompanion.OnSeedAbandoned += StatsCompanion_OnSeedAbandoned;
        statsCompanion.OnWaitingForNewGame += StatsCompanion_OnWaitingForNewGame;
        statsCompanion.OnTrackingRun += StatsCompanion_OnTrackingRun;
        statsCompanion.OnShowDebugInformation += StatsCompanion_OnShowDebugInformation;
        statsCompanion.OnRunSuccessful += StatsCompanion_OnRunSuccessful;
        statsCompanion.OnCheckKeypress += StatsCompanion_OnCheckKeypress;

        Version();
    }

    private void FileHandler_OnShowSeedInfoStatus(object? sender, SeedInfoEventArgs e)
    {
        switch (e.SeedData.Status)
        {
            case FileHandler.SeedDirectoryStatus.NONE:
                NoSeedDirectory();
                break;
            case FileHandler.SeedDirectoryStatus.INVALID:
                InvalidSeedDirectory();
                break;
            case FileHandler.SeedDirectoryStatus.NO_FILES_FOUND:
                NoFilesFound();
                break;
            case FileHandler.SeedDirectoryStatus.NO_SEEDS_FOUND:
                NoSeedsFound();
                break;
            case FileHandler.SeedDirectoryStatus.SEED_FOUND:
                SeedInformation(e.SeedData);
                break;
            case FileHandler.SeedDirectoryStatus.SEED_FOUND_NO_INFO_AVAILABLE:
                if (e.SeedData.Filename != null) NoMatchingSeedInfoFound(e.SeedData.Filename);
                break;
        }
    }

    private void StatsCompanion_OnCheckKeypress(object? sender, EventArgs e)
    {
        CheckForEscapeKeypress();
    }

    private void SniClient_OnCountdownTick(object? sender, CountdownEventArgs e)
    {
        RetryCounter(e.Counter);
    }

    private void StatsCompanion_OnExecutionLoopStart(object? sender, EventArgs e)
    {
        _cursorTopPosition = 3;
    }

    private void StatsCompanion_OnRunSuccessful(object? sender, RunSuccessfulEventArgs e)
    {
        RunSuccessful(e.FinalTime);
    }

    private void StatsCompanion_OnShowDebugInformation(object? sender, DebugInformationEventArgs e)
    {
        DebugInformation(e.RunData);
    }

    private void StatsCompanion_OnTrackingRun(object? sender, EventArgs e)
    {
        TrackingRun();
    }

    private void StatsCompanion_OnWaitingForNewGame(object? sender, EventArgs e)
    {
        WaitingForNewGame();
    }

    private void StatsCompanion_OnSeedAbandoned(object? sender, EventArgs e)
    {
        SeedAbandoned();
    }

    private void StatsCompanion_OnShowVersionDebug(object? sender, EventArgs e)
    {
        Version(true);
    }

    private void SniClient_OnConnectionSuccessful(object? sender, ConnectionSuccessfulEventArgs e)
    {
        ConnectionSuccessful(e.Uri);
    }

    private void SniClient_OnConnectionError(object? sender, ConnectionErrorEventArgs e)
    {
        ConnectionError(e.Message);
    }

    public void Version(bool isDebugMode = false)
    {
        if (isDebugMode) Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        //string version = $"Stats Companion v{_libVersion} (lib) | v{_appVersion} (app)";
        string version = $"Stats Companion v{_libVersion}";
        Console.Write(version);
        if (isDebugMode) Console.Write(" - DEBUG MODE");
        Console.WriteLine();
        for (int i = 0; i < version.Length; i++)
        {
            Console.Write("-");
        }
        Console.WriteLine();
        _cursorTopPosition = 3;
    }

    public static void SeedAbandoned()
    {
        Console.CursorTop = 5;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("It seems like you've abandoned your run! Better luck next time!".PadRight(RIGHT_PADDING));
    }

    public static void WaitingForNewGame()
    {
        if (Console.CursorTop == 6)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Waiting for new game...".PadRight(RIGHT_PADDING));
            ClearLines(1);
        }
    }

    public static void NoSeedsFound()
    {
        Console.CursorLeft = 0;
        Console.CursorTop = 8;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"No seeds found in the seed directory! Make sure your seed is in the correct folder.".PadRight(RIGHT_PADDING));
        ClearLines(10);
    }

    public static void NoFilesFound()
    {
        Console.CursorLeft = 0;
        Console.CursorTop = 8;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Seed directory is empty! Make sure your seed is in the correct folder.".PadRight(RIGHT_PADDING));
        ClearLines(10);
    }

    public static void SeedInformation(SeedData seedData)
    {
        // If status is SEED_INFO_FOUND, data will not be null.
        string filename = seedData.Filename!;
        string[] seedInfo = seedData.SeedInfo!;
        List<string> seedInfoLines = seedData.SeedInfoLines!;
        string flags = "";
        string flagset;
        Console.CursorLeft = 0;
        Console.CursorTop = 8;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Loaded seed: {filename}".PadRight(RIGHT_PADDING));
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        for (int i = 0; i < seedInfo.Length; i++)
        {
            string lineStart = seedInfo[i].Split(" ")[0];
            if (seedInfoLines.Contains(lineStart))
            {
                Console.WriteLine(seedInfo[i].PadRight(RIGHT_PADDING));
            }
            if (lineStart == "Flags")
            {
                flags = seedInfo[i].Substring(10).PadRight(RIGHT_PADDING);
            }
        }
        flagset = FlagHandler.GetFlagset(flags);
        Console.WriteLine();
        Console.WriteLine($"Detected flagset: {flagset}".PadRight(RIGHT_PADDING));
    }

    public static void TrackingRun()
    {
        ResetConsoleCursor();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Now tracking your run...".PadRight(RIGHT_PADDING));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("*** DO NOT close this window! ***".PadRight(RIGHT_PADDING));
        ClearLines(1);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Press ESCAPE to reset this run and start tracking a new seed");
        ClearLines(15);
        _cursorTopPosition = 6;
    }

    public static void RunSuccessful(string finalTime)
    {
        Console.CursorTop = 5;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"JSON file successfully saved. Final time: {finalTime}.".PadRight(RIGHT_PADDING));

#if JSON_DEBUG
        Console.WriteLine("JSON_DEBUG: Press a key to write another JSON file.".PadRight(RIGHT_PADDING));
        Console.ReadKey(); // Wait for a keypress to avoid spamming JSONs
#endif
    }

    public static void ConnectionSuccessful(string uri)
    {
        ClearLines(1);
        ResetConsoleCursor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Connection to SNI successful!".PadRight(RIGHT_PADDING));
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Tracking device URI: {uri}".PadRight(RIGHT_PADDING));
        Console.WriteLine();
    }

    public static void ConnectionError(string message)
    {
        ResetConsoleCursor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message.PadRight(RIGHT_PADDING));
        Console.ForegroundColor = ConsoleColor.White;
        ClearLines(3);
        Console.CursorTop -= 2;
    }

    public static void RetryCounter(int counter)
    {
        Console.Write($"Retrying in {counter} seconds...".PadRight(RIGHT_PADDING));
        Console.CursorLeft = 0;
    }

    public static void DebugInformation(Run run)
    {
        Console.Clear();
        Console.WriteLine($"Game mode: {run.GameState}");
        Console.WriteLine();
        bool isInAShop = run.IsMenuTimerRunning && run.MenuNumber == 3;
        bool isInAMenu = run.IsMenuTimerRunning && run.MenuNumber != 3;
        Console.ForegroundColor = isInAMenu ? ConsoleColor.Blue : ConsoleColor.White;
        Console.WriteLine($"In a menu: {run.IsMenuTimerRunning && run.MenuNumber != 3}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Time spent in a menu: {run.TimeSpentOnMenus}");
        Console.WriteLine($"Times you entered a menu: {run.MenuOpenCounter}");
        Console.WriteLine();

        Console.ForegroundColor = isInAShop ? ConsoleColor.Cyan : ConsoleColor.White;
        Console.WriteLine($"In a shop: {run.IsMenuTimerRunning && run.MenuNumber == 3}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Time spent in a shop: {run.TimeSpentOnShops}");
        Console.WriteLine($"Times you visited a shop: {run.ShopOpenCounter}");
        Console.WriteLine();

        Console.ForegroundColor = run.IsAirshipTimerRunning ? ConsoleColor.DarkYellow : ConsoleColor.White;
        Console.WriteLine($"Flying the airship: {run.IsAirshipTimerRunning}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Time spent flying the airship: {run.TimeSpentOnAirship}");
        Console.WriteLine($"Times you used the airship: {run.AirshipCounter}");
        Console.WriteLine();

        Console.ForegroundColor = run.IsBattleTimerRunning ? ConsoleColor.Red : ConsoleColor.White;
        Console.WriteLine($"In battle: {run.IsBattleTimerRunning}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Time spent battling: {run.TimeSpentOnBattles}");
        Console.WriteLine($"Battles fought: {run.BattlesFought}");
        Console.WriteLine();

        Console.WriteLine($"GP spent: {run.GPSpent} GP");
        Console.WriteLine();

        Console.WriteLine("Last 5 route events:");
        for (int i = 0; i < 5; i++)
        {
            if (run.Route.Count - 1 - i >= 0)
            {
                Console.WriteLine(run.Route[run.Route.Count - 1 - i]);
            }
        }
    }

    public static void CrashInformation(Exception e, string crashlogPath)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e);
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Crash log saved at {crashlogPath}");
        Console.WriteLine();
        Console.Write("Press enter to exit.");
        Console.ReadLine();
    }

    private static void ResetConsoleCursor()
    {
        Console.CursorTop = _cursorTopPosition;
        Console.CursorLeft = 0;
    }

    private static void ClearLines(int amountOfLines)
    {
        for (int i = 0; i < amountOfLines; i++)
        {
            Console.WriteLine("".PadRight(RIGHT_PADDING));
        }
    }

    public static void InvalidSeedDirectory()
    {
        Console.CursorLeft = 0;
        Console.CursorTop = 8;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Invalid/non-existant seed directory - not collecting seed information!".PadRight(RIGHT_PADDING));
    }

    public static void NoSeedDirectory()
    {
        Console.CursorLeft = 0;
        Console.CursorTop = 8;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("No seed directory provided in the config file - not collecting seed information!".PadRight(RIGHT_PADDING));
    }
    public static void NoMatchingSeedInfoFound(string filename)
    {
        Console.CursorLeft = 0;
        Console.CursorTop = 8;
        Console.ForegroundColor = ConsoleColor.Yellow;

        string noMatchingTxt = "No matching .txt found for ";
        string notCollectingInfo = " - not collecting seed info!";
        int maxFilenameLength = RIGHT_PADDING - noMatchingTxt.Length - notCollectingInfo.Length;

        if (filename.Length > maxFilenameLength)
        {
            int charactersToTrim = filename.Length - maxFilenameLength + 3;
            filename = filename.Substring(0, filename.Length - charactersToTrim) + "...";
        }

        Console.WriteLine($"{noMatchingTxt}{filename}{notCollectingInfo}".PadRight(RIGHT_PADDING));
        ClearLines(10);
    }

    /// <summary>
    /// Checks if escape key was pressed and resets Stats Companion.
    /// TODO: this should be moved to an InputHandler of some sorts.
    /// It has no reason to live in the logger.
    /// </summary>
    private static void CheckForEscapeKeypress()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo cki = Console.ReadKey(true);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            if (cki.Key == ConsoleKey.Escape)
            {
                StatsCompanion.ForceRunReset();
            }
        }
    }
}
