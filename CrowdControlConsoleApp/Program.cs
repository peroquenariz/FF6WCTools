using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using FF6WCToolsLib;
using CrowdControlLib;

namespace CrowdControlConsoleApp;

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
        FileHandler fileHandler = new(false);
        SniClient sniClient = new SniClient();
        CrowdControl crowdControl = new CrowdControl(sniClient);
        ConsoleViewer consoleViewer = new(consoleAppVersion, crowdControl, sniClient);

        try
        {
            // Execute Crowd Control
            crowdControl.Execute();
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