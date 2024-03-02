using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using FF6WCToolsLib;
using StatsCompanionLib;

namespace StatsCompanionConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        // Initial console clear.
        Console.Clear();
        
        // Get console app version.
        string? consoleAppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        // Get data from config file.
        NameValueCollection config = ConfigurationManager.AppSettings;
        
        // Set debug mode.
        bool isDebugMode = Convert.ToBoolean(config.Get("debugMode"));
        
        // Component initialization.
        FileHandler fileHandler = new(true);
        SniClient sniClient = new SniClient();
        StatsCompanion statsCompanion = new StatsCompanion(sniClient, fileHandler);
        ConsoleViewer log = new(consoleAppVersion, statsCompanion, sniClient, fileHandler);
        fileHandler.SeedDirectory = (config.Get("seedDirectory")!);
        
        try
        {
            // Execute Stats Companion.
            statsCompanion.Execute(isDebugMode);
        }
        
        catch (Exception e)
        {
            // Write crashlog file
            string crashlogPath = fileHandler.WriteCrashlogFile(DateTime.Now, e.ToString());
            
            // Show crashlog in console
            ConsoleViewer.CrashInformation(e, crashlogPath);
        }
    }
}