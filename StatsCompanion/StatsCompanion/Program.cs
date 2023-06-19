using System;
using System.IO;
using System.Threading;
using System.Text.Json;
using System.Reflection;

namespace StatsCompanion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SniConnection sniConnection = new();
                Run run = new();
                var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                int requestTimer = 0;

                while (true)
                {
                    if (run.SeedHasBeenAbandoned == true)
                    {
                        Console.Clear();
                        Console.WriteLine("It seems like you've abandoned your run! Better luck next time!");
                    }
                    else
                    {
                        Console.WriteLine("Welcome to Stats Companion!");
                        Console.WriteLine($"v{Assembly.GetEntryAssembly()!.GetName().Version}");
                    }
                    Console.WriteLine();

                    // Open a connection to SNI
                    sniConnection.ResetConnection();

                    // Start a new run.
                    run = new();

                    Console.WriteLine("Waiting for new game...");
                    Console.WriteLine();

#if RELEASE
                    // Wait for the player to start a new game.
                    // Only exit the loop if current menu is FF6WC custom pre-game menu and new game has been selected.
                    while (true)
                    {
                        run.MapId = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.MapId, 2)) & 0x1FF;
                        run.MenuNumber = sniConnection.ReadMemory(WCData.MenuNumber, 1)[0];
                        run.NewGameSelected = sniConnection.ReadMemory(WCData.NewGameSelected, 1)[0];
                        if (run.CheckIfRunStarted() == true)
                        {
                            break;
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Stats Companion is now tracking your run...");
                    Console.WriteLine("*** DO NOT close this window! ***");

                    Thread.Sleep(3500); 
#endif

                    // Read character data.
                    run.CharacterData = sniConnection.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
                    run.CharactersBytes = sniConnection.ReadMemory(WCData.CharactersByte, 2);

                    // Get starting characters and commands.
                    run.CharacterCommands = DataHandler.GetCharacterCommands(run.CharacterData);
                    run.StartingCharacters = DataHandler.GetAvailableCharacters(run.CharactersBytes);
                    run.StartingCommands = DataHandler.GetAvailableCommands(run.CharactersBytes, run.CharacterCommands);

                    // Get initial GP count.
                    run.GPCurrent = run.GPPrevious = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.CurrentGP, 3));

                    // Loop while run is in progress.
                    while (run.HasFinished == false)
                    {
                        if (requestTimer % 2 != 0)
                        {
                            // Check if the player is in a menu or shop, track time spent menuing and times they opened a menu.
                            run.EnableDialogWindow = sniConnection.ReadMemory(WCData.EnableDialogWindow, 1)[0];
                            run.GameStatusData = sniConnection.ReadMemory(WCData.NMIJumpCode, 3);
                            run.GetGameStatus();
                            run.CheckIfMenuIsOpen();
                        }
                        
                        // Check if the player is flying the airship, track time spent flying and times they drove it.
                        if (WCData.AirshipMapIds.Contains(run.MapId) && run.EnableDialogWindow != 1)
                        {
                            run.Character1Graphic = sniConnection.ReadMemory(WCData.Character1Graphic, 1)[0];
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
                            run.IsKefkaFight = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.BattleIndex, 2));
                            run.IsKefkaDead = sniConnection.ReadMemory(WCData.EnableKefkaFinalAnimation, 1)[0];

                            // Check if the player is in the party selection menu before final Kefka fight.
                            if (run.IsMenuTimerRunning)
                            {
                                // Get final Kefka character lineup.
                                run.FinalBattleLineup = sniConnection.ReadMemory(WCData.FinalBattleCharacterListStart, 12);
                            }

                            // Get character data.
                            if (!run.IsFinalBattle)
                            {
                                run.CharacterData = sniConnection.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
                                run.CharacterSkillData = sniConnection.ReadMemory(WCData.CharacterSkillData, WCData.CharacterSkillDataSize);
                                run.IsFinalBattle = true;
                            }

                            run.CheckKefkaKill();
                        }
                        
                        // Tzen thief peek WoB.
                        if (run.MapId == 0x132 && run.TzenThiefPeekWob == "Did not check")
                        {
                            run.DialogIndex = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.DialogIndex, 2));
                            if (run.DialogIndex == 1569)
                            {
                                run.DialogWaitingForInput = sniConnection.ReadMemory(WCData.DialogWaitingForInput, 1)[0];
                                run.DialogPointer = sniConnection.ReadMemory(WCData.DialogPointer, 1)[0];
                                run.DialogChoiceSelected = sniConnection.ReadMemory(WCData.DialogChoiceSelected, 1)[0];
                                run.TzenThiefPeekWob = DataHandler.PeekTzenThiefRewardWob(run.DialogWaitingForInput, run.DialogPointer, run.DialogChoiceSelected);
                            }
                        }

                        // All the reads that don't need to happen every frame are checked against requestTimer, to avoid spamming SNI.
                        requestTimer++;
                        if (requestTimer % 10 == 0 || run.HasFinished)
                        {
                            // Check if the player is in a battle, track time spent battling.
                            run.CheckIfInBattle();

                            // If in battle, log encounter in event list.
                            if (run.IsBattleTimerRunning)
                            {
                                run.MonsterBytes = sniConnection.ReadMemory(WCData.MonsterIndexStart, 12);
                                run.LogBattle();
                            }
                        }
                        
                        if (requestTimer > 10)
                        {
#if DEBUG
                            run.WriteDebugInformation();
#endif
                            requestTimer = 0;
                            
                            run.MapId = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.MapId, 2)) & 0x1FF;
                            run.Inventory = sniConnection.ReadMemory(WCData.InventoryStart, WCData.InventorySize);

                            // Read KT unlock and skip status only if the bits haven't been set.
                            if (!run.IsKTSkipUnlocked || !run.IsKefkaTowerUnlocked)
                            {
                                run.KefkaTowerEventByte = sniConnection.ReadMemory(WCData.EventBitStartAddress + 0x093 / 8, 1)[0];
                            }
                            
                            if (run.IsMenuTimerRunning)
                            {
                                run.MenuNumber = sniConnection.ReadMemory(WCData.MenuNumber, 1)[0];
                                run.ScreenDisplayRegister = sniConnection.ReadMemory(WCData.ScreenDisplayRegister, 1)[0]; // Menu fades
                                run.NextMenuState = sniConnection.ReadMemory(WCData.NextMenuState, 1)[0]; // Next menu state
                            }

                            // Add visited maps to the list.
                            run.UpdateMapsVisited();

                            // Update GP spent.
                            if (run.MapId != 3)
                            {
                                run.GPCurrent = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.CurrentGP, 3));
                            }
                            run.UpdateGPSpent();

                            // Only execute on game reset.
                            if (run.MapId == 3)
                            {
                                // Check if the seed has been abandoned.
                                run.NewGameSelected = sniConnection.ReadMemory(WCData.NewGameSelected, 1)[0];
                                if (run.MenuNumber == 9 && run.NewGameSelected == 0)
                                {
                                    run.SeedHasBeenAbandoned = true;
                                    break;
                                }
                                run.IsFinalBattle = false; // In the case a player resets final Kefka and character data changes.
                            }

                            // Count espers that were bought at the Auction House.
                            if (run.MapId == 0x0C8 && !run.InAuctionHouse)
                            {
                                run.EsperCountPrevious = sniConnection.ReadMemory(WCData.EsperCount, 1)[0];
                                run.InAuctionHouse = true;
                            }
                            else if (run.MapId != 0x0C8 && run.InAuctionHouse)
                            {
                                run.EsperCount = sniConnection.ReadMemory(WCData.EsperCount, 1)[0];
                                run.CountAuctionHouseEspersBought();
                                run.InAuctionHouse = false;
                            }

                            // Whelk peek.
                            if (run.MapId == 0x02B && run.IsWhelkPeeked == false)
                            {
                                run.PartyYPosition = sniConnection.ReadMemory(WCData.PartyYPosition, 1)[0];
                                if (run.PartyYPosition <= 32 && run.PartyYPosition >= 30)
                                {
                                    run.IsWhelkPeeked = true;
                                }
                            }

                            // Esper Mountain peek.
                            if (run.MapId == 0x177 && run.IsEsperMountainPeeked == false)
                            {
                                run.EsperMountainPeekByte = sniConnection.ReadMemory(WCData.EventBitStartAddress + 0x17B / 8, 1)[0];
                                run.IsEsperMountainPeeked = DataHandler.CheckBitByOffset(run.EsperMountainPeekByte, 0x17B);
                            }

                            // South Figaro basement peek.
                            if (run.MapId == 0x053 && run.IsSouthFigaroBasementPeeked == false)
                            {
                                run.PartyXPosition = sniConnection.ReadMemory(WCData.PartyXPosition, 1)[0];
                                //byte basementNpcStatus = sniConnection.ReadMemory(WCData.FieldObjectStartAddress + 41 * 0x10, 1)[0];
                                if (run.PartyXPosition >= 0x37 && run.PartyXPosition <= 0x3B)
                                {
                                    run.IsSouthFigaroBasementPeeked = true;
                                }
                            }

                            // Tzen thief peek WoR.
                            if (run.MapId == 0x131 && run.TzenThiefPeekWor == "Did not check")
                            {
                                run.DialogIndex = DataHandler.ConcatenateByteArray(sniConnection.ReadMemory(WCData.DialogIndex, 2));
                                run.TzenThiefPeekWor = DataHandler.PeekTzenThiefRewardWor(run.DialogIndex);
                            }

                            // Check if Tzen thief was bought, and if it was an esper or an item.
                            // Works by checking esper changes against Tzen Thief bit.
                            if ((run.MapId == 0x131 || run.MapId == 0x132) && run.TzenThiefBought == "")
                            {
                                run.PartyYPosition = sniConnection.ReadMemory(WCData.PartyYPosition, 1)[0];
                                if (run.PartyYPosition < 7)
                                {
                                    run.EsperCount = sniConnection.ReadMemory(WCData.EsperCount, 1)[0];
                                    run.TzenThiefBit = DataHandler.CheckBitByOffset(sniConnection.ReadMemory(WCData.EventBitStartAddress + 0x27c / 8, 1)[0], 0x27c);
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

                            // Check for specific items in the inventory.
                            run.CheckForItemsInInventory();

                            // Check if Kefka tower has been unlocked and log the time.
                            run.CheckKefkaTowerUnlock();

                            // Check if Kefka tower skip has been unlocked and log the time.
                            run.CheckKTSkipUnlock();

                            // Check if the player has entered Kefka tower and KT is unlocked, log the time.
                            // Either skip or regular KT is logged, whatever happens first.
                            run.CheckKefkaTowerStart();
                        }
                    }

                    // If the seed has been abandoned, start tracking the new run.
                    if (run.SeedHasBeenAbandoned == true)
                    {
                        continue;
                    }

                    // If KT skip was unlocked, format a string with the time.
                    if (run.IsKTSkipUnlocked)
                    {
                        run.KtSkipUnlockTimeString = (run.KtSkipUnlockTime - run.StartTime).ToString(@"hh\:mm\:ss\.ff");
                    }

                    // Add Kefka kill time to event list.
                    string kefkaKillTime = (run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@"hh\:mm\:ss");
                    run.Route.Add(("Kefka kill", kefkaKillTime));

                    // Get data after Kefka kill.
                    run.CharactersBytes = sniConnection.ReadMemory(WCData.CharactersByte, 2);
                    run.DragonsBytes = sniConnection.ReadMemory(WCData.DragonsByte, 2);
                    run.BossCount = sniConnection.ReadMemory(WCData.BossCount, 1)[0];
                    run.EsperCount = sniConnection.ReadMemory(WCData.EsperCount, 1)[0];
                    run.CheckCount = sniConnection.ReadMemory(WCData.CheckCount, 1)[0];
                    run.DragonCount = sniConnection.ReadMemory(WCData.DragonCount, 1)[0];
                    run.ChestData = sniConnection.ReadMemory(WCData.ChestDataStart, WCData.ChestDataSize);
                    run.EventBitData = sniConnection.ReadMemory(WCData.EventBitStartAddress, WCData.EventBitDataSize);
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
                    Arguments runArguments = new(run);
                    string runArgumentsStr = JsonSerializer.Serialize(runArguments, jsonOptions);

                    // Create a runs directory if it doesn't exist.
                    if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\runs"))
                    {
                        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\runs");
                    }

                    // Create a timestamped filename.
                    string jsonFilename = $"{Directory.GetCurrentDirectory()}\\runs\\{run.EndTime.ToString("yyyy_MM_dd - HH_mm_ss")}.json";

                    // Write to a .json file.
                    File.WriteAllText(jsonFilename, runArgumentsStr);

                    Console.Clear();
                    Console.WriteLine($"The clown is dead, GG! Final time is {(run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@"hh\:mm\:ss\.ff")}");
                    Console.WriteLine($"Run successfully saved at {jsonFilename}");
                    Console.WriteLine();
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                // Create a crashlog directory if it doesn't exist.
                if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\crashlog"))
                {
                    Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\crashlog");
                }
                
                // Crash log path.
                string crashlogPath = $"{Directory.GetCurrentDirectory()}\\crashlog\\crashlog - {DateTime.Now.ToString("yyyy_MM_dd - HH_mm_ss")}.txt";
                // Print exception to console.
                Console.WriteLine(e);
                Console.WriteLine();
                File.WriteAllText(crashlogPath, e.ToString());
                Console.WriteLine();
                Console.WriteLine($"Crash log saved at {crashlogPath}");
                Console.WriteLine();
                Console.Write("Press enter to exit.");
                Console.ReadLine();
            }
        }
    }
}