using System;
using System.Reflection;
using System.Threading;
using FF6WCToolsLib;

namespace StatsCompanionLib;

/// <summary>
/// Stats Companion core.
/// </summary>
public class StatsCompanion
{
    public event EventHandler? OnExecutionLoopStart;
    public event EventHandler? OnShowVersionDebug;
    public event EventHandler? OnSeedAbandoned;
    public event EventHandler? OnWaitingForNewGame;
    public event EventHandler? OnTrackingRun;
    public event EventHandler? OnCheckKeypress;
    public event EventHandler<DebugInformationEventArgs>? OnShowDebugInformation;
    public event EventHandler<RunSuccessfulEventArgs>? OnRunSuccessful;
    
    private readonly SniClient _sniClient;
    private readonly FileHandler _fileHandler;
    
    private readonly string? _libVersion;
    private static bool _enableForceRunReset;

    public string? LibVersion { get => _libVersion; }

    public StatsCompanion(SniClient sniClient, FileHandler fileHandler)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;
        _fileHandler = fileHandler;
    }

    public void Execute(bool isDebugMode)
    {
        Run run = new Run();

        while (true)
        {
            if (isDebugMode)
            {
                OnShowVersionDebug?.Invoke(this, EventArgs.Empty);
            }

            OnExecutionLoopStart?.Invoke(this, EventArgs.Empty);
            
            _enableForceRunReset = false;
            
            bool isValidDirectory = true; // TODO: possibly move this to FileHandler?

            // Open a connection to SNI
            _sniClient.ResetConnection();

            if (run.SeedHasBeenAbandoned)
            {
                OnSeedAbandoned?.Invoke(this, EventArgs.Empty);
            }

            // Start a new run.
            run = new Run();
#if RELEASE
            // Wait for the player to start a new game.
            // Only exit the loop if current menu is FF6WC custom pre-game menu and new game has been selected.

            while (true)
            {
                OnWaitingForNewGame?.Invoke(this, EventArgs.Empty);

                // TODO: move this to FileHandler and make it a function (get rid of copypasted auto-reset code)
                if (isValidDirectory &&
                    DateTime.Now - _fileHandler.LastDirectoryRefresh > _fileHandler.RefreshInterval)
                {
                    isValidDirectory = _fileHandler.UpdateLastSeed(run.SeedInfo, out string[] updatedSeedInfo);
                    run.SeedInfo = updatedSeedInfo;
                }
                run.MapId = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.MapId, 2)) & 0x1FF;
                run.MenuNumber = _sniClient.ReadMemory(WCData.MenuNumber, 1)[0];
                run.NewGameSelected = _sniClient.ReadMemory(WCData.NewGameSelected, 1)[0];
                if (run.CheckIfRunStarted())
                {
                    break;
                }
            }
#endif
            OnTrackingRun?.Invoke(this, EventArgs.Empty);

            Thread.Sleep(3500);

            // Read character data.
            run.CharacterData = _sniClient.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
            run.CharactersBytes = _sniClient.ReadMemory(WCData.CharactersByte, 2);

            // Get starting characters and commands.
            run.CharacterCommands = DataHandler.GetCharacterCommands(run.CharacterData);
            run.StartingCharacters = DataHandler.GetAvailableCharacters(run.CharactersBytes);
            run.StartingCommands = DataHandler.GetAvailableCommands(run.CharactersBytes, run.CharacterCommands);

            // Get initial GP count.
            run.GPCurrent = run.GPPrevious = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.CurrentGP, 3));

            // Loop while run is in progress.
            while (!run.HasFinished)
            {
                if (_sniClient.RequestTimer % 2 != 0)
                {
                    // Check if the player is in a menu or shop, track time spent menuing and times they opened a menu.
                    run.EnableDialogWindow = _sniClient.ReadMemory(WCData.EnableDialogWindow, 1)[0];
                    run.GameStatusData = _sniClient.ReadMemory(WCData.NMIJumpCode, 3);
                    run.GetGameStatus();
                    run.CheckIfMenuIsOpen();
                }

                // Check if the player is flying the airship, track time spent flying and times they drove it.
                if (WCData.AirshipMapIds.Contains(run.MapId) && run.EnableDialogWindow != 1)
                {
                    run.Character1Graphic = _sniClient.ReadMemory(WCData.Character1Graphic, 1)[0];
                    run.CheckIfFlyingAirship();
                }

                // Check only after reaching Kefka's Lair.
                // Log Kefka start time.
                if (run.MapId == 0x150 && !run.SteppedOnKTSwitches)
                {
                    run.LogKefkaStartTime();
                }

                // Check for final Kefka kill.
                if (run.MapId == 0x164)
                {
                    run.IsKefkaFight = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.BattleIndex, 2));
                    run.IsKefkaDead = _sniClient.ReadMemory(WCData.EnableKefkaFinalAnimation, 1)[0];

                    // Check if the player is in the party selection menu before final Kefka fight.
                    if (run.IsMenuTimerRunning)
                    {
                        // Get final Kefka character lineup.
                        run.FinalBattleLineup = _sniClient.ReadMemory(WCData.FinalBattleCharacterListStart, 12);
                    }

                    // Get character data.
                    if (!run.IsFinalBattle)
                    {
                        run.CharacterData = _sniClient.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
                        run.CharacterSkillData = _sniClient.ReadMemory(WCData.CharacterSkillData, WCData.CharacterSkillDataSize);
                        run.IsFinalBattle = true;
                    }

                    run.CheckKefkaKill();
                }

                // Tzen thief peek WoB.
                if (run.MapId == 0x132 && run.TzenThiefPeekWob == "Did_not_check") // TODO: get rid of underscores here too!
                {
                    run.DialogIndex = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.DialogIndex, 2));
                    if (run.DialogIndex == 1569)
                    {
                        run.DialogWaitingForInput = _sniClient.ReadMemory(WCData.DialogWaitingForInput, 1)[0];
                        run.DialogPointer = _sniClient.ReadMemory(WCData.DialogPointer, 1)[0];
                        run.DialogChoiceSelected = _sniClient.ReadMemory(WCData.DialogChoiceSelected, 1)[0];
                        run.TzenThiefPeekWob = DataHandler.PeekTzenThiefRewardWob(run.DialogWaitingForInput, run.DialogPointer, run.DialogChoiceSelected);
                    }
                }

                // All the reads that don't need to happen every frame are checked against RequestTimer, to avoid spamming SNI.
                _sniClient.RequestTimer++;
                if (_sniClient.RequestTimer % 10 == 0 || run.HasFinished)
                {
                    // Check if the player is in a battle, track time spent battling.
                    run.CheckIfInBattle();

                    // If in battle, log encounter in event list.
                    if (run.IsBattleTimerRunning)
                    {
                        run.MonsterBytes = _sniClient.ReadMemory(WCData.MonsterIndexStart, 12);
                        run.LogBattle();
                    }
                }

                if (_sniClient.RequestTimer > 10)
                {
                    _sniClient.RequestTimer = 0;

                    run.MapId = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.MapId, 2)) & 0x1FF;
                    run.Inventory = _sniClient.ReadMemory(WCData.InventoryStart, WCData.InventorySize);

                    // Read KT unlock and skip status only if the bits haven't been set.
                    if (!run.IsKTSkipUnlocked || !run.IsKefkaTowerUnlocked)
                    {
                        run.KefkaTowerEventByte = _sniClient.ReadMemory(WCData.EventBitStartAddress + 0x093 / 8, 1)[0];
                    }

                    if (run.IsMenuTimerRunning)
                    {
                        run.MenuNumber = _sniClient.ReadMemory(WCData.MenuNumber, 1)[0];
                        run.ScreenDisplayRegister = _sniClient.ReadMemory(WCData.ScreenDisplayRegister, 1)[0]; // Menu fades
                        run.NextMenuState = _sniClient.ReadMemory(WCData.NextMenuState, 1)[0]; // Next menu state
                    }

                    // Add visited maps to the list.
                    run.UpdateMapsVisited();

                    // Update GP spent.
                    if (run.MapId != 3)
                    {
                        run.GPCurrent = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.CurrentGP, 3));
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
                        // If next menu after reset is not the load game menu,
                        // remove the reset from the list.
                        run.CheckResetFalsePositive();
                    }

                    // Count espers that were bought at the Auction House.
                    if (run.MapId == 0x0C8 && !run.InAuctionHouse)
                    {
                        run.EsperCountPrevious = _sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                        run.InAuctionHouse = true;
                    }
                    else if (run.MapId != 0x0C8 && run.InAuctionHouse)
                    {
                        run.EsperCount = _sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                        run.CountAuctionHouseEspersBought();
                        run.InAuctionHouse = false;
                    }

                    // Whelk peek.
                    if (run.MapId == 0x02B && !run.IsWhelkPeeked)
                    {
                        run.PartyYPosition = _sniClient.ReadMemory(WCData.PartyYPosition, 1)[0];
                        if (run.PartyYPosition <= 32 && run.PartyYPosition >= 30)
                        {
                            run.IsWhelkPeeked = true;
                        }
                    }

                    // Esper Mountain peek.
                    if (run.MapId == 0x177 && !run.IsEsperMountainPeeked)
                    {
                        run.EsperMountainPeekByte = _sniClient.ReadMemory(WCData.EventBitStartAddress + 0x17B / 8, 1)[0];
                        run.IsEsperMountainPeeked = DataHandler.CheckBitByOffset(run.EsperMountainPeekByte, 0x17B);
                    }

                    // South Figaro basement peek.
                    if (run.MapId == 0x053 && !run.IsSouthFigaroBasementPeeked)
                    {
                        run.PartyXPosition = _sniClient.ReadMemory(WCData.PartyXPosition, 1)[0];
                        //byte basementNpcStatus = sniConnection.ReadMemory(WCData.FieldObjectStartAddress + 41 * 0x10, 1)[0];
                        if (run.PartyXPosition >= 0x37 && run.PartyXPosition <= 0x3B)
                        {
                            run.IsSouthFigaroBasementPeeked = true;
                        }
                    }

                    // Tzen thief peek WoR.
                    if (run.MapId == 0x131 && run.TzenThiefPeekWor == "Did_not_check")
                    {
                        run.DialogIndex = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(WCData.DialogIndex, 2));
                        run.TzenThiefPeekWor = DataHandler.PeekTzenThiefRewardWor(run.DialogIndex);
                    }

                    // Check if Tzen thief was bought, and if it was an esper or an item.
                    // Works by checking esper changes against Tzen Thief bit.
                    // TODO: cleanup and properly comment this code!
                    if ((run.MapId == 0x131 || run.MapId == 0x132) && run.TzenThiefBought == "")
                    {
                        run.PartyYPosition = _sniClient.ReadMemory(WCData.PartyYPosition, 1)[0];
                        if (run.PartyYPosition < 7)
                        {
                            run.EsperCount = _sniClient.ReadMemory(WCData.EsperCount, 1)[0];
                            run.TzenThiefBit = DataHandler.CheckBitByOffset(_sniClient.ReadMemory(WCData.EventBitStartAddress + 0x27c / 8, 1)[0], 0x27c);
                            run.TzenThiefBought = DataHandler.CheckTzenThiefBought(run.EsperCount, run.EsperCountPrevious, run.TzenThiefBit);
                            if (!run.InTzenThiefArea)
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
                    run.EventBitData = _sniClient.ReadMemory(WCData.EventBitStartAddress, WCData.EventBitDataSize);
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

                    // Ask the console application if a key has been pressed.
                    // For now it's just escape for a force reset. More keys might be implemented in the future.
                    // TODO: maybe send an interface as argument and only expose functionality to event subscribers?
                    OnCheckKeypress?.Invoke(this, EventArgs.Empty);

                    // If a force reset has been triggered, reset the run.
                    if (_enableForceRunReset)
                    {
                        run.SeedHasBeenAbandoned = true;
                        break;
                    }

                    // If a new seed is found in the directory, abandon the seed.
                    // TODO: move this to _fileHandler and make it a function (get rid of copypasted auto-reset code)
                    if (isValidDirectory &&
                        DateTime.Now - _fileHandler.LastDirectoryRefresh > _fileHandler.RefreshInterval)
                    {
                        string previousSeed = _fileHandler.LastLoadedSeed;
                        isValidDirectory = _fileHandler.UpdateLastSeed(run.SeedInfo, out string[] updatedSeedInfo, false);

                        run.SeedInfo = updatedSeedInfo;
                        if (_fileHandler.LastLoadedSeed != previousSeed)
                        {
                            run.SeedHasBeenAbandoned = true;
                            break;
                        }
                    }

                    if (isDebugMode)
                    {
                        OnShowDebugInformation?.Invoke(this, new DebugInformationEventArgs(run));
                    }
#if JSON_DEBUG
                    run.EndTime = DateTime.Now;
                    run.FinalTime = run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation;
                    run.HasFinished = true;
#endif
                }
            }

            // If the seed has been abandoned, start tracking the new run.
            if (run.SeedHasBeenAbandoned)
            {
                _fileHandler.ResetLastLoadedSeed();
                continue;
            }

            // If KT skip was unlocked, format a string with the time.
            if (run.IsKTSkipUnlocked)
            {
                run.KtSkipUnlockTimeString = (run.KtSkipUnlockTime - run.StartTime).ToString(@"hh\:mm\:ss");
            }

            // Add Kefka kill time to event list.
            // TODO: move this to Run class.
            run.Route.Add(("Kefka kill", run.FinalTime.ToString(@"hh\:mm\:ss")));

            // Get data after Kefka kill.
            run.CharactersBytes = _sniClient.ReadMemory(WCData.CharactersByte, 2);
            run.DragonsBytes = _sniClient.ReadMemory(WCData.DragonsByte, 2);
            run.BossCount = _sniClient.ReadMemory(WCData.BossCount, 1)[0];
            run.EsperCount = _sniClient.ReadMemory(WCData.EsperCount, 1)[0];
            run.CheckCount = _sniClient.ReadMemory(WCData.CheckCount, 1)[0];
            run.DragonCount = _sniClient.ReadMemory(WCData.DragonCount, 1)[0];
            run.ChestData = _sniClient.ReadMemory(WCData.ChestDataStart, WCData.ChestDataSize);
            run.EventBitData = _sniClient.ReadMemory(WCData.EventBitStartAddress, WCData.EventBitDataSize);
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
            run.CharacterData = _sniClient.ReadMemory(WCData.CharacterDataStart, WCData.CharacterDataSize);
            run.CharacterSkillData = _sniClient.ReadMemory(WCData.CharacterSkillData, WCData.CharacterSkillDataSize);
            
            // Mock-up final lineup for the JSON
            run.FinalBattleLineup = new byte[] {0x0D, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A};
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
            RunJson runJson = new(run, _libVersion, _fileHandler.LastLoadedSeed);

            // Write JSON file.
            _fileHandler.WriteJSONFile(run.EndTime, runJson);

            // Show final time in console.
            OnRunSuccessful?.Invoke(this, new RunSuccessfulEventArgs(run.FinalTime.ToString(@"hh\:mm\:ss\.fff")));
            _fileHandler.ResetLastLoadedSeed();
        }
    }

    /// <summary>
    /// Forces a run reset.
    /// TODO: this shouldn't be available on non-UI classes.
    /// </summary>
    public static void ForceRunReset()
    {
        _enableForceRunReset = true;
    }
}

public class DebugModeEventArgs : EventArgs
{
    public bool IsDebugMode { get; }

    public DebugModeEventArgs(bool isDebugMode)
    {
        IsDebugMode = isDebugMode;
    }
}

public class RunSuccessfulEventArgs : EventArgs
{
    public string FinalTime { get; }

    public RunSuccessfulEventArgs(string finalTime)
    {
        FinalTime = finalTime;
    }
}

public class DebugInformationEventArgs : EventArgs
{
    public Run RunData { get; }

    public DebugInformationEventArgs(Run runData)
    {
        RunData = runData;
    }
}