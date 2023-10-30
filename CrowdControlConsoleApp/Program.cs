using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using FF6WCToolsLib;
using CrowdControlLib;
using TwitchChatbot;

namespace CrowdControlConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
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
        Chatbot chatbot = new Chatbot(config);
        CrowdControl crowdControl = new CrowdControl(sniClient, chatbot.CrowdControlMessageQueue);
        ConsoleViewer consoleViewer = new(consoleAppVersion, crowdControl, sniClient, chatbot);

        try
        {
            // Open Twitch Chatbot
            Task chatbotTask = chatbot.StartAsync();
            
            // Execute Crowd Control
            Task crowdControlTask = crowdControl.ExecuteAsync();

            await chatbotTask;
            await crowdControlTask;
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