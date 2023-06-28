using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace StatsCompanion
{
    /// <summary>
    /// Handles all console log messages.
    /// </summary>
    
    internal class Log
    {
        private const int RightPadding = 90;
        private const string WindowTitle = "Stats Companion";
        
        private readonly string _appVersion;

        public static int cursorTopPosition;
        
        public Log(string appVersion)
        {
            Console.CursorVisible = false;
            Console.Title = WindowTitle;
            _appVersion = appVersion;
            AssemblyVersion();
            cursorTopPosition = 3;
        }

        private void AssemblyVersion()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Stats Companion v{_appVersion}".PadRight(RightPadding));
            Console.WriteLine("------------------------");
            Console.WriteLine();
        }

        public static void SeedAbandoned()
        {
            Console.CursorTop = 5;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("It seems like you've abandoned your run! Better luck next time!".PadRight(RightPadding));
        }

        public static void WaitingForNewGame()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Waiting for new game...".PadRight(RightPadding));
            ClearLines(1);
        }

        public static void NoSeedsFound()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 8;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"No seeds found in the seed directory.".PadRight(60));
            ClearLines(10);
        }

        public static void SeedInformation(FileInfo lastCreatedZip, string[] seedInfo, List<string> seedInfoLines)
        {
            string flags = "";
            string flagset;
            Console.CursorLeft = 0;
            Console.CursorTop = 8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Loaded seed: {lastCreatedZip.Name}".PadRight(RightPadding));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            for (int i = 0; i < seedInfo.Length; i++)
            {
                string lineStart = seedInfo[i].Split(" ")[0];
                if (seedInfoLines.Contains(lineStart))
                {
                    Console.WriteLine(seedInfo[i]);
                }
                if (lineStart == "Flags")
                {
                    flags = seedInfo[i].Substring(10);
                }
            }
            flagset = Arguments.GetFlagset(flags);
            Console.WriteLine();
            Console.WriteLine($"Detected flagset: {flagset}".PadRight(RightPadding));
        }

        public static void TrackingRun()
        {
            ResetConsoleCursor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Now tracking your run...".PadRight(RightPadding));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*** DO NOT close this window! ***".PadRight(RightPadding));
            ClearLines(10);
        }

        public static void RunSuccessful(string finalTime)
        {
            Console.CursorTop = 5;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"JSON file successfully saved. Final time: {finalTime}.".PadRight(RightPadding));
        }

        public static void ConnectionSuccessful(string uri, string requestAddressSpace)
        {
            ResetConsoleCursor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connection to SNI successful!".PadRight(RightPadding));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Tracking device URI: {uri}".PadRight(RightPadding));
            Console.WriteLine();
        }

        public static void ConnectionError(string message)
        {
            ResetConsoleCursor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message.PadRight(RightPadding));
            Console.ForegroundColor = ConsoleColor.White;
            ClearLines(3);
            Console.CursorTop -= 2;
            for (int i = 5; i > 0; i--)
            {
                Console.Write($"Retrying in {i} seconds...".PadRight(RightPadding));
                Thread.Sleep(1000);
                Console.CursorLeft = 0;
            }
            ClearLines(1);
        }

        public static void DebugInformation(Run run)
        {
            Console.Clear();

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
        }

        private static void ResetConsoleCursor()
        {
            Console.CursorTop = cursorTopPosition;
            Console.CursorLeft = 0;
        }

        private static void ClearLines(int amountOfLines)
        {
            for (int i = 0; i < amountOfLines; i++)
            {
                Console.WriteLine("".PadRight(RightPadding));
            }
        }

        public static void InvalidSeedDirectory()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Invalid/non-existant seed directory - not collecting seed information!");
        }

        public static void NoSeedDirectory()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("No seed directory provided in the config file - not collecting seed information!");
        }
    }
}
