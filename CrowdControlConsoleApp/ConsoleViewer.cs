using System;
using FF6WCToolsLib;
using CrowdControlLib;

namespace CrowdControlConsoleApp;

/// <summary>
/// Handles the console view.
/// </summary>
internal class ConsoleViewer
{
    private const int RIGHT_PADDING = 90;
    private const string WINDOW_TITLE = "FF6WC Crowd Control";

    private readonly string? _appVersion;
    private readonly string? _libVersion;

    private static int _cursorTopPosition;

    public ConsoleViewer(string? appVersion, CrowdControl crowdControl, SniClient sniClient)
    {
        Console.CursorVisible = false;
        Console.Title = WINDOW_TITLE;
        _appVersion = appVersion;
        _libVersion = crowdControl.LibVersion;

        Version();
    }

    public void Version(bool isDebugMode = false)
    {
        if (isDebugMode) Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        //string version = $"Stats Companion v{_libVersion} (lib) | v{_appVersion} (app)";
        string version = $"FF6WC Crowd Control v{_libVersion}";
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
}