﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using System.Reflection;
using FF6WCToolsLib;
using StatsCompanionLib;

namespace StatsCompanionConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        StatsCompanion statsCompanion = new StatsCompanion();
        string? consoleAppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        string? libVersion = statsCompanion.LibVersion;
        NameValueCollection config = ConfigurationManager.AppSettings;
        FileHandler fileHandler = new(config.Get("seedDirectory")!);
        SniClient sniClient = new();
        Log log = new(consoleAppVersion, libVersion, sniClient, fileHandler);
        bool debugMode = Convert.ToBoolean(config.Get("debugMode"));

        try
        {
            Run run = new();
            
            while (true)
            {
                if (debugMode)
                {
                    Console.Clear();
                    log.Version(debugMode);
                }

                Log.cursorTopPosition = 3;
                bool isValidDirectory = true;

                
                // Open a connection to SNI
                sniClient.ResetConnection();

                if (run.SeedHasBeenAbandoned)
                {
                    Log.SeedAbandoned();
                }

                // Start a new run.
                run = new();
#if !DEBUG
                // Wait for the player to start a new game.
                // Only exit the loop if current menu is FF6WC custom pre-game menu and new game has been selected.
                while (true)
                {
                    if (Console.CursorTop == 6)
                    {
                        Log.WaitingForNewGame();
                    }

                    // TODO: move this to FileHandler and make it a function (get rid of copypasted auto-reset code)
                    if (isValidDirectory == true &&
                        DateTime.Now - fileHandler.LastDirectoryRefresh > fileHandler.RefreshInterval)
                    {
                        isValidDirectory = fileHandler.UpdateLastSeed(run.SeedInfo, out string[] updatedSeedInfo);
                        run.SeedInfo = updatedSeedInfo;
                    }
                    run.MapId = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.MapId, 2)) & 0x1FF;
                    run.MenuNumber = sniClient.ReadMemory(WCData.MenuNumber, 1)[0];
                    run.NewGameSelected = sniClient.ReadMemory(WCData.NewGameSelected, 1)[0];
                    if (run.CheckIfRunStarted() == true)
                    {
                        break;
                    }
                }
#endif
                Log.TrackingRun(); // TODO: move this to an event
                
                Thread.Sleep(3500);

                // Read character data.
                run.CharacterData = sniClient.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
                run.CharactersBytes = sniClient.ReadMemory(WCData.CharactersByte, 2);

                // Get starting characters and commands.
                run.CharacterCommands = DataHandler.GetCharacterCommands(run.CharacterData);
                run.StartingCharacters = DataHandler.GetAvailableCharacters(run.CharactersBytes);
                run.StartingCommands = DataHandler.GetAvailableCommands(run.CharactersBytes, run.CharacterCommands);

                // Get initial GP count.
                run.GPCurrent = run.GPPrevious = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.CurrentGP, 3));

                // Loop while run is in progress.
                while (run.HasFinished == false)
                {
                    if (sniClient.RequestTimer % 2 != 0)
                    {
                        // Check if the player is in a menu or shop, track time spent menuing and times they opened a menu.
                        run.EnableDialogWindow = sniClient.ReadMemory(WCData.EnableDialogWindow, 1)[0];
                        run.GameStatusData = sniClient.ReadMemory(WCData.NMIJumpCode, 3);
                        run.GetGameStatus();
                        run.CheckIfMenuIsOpen();
                    }
                    
                    // Check if the player is flying the airship, track time spent flying and times they drove it.
                    if (WCData.AirshipMapIds.Contains(run.MapId) && run.EnableDialogWindow != 1)
                    {
                        run.Character1Graphic = sniClient.ReadMemory(WCData.Character1Graphic, 1)[0];
                        run.CheckIfFlyingAirship();
                    }

                    // Check only after reaching Kefka's Lair.
                    // Log Kefka start time.
                    if (run.MapId == 0x150 && run.SteppedOnKTSwitches == false)
                    {
                        run.LogKefkaStartTime();
                    }

                    // Check for final Kefka kill.
                    if (run.MapId == 0x164)
                    {
                        run.IsKefkaFight = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.BattleIndex, 2));
                        run.IsKefkaDead = sniClient.ReadMemory(WCData.EnableKefkaFinalAnimation, 1)[0];

                        // Check if the player is in the party selection menu before final Kefka fight.
                        if (run.IsMenuTimerRunning)
                        {
                            // Get final Kefka character lineup.
                            run.FinalBattleLineup = sniClient.ReadMemory(WCData.FinalBattleCharacterListStart, 12);
                        }

                        // Get character data.
                        if (!run.IsFinalBattle)
                        {
                            run.CharacterData = sniClient.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
                            run.CharacterSkillData = sniClient.ReadMemory(WCData.CharacterSkillData, WCData.CharacterSkillDataSize);
                            run.IsFinalBattle = true;
                        }

                        run.CheckKefkaKill();
                    }
                    
                    // Tzen thief peek WoB.
                    if (run.MapId == 0x132 && run.TzenThiefPeekWob == "Did_not_check") // TODO: get rid of underscores here too!
                    {
                        run.DialogIndex = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.DialogIndex, 2));
                        if (run.DialogIndex == 1569)
                        {
                            run.DialogWaitingForInput = sniClient.ReadMemory(WCData.DialogWaitingForInput, 1)[0];
                            run.DialogPointer = sniClient.ReadMemory(WCData.DialogPointer, 1)[0];
                            run.DialogChoiceSelected = sniClient.ReadMemory(WCData.DialogChoiceSelected, 1)[0];
                            run.TzenThiefPeekWob = DataHandler.PeekTzenThiefRewardWob(run.DialogWaitingForInput, run.DialogPointer, run.DialogChoiceSelected);
                        }
                    }

                    // All the reads that don't need to happen every frame are checked against RequestTimer, to avoid spamming SNI.
                    sniClient.RequestTimer++;
                    if (sniClient.RequestTimer % 10 == 0 || run.HasFinished)
                    {
                        // Check if the player is in a battle, track time spent battling.
                        run.CheckIfInBattle();

                        // If in battle, log encounter in event list.
                        if (run.IsBattleTimerRunning)
                        {
                            run.MonsterBytes = sniClient.ReadMemory(WCData.MonsterIndexStart, 12);
                            run.LogBattle();
                        }
                    }
                    
                    if (sniClient.RequestTimer > 10)
                    {
                        sniClient.RequestTimer = 0;
                        
                        run.MapId = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.MapId, 2)) & 0x1FF;
                        run.Inventory = sniClient.ReadMemory(WCData.InventoryStart, WCData.InventorySize);

                        // Read KT unlock and skip status only if the bits haven't been set.
                        if (!run.IsKTSkipUnlocked || !run.IsKefkaTowerUnlocked)
                        {
                            run.KefkaTowerEventByte = sniClient.ReadMemory(WCData.EventBitStartAddress + 0x093 / 8, 1)[0];
                        }
                        
                        if (run.IsMenuTimerRunning)
                        {
                            run.MenuNumber = sniClient.ReadMemory(WCData.MenuNumber, 1)[0];
                            run.ScreenDisplayRegister = sniClient.ReadMemory(WCData.ScreenDisplayRegister, 1)[0]; // Menu fades
                            run.NextMenuState = sniClient.ReadMemory(WCData.NextMenuState, 1)[0]; // Next menu state
                        }

                        // Add visited maps to the list.
                        run.UpdateMapsVisited();

                        // Update GP spent.
                        if (run.MapId != 3)
                        {
                            run.GPCurrent = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.CurrentGP, 3));
                        }
                        run.UpdateGPSpent();

                        // Only execute on game reset.
                        if (run.MapId == 3)
                        {
                            run.IsReset = true;
                            run.IsFinalBattle = false; // In the case a player resets final Kefka and character data changes.
                        }
                        else if (run.IsReset)
                        {
                            run.CheckResetFalsePositive();
                        }

                        // Count espers that were bought at the Auction House.
                        if (run.MapId == 0x0C8 && !run.InAuctionHouse)
                        {
                            run.EsperCountPrevious = sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                            run.InAuctionHouse = true;
                        }
                        else if (run.MapId != 0x0C8 && run.InAuctionHouse)
                        {
                            run.EsperCount = sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                            run.CountAuctionHouseEspersBought();
                            run.InAuctionHouse = false;
                        }

                        // Whelk peek.
                        if (run.MapId == 0x02B && run.IsWhelkPeeked == false)
                        {
                            run.PartyYPosition = sniClient.ReadMemory(WCData.PartyYPosition, 1)[0];
                            if (run.PartyYPosition <= 32 && run.PartyYPosition >= 30)
                            {
                                run.IsWhelkPeeked = true;
                            }
                        }

                        // Esper Mountain peek.
                        if (run.MapId == 0x177 && run.IsEsperMountainPeeked == false)
                        {
                            run.EsperMountainPeekByte = sniClient.ReadMemory(WCData.EventBitStartAddress + 0x17B / 8, 1)[0];
                            run.IsEsperMountainPeeked = DataHandler.CheckBitByOffset(run.EsperMountainPeekByte, 0x17B);
                        }

                        // South Figaro basement peek.
                        if (run.MapId == 0x053 && run.IsSouthFigaroBasementPeeked == false)
                        {
                            run.PartyXPosition = sniClient.ReadMemory(WCData.PartyXPosition, 1)[0];
                            //byte basementNpcStatus = sniConnection.ReadMemory(WCData.FieldObjectStartAddress + 41 * 0x10, 1)[0];
                            if (run.PartyXPosition >= 0x37 && run.PartyXPosition <= 0x3B)
                            {
                                run.IsSouthFigaroBasementPeeked = true;
                            }
                        }

                        // Tzen thief peek WoR.
                        if (run.MapId == 0x131 && run.TzenThiefPeekWor == "Did_not_check")
                        {
                            run.DialogIndex = DataHandler.ConcatenateByteArray(sniClient.ReadMemory(WCData.DialogIndex, 2));
                            run.TzenThiefPeekWor = DataHandler.PeekTzenThiefRewardWor(run.DialogIndex);
                        }

                        // Check if Tzen thief was bought, and if it was an esper or an item.
                        // Works by checking esper changes against Tzen Thief bit.
                        // TODO: cleanup and properly comment this code!
                        if ((run.MapId == 0x131 || run.MapId == 0x132) && run.TzenThiefBought == "")
                        {
                            run.PartyYPosition = sniClient.ReadMemory(WCData.PartyYPosition, 1)[0];
                            if (run.PartyYPosition < 7)
                            {
                                run.EsperCount = sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                                run.TzenThiefBit = DataHandler.CheckBitByOffset(sniClient.ReadMemory(WCData.EventBitStartAddress + 0x27c / 8, 1)[0], 0x27c);
                                run.TzenThiefBought = DataHandler.CheckTzenThiefBought(run.EsperCount, run.EsperCountPrevious, run.TzenThiefBit);
                                if (run.InTzenThiefArea == false)
                                {
                                    run.EsperCountPrevious = run.EsperCount;
                                    run.InTzenThiefArea = true;
                                }
                            }
                            else
                            {
                                run.InTzenThiefArea = false;
                            }
                        }

                        // Generic event bit peeks.
                        run.EventBitData = sniClient.ReadMemory(WCData.EventBitStartAddress, WCData.EventBitDataSize);
                        run.CheckEventBitPeeks();

                        // Check for specific items in the inventory.
                        run.CheckForItemsInInventory();

                        // Check if Kefka tower has been unlocked and log the time.
                        run.CheckKefkaTowerUnlock();

                        // Check if Kefka tower skip has been unlocked and log the time.
                        run.CheckKTSkipUnlock();

                        // Check if the player has entered Kefka tower and KT is unlocked, log the time.
                        // Either skip or regular KT is logged, whatever happens first.
                        run.CheckKefkaTowerStart();

                        // If escape key is pressed, abandon the seed.
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo cki = Console.ReadKey(true);
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(true);
                            }

                            if (cki.Key == ConsoleKey.Escape)
                            {
                                run.SeedHasBeenAbandoned = true;
                                break;
                            }
                        }

                        // If a new seed is found in the directory, abandon the seed.
                        // TODO: move this to FileHandler and make it a function (get rid of copypasted auto-reset code)
                        if (isValidDirectory == true &&
                            DateTime.Now - fileHandler.LastDirectoryRefresh > fileHandler.RefreshInterval)
                        {
                            string previousSeed = fileHandler.LastLoadedSeed;
                            isValidDirectory = fileHandler.UpdateLastSeed(run.SeedInfo, out string[] updatedSeedInfo, false);

                            run.SeedInfo = updatedSeedInfo;
                            if (fileHandler.LastLoadedSeed != previousSeed)
                            {
                                run.SeedHasBeenAbandoned = true;
                                break;
                            }
                        }
                        
                        if (debugMode)
                        {
                            Log.DebugInformation(run);
                        }
#if JSON_DEBUG
                        run.EndTime = DateTime.Now;
                        run.HasFinished = true;
#endif
                    }
                }

                // If the seed has been abandoned, start tracking the new run.
                if (run.SeedHasBeenAbandoned == true)
                {
                    fileHandler.ResetLastLoadedSeed();
                    continue;
                }

                // If KT skip was unlocked, format a string with the time.
                if (run.IsKTSkipUnlocked)
                {
                    run.KtSkipUnlockTimeString = (run.KtSkipUnlockTime - run.StartTime).ToString(@"hh\:mm\:ss");
                }

                // Add Kefka kill time to event list.
                // TODO: move this to Run class.
                string kefkaKillTime = (run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@"hh\:mm\:ss");
                run.Route.Add(("Kefka kill", kefkaKillTime));

                // Get data after Kefka kill.
                run.CharactersBytes = sniClient.ReadMemory(WCData.CharactersByte, 2);
                run.DragonsBytes = sniClient.ReadMemory(WCData.DragonsByte, 2);
                run.BossCount = sniClient.ReadMemory(WCData.BossCount, 1)[0];
                run.EsperCount = sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                run.CheckCount = sniClient.ReadMemory(WCData.CheckCount, 1)[0];
                run.DragonCount = sniClient.ReadMemory(WCData.DragonCount, 1)[0];
                run.ChestData = sniClient.ReadMemory(WCData.ChestDataStart, WCData.ChestDataSize);
                run.EventBitData = sniClient.ReadMemory(WCData.EventBitStartAddress, WCData.EventBitDataSize);
                run.CharacterCount = DataHandler.GetCharacterCount(run.CharactersBytes);
                run.ChestCount = DataHandler.GetChestCount(run.ChestData);
                run.CharacterMaxLevel = DataHandler.GetMaximumCharacterLevel(run.CharacterData);
                run.DragonsKilled = DataHandler.GetDragonsKilled(run.DragonsBytes);
                run.WonColiseumMatch = DataHandler.CheckBitByOffset(run.EventBitData[0x1ef / 8], 0x1ef);

                // Get checks data.
                run.GetListOfCompletedChecks();
                run.GetListOfPeekedChecks();
                run.GetTzenThiefData();
                run.CreateAuctionHouseString();

                // Get Coliseum data.
                run.CheckColiseumVisit();
#if JSON_DEBUG
                run.CharacterData = sniConnection.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
                run.CharacterSkillData = sniConnection.ReadMemory(WCData.CharacterSkillData, WCData.CharacterSkillDataSize);
                run.FinalBattleLineup = sniConnection.ReadMemory(WCData.FinalBattleCharacterListStart, 12);
#endif
                // Get final 4-character lineup and skills data.
                run.GetFinalLineup();
                run.GetSwdTechList();
                run.GetBlitzList();
                run.GetLoreList();

                // Get final battle prep.
                run.CheckForMute();
                run.CheckForInstantDeath();
                run.CheckForCalmnessProtection();
                // Make a timestamped route that is usable in JSON format.
                // Get reset count.
                run.CreateTimestampedRoute();

                // Create JSON string with the run data.
                RunJson runJson = new(run, consoleAppVersion, fileHandler.LastLoadedSeed);
                
                // Write JSON file.
                fileHandler.WriteJSONFile(run.EndTime, runJson);
                
                // Show final time in console.
                Log.RunSuccessful((run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@"hh\:mm\:ss\.fff"));
                fileHandler.ResetLastLoadedSeed();
#if JSON_DEBUG
                Console.ReadKey();
#endif
            }
        }
        
        catch (Exception e)
        {
            // Write crashlog file
            string crashlogPath = fileHandler.WriteCrashlogFile(DateTime.Now, e.ToString());
            
            // Show crashlog in console
            Log.CrashInformation(e, crashlogPath);
            
            Console.ReadLine();
        }
    }
}