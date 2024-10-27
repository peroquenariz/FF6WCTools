using System;
using System.Reflection;
using System.Threading;
using FF6WCToolsLib;
using static FF6WCToolsLib.WCData;

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

                if (_fileHandler.CanRefresh)
                {
                    run.SeedInfo = _fileHandler.UpdateSeedInfo(run.SeedInfo);
                }

                run.MapId = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(MAP_ID_START, 2)) & 0x1FF;
                run.MenuNumber = _sniClient.ReadMemory(MENU_NUMBER, 1)[0];
                run.NewGameSelected = _sniClient.ReadMemory(NEW_GAME_SELECTED, 1)[0];
                if (run.CheckIfRunStarted())
                {
                    break;
                }
            }
#endif
            OnTrackingRun?.Invoke(this, EventArgs.Empty);

            Thread.Sleep(3500);

            // Read character data.
            run.CharacterData = _sniClient.ReadMemory(CHARACTER_DATA_START, CHARACTER_DATA_SIZE);
            run.CharactersBytes = _sniClient.ReadMemory(CHARACTERS_AVAILABLE_START, 2);

            // Get starting characters and commands.
            run.CharacterCommands = DataHandler.GetCharacterCommands(run.CharacterData);
            run.StartingCharacters = DataHandler.GetAvailableCharacters(run.CharactersBytes);
            run.StartingCommands = DataHandler.GetAvailableCommands(run.CharactersBytes, run.CharacterCommands);

            // Get initial GP count.
            run.StartingGp = run.GPCurrent = run.GPPrevious = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(CURRENT_GP_START, 3));

            // Loop while run is in progress.
            while (!run.HasFinished)
            {
                if (_sniClient.RequestTimer % 2 != 0)
                {
                    // Check if the player is in a menu or shop, track time spent menuing and times they opened a menu.
                    run.EnableDialogWindow = _sniClient.ReadMemory(ENABLE_DIALOG_WINDOW, 1)[0];
                    run.GameStateData = _sniClient.ReadMemory(NMI_JUMP_CODE, 3);
                    run.GameState = DataHandler.GetGameState(run.GameStateData);
                    run.CheckIfMenuIsOpen();
                }

                // Check if the player is flying the airship, track time spent flying and times they drove it.
                if (AIRSHIP_MAP_IDS.Contains(run.MapId) && run.EnableDialogWindow != 1)
                {
                    run.Character1Graphic = _sniClient.ReadMemory(CHARACTER_1_GRAPHIC, 1)[0];
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
                    run.IsKefkaFight = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(BATTLE_INDEX_START, 2));
                    run.IsKefkaDead = _sniClient.ReadMemory(ENABLE_KEFKA_FINAL_ANIMATION, 1)[0];

                    // Check if the player is in the party selection menu before final Kefka fight.
                    if (run.IsMenuTimerRunning)
                    {
                        // Get final Kefka character lineup.
                        run.FinalBattleLineup = _sniClient.ReadMemory(FINAL_BATTLE_CHARACTER_LIST_START, 12);
                    }

                    // Get character data.
                    if (!run.IsFinalBattle)
                    {
                        run.CharacterData = _sniClient.ReadMemory(CHARACTER_DATA_START, CHARACTER_DATA_SIZE);
                        run.CharacterSkillData = _sniClient.ReadMemory(CHARACTER_SKILL_DATA_START, CHARACTER_SKILL_DATA_SIZE);
                        run.IsFinalBattle = true;
                    }

                    run.CheckKefkaKill();
                }

                // Tzen thief peek WoB.
                if (run.MapId == 0x132 && run.TzenThiefPeekWob == TzenThiefPeekWob.Did_not_check)
                {
                    // Read dialog index and only execute if the dialog is the WoB Tzen Thief dialogue
                    run.DialogIndex = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(DIALOG_INDEX_START, 2));
                    if (run.DialogIndex == DIALOG_INDEX_WOB_TZEN_THIEF)
                    {
                        // Wait for the dialogue options to be available
                        run.DialogWaitingForInput = _sniClient.ReadMemory(DIALOG_WAITING_FOR_INPUT, 1)[0];
                        
                        // Get dialog pointer, value will be different if esper or item
                        // due to the amount of lines in the dialog box
                        run.DialogPointer = _sniClient.ReadMemory(DIALOG_POINTER, 1)[0];

                        // Get which dialog choice is selected
                        run.DialogChoiceSelected = _sniClient.ReadMemory(DIALOG_CHOICE_SELECTED, 1)[0];

                        // Store the peek data.
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
                        run.MonsterBytes = _sniClient.ReadMemory(MONSTER_INDEX_START, 12);
                        run.LogBattle();
                    }
                }

                if (_sniClient.RequestTimer > 10)
                {
                    _sniClient.RequestTimer = 0;

                    run.MapId = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(MAP_ID_START, 2)) & 0x1FF;
                    run.Inventory = _sniClient.ReadMemory(INVENTORY_START, INVENTORY_SIZE);

                    // Read KT unlock and skip status only if the bits haven't been set.
                    if (!run.IsKTSkipUnlocked || !run.IsKefkaTowerUnlocked)
                    {
                        // KT unlock and skip event bits are in the same byte.
                        run.KefkaTowerEventByte = _sniClient.ReadMemory(EVENT_BIT_START_ADDRESS + EVENT_BIT_OFFSET_KT_SKIP_UNLOCK / 8, 1)[0];
                    }

                    if (run.IsMenuTimerRunning)
                    {
                        run.MenuNumber = _sniClient.ReadMemory(MENU_NUMBER, 1)[0];
                        run.ScreenDisplayRegister = _sniClient.ReadMemory(SCREEN_DISPLAY_REGISTER, 1)[0]; // Menu fades
                        run.NextMenuState = _sniClient.ReadMemory(NEXT_MENU_STATE, 1)[0]; // Next menu state
                    }

                    // Add visited maps to the list.
                    run.UpdateMapsVisited();

                    // Update GP spent.
                    if (run.MapId != 3)
                    {
                        run.GPCurrent = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(CURRENT_GP_START, 3));
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
                        run.EsperCountPrevious = _sniClient.ReadMemory(ESPER_COUNT, 1)[0];
                        run.InAuctionHouse = true;
                    }
                    else if (run.MapId != 0x0C8 && run.InAuctionHouse)
                    {
                        run.EsperCount = _sniClient.ReadMemory(ESPER_COUNT, 1)[0];
                        run.CountAuctionHouseEspersBought();
                        run.InAuctionHouse = false;
                    }

                    // Whelk peek.
                    if (run.MapId == 0x02B && !run.IsWhelkPeeked)
                    {
                        run.PartyYPosition = _sniClient.ReadMemory(PARTY_Y_POSITION, 1)[0];
                        if (run.PartyYPosition <= 32 && run.PartyYPosition >= 30) // Coordinates past whelk gate
                        {
                            run.IsWhelkPeeked = true;
                        }
                    }

                    // Esper Mountain peek.
                    if (run.MapId == 0x177 && !run.IsEsperMountainPeeked)
                    {
                        run.EsperMountainPeekByte = _sniClient.ReadMemory(EVENT_BIT_START_ADDRESS + 0x17B / 8, 1)[0]; // NPC running offscreen
                        run.IsEsperMountainPeeked = DataHandler.CheckBitByOffset(run.EsperMountainPeekByte, 0x17B);
                    }

                    // South Figaro basement peek.
                    if (run.MapId == 0x053 && !run.IsSouthFigaroBasementPeeked)
                    {
                        run.PartyXPosition = _sniClient.ReadMemory(PARTY_X_POSITION, 1)[0];
                        // TODO: this is currently flagging as peeked even if Celes isn't on the party.
                        // Open world seeds are fine, but character gating needs this fixed.
                        if (run.PartyXPosition >= 0x37 && run.PartyXPosition <= 0x3B)
                        {
                            run.IsSouthFigaroBasementPeeked = true;
                        }
                    }

                    // Tzen thief peek WoR.
                    if (run.MapId == 0x131 && run.TzenThiefPeekWor == TzenThiefPeekWor.Did_not_check)
                    {
                        // Get dialog index for the WoR Tzen Thief
                        run.DialogIndex = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(DIALOG_INDEX_START, 2));
                        
                        // Store the peek data.
                        run.TzenThiefPeekWor = DataHandler.PeekTzenThiefRewardWor(run.DialogIndex);
                    }

                    // Check if Tzen thief was bought, and if it was an esper or an item.
                    // Works by checking esper changes against Tzen Thief bit.
                    
                    // If map is Tzen WoB or WoR and Thief hasn't been bought yet
                    if ((run.MapId == 0x131 || run.MapId == 0x132) && run.TzenThiefBought == TzenThiefBought.None)
                    {
                        // Check party Y position in map, only check if in the Tzen Thief area
                        run.PartyYPosition = _sniClient.ReadMemory(PARTY_Y_POSITION, 1)[0];
                        if (run.PartyYPosition < 7)
                        {
                            // Store esper count
                            run.EsperCount = _sniClient.ReadMemory(ESPER_COUNT, 1)[0];
                            
                            // Check if the event bit was set.
                            run.TzenThiefBit = DataHandler.CheckBitByOffset(_sniClient.ReadMemory(EVENT_BIT_START_ADDRESS + 0x27c / 8, 1)[0], 0x27c);
                            
                            // If event bit was changed, set the reward based on esper count change.
                            run.TzenThiefBought = DataHandler.CheckTzenThiefBought(run.EsperCount, run.EsperCountPrevious, run.TzenThiefBit);
                            
                            if (!run.InTzenThiefArea)
                            {
                                run.EsperCountPrevious = run.EsperCount; // Keep track of esper count changes
                                run.InTzenThiefArea = true;
                            }
                        }
                        else
                        {
                            run.InTzenThiefArea = false;
                        }
                    }

                    // Generic event bit peeks.
                    run.EventBitData = _sniClient.ReadMemory(EVENT_BIT_START_ADDRESS, EVENT_BIT_DATA_SIZE);
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

                    if (_fileHandler.IsSeedDirectoryValid && _fileHandler.CanRefresh)
                    {
                        string previousSeed = _fileHandler.LastLoadedSeed;
                        _fileHandler.UpdateSeedInfo(run.SeedInfo, false);

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
                    run.FinalTime = run.EndTime - run.StartTime - TIME_FROM_KEFKA_FLASH_TO_ANIMATION;
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

            // Get data after Kefka kill.
            run.CharactersBytes = _sniClient.ReadMemory(CHARACTERS_AVAILABLE_START, 2);
            run.DragonsBytes = _sniClient.ReadMemory(DRAGONS_KILLED_DATA, 2);
            run.BossCount = _sniClient.ReadMemory(BOSS_COUNT, 1)[0];
            run.EsperCount = _sniClient.ReadMemory(ESPER_COUNT, 1)[0];
            run.CheckCount = _sniClient.ReadMemory(CHECK_COUNT, 1)[0];
            run.DragonCount = _sniClient.ReadMemory(DRAGON_COUNT, 1)[0];
            run.ChestData = _sniClient.ReadMemory(CHEST_DATA_START, CHEST_DATA_SIZE);
            run.EventBitData = _sniClient.ReadMemory(EVENT_BIT_START_ADDRESS, EVENT_BIT_DATA_SIZE);
            run.CharacterCount = DataHandler.GetCharacterCount(run.CharactersBytes);
            run.ChestCount = DataHandler.GetChestCount(run.ChestData);
            run.CharacterMaxLevel = DataHandler.GetMaximumCharacterLevel(run.CharacterData);
            run.DragonsKilled = DataHandler.GetDragonsKilled(run.DragonsBytes);
            run.StepsTaken = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(STEP_COUNTER, 3));
            run.SaveCount = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(SAVE_COUNTER, 2));

            // Get checks data.
            run.GetListOfCompletedChecks();
            run.GetListOfPeekedChecks();
            run.GetTzenThiefData();
            run.CreateAuctionHouseString();

            // Get Coliseum data.
            run.CheckColiseumVisit();

#if JSON_DEBUG
            run.CharacterData = _sniClient.ReadMemory(CHARACTER_DATA_START, CHARACTER_DATA_SIZE);
            run.CharacterSkillData = _sniClient.ReadMemory(CHARACTER_SKILL_DATA_START, CHARACTER_SKILL_DATA_SIZE);
            
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
            runJson.WriteJSONFile(run.EndTime, runJson, _fileHandler);

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