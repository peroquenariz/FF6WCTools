using System;
using System.Collections.Generic;

namespace StatsCompanion
{
    /// <summary>
    /// A class representing a Worlds Collide run.
    /// </summary>
    internal class Run
    {
        bool _isMenuTimerRunning;
        bool _isBattleTimerRunning;
        bool _isAirshipTimerRunning;
        bool _seedHasBeenAbandoned;
        bool _steppedOnKTSwitches;
        bool _HasFinished;
        bool _isWhelkPeeked;
        bool _isEsperMountainPeeked;
        bool _isSouthFigaroBasementPeeked;
        bool _isKefkaTowerUnlocked;
        bool _isKTSkipUnlocked;
        bool _isKefkaTowerStarted;
        bool _isFinalBattle;
        bool _wonColiseumMatch;
        bool _tzenThiefBit;
        bool _inTzenThiefArea;
        bool _inAuctionHouse;
        byte _auctionHouseEsperCount;
        byte _menuNumber;
        byte _newGameSelected;
        byte _bossCount;
        byte _esperCount;
        byte _esperCountPrevious;
        byte _checkCount;
        byte _characterMaxLevel;
        byte _dragonCount;
        byte _isKefkaDead;
        byte _isMenuActive;
        byte _isMenuActivePrevious;
        byte _nextMenuState;
        byte _battleCounter;
        byte _battleCounterPrevious;
        byte _enableDialogWindow;
        byte _character1Graphic;
        byte _character1GraphicPrevious;
        byte _dialogWaitingForInput;
        byte _dialogPointer;
        byte _dialogChoiceSelected;
        byte _kefkaTowerEventByte;
        byte _esperMountainPeekByte;
        byte _partyXPosition;
        byte _partyYPosition;
        byte _screenDisplayRegister;
        byte[] _inventory;
        byte[] _charactersBytes;
        byte[] _characterData;
        byte[] _characterSkillData;
        byte[] _characterCommands;
        byte[] _dragonsBytes;
        byte[] _chestData;
        byte[] _eventBitData;
        byte[] _finalBattleLineup;
        byte[] _monsterBytes;
        byte[] _monsterBytesPrevious;
        Character[] _finalBattleCharacters;
        int _chestCount;
        int _characterCount;
        int _isKefkaFight;
        int _menuOpenCounter;
        int _airshipCounter;
        int _battlesFought;
        int _mapId;
        int _dialogIndex;
        int _dialogIndexPrevious;
        int _gpCurrent;
        int _gpPrevious;
        int _gpSpent;
        int _resetCount;
        DateTime _StartTime;
        DateTime _EndTime;
        DateTime _menuOpen;
        DateTime _menuClose;
        DateTime _battleStart;
        DateTime _battleEnd;
        DateTime _airshipStart;
        DateTime _airshipStop;
        DateTime _kefkaTowerUnlockTime;
        DateTime _ktSkipUnlockTime;
        DateTime _kefkaTowerStartTime;
        DateTime _kefkaStartTime;
        DateTime _lastMapTimestamp;
        DateTime _lastAddedBattleFormation;
        TimeSpan _timeSpentOnMenus;
        TimeSpan _timeSpentOnBattles;
        TimeSpan _timeSpentOnAirship;
        string _hasExpEgg;
        string _hasSuperBall;
        string _tzenThiefPeekWob;
        string _tzenThiefPeekWor;
        string _tzenThiefBought;
        string _tzenThief;
        string _tzenThiefReward;
        string _coliseumVisit;
        string _auctionHouseEsperCountText;
        string _ktSkipUnlockTimeString;
        string _battleFormation;
        List<string> _startingCharacters;
        List<string> _startingCommands;
        List<string> _dragonsKilled;
        List<string> _checksCompleted;
        List<string> _checksPeeked;
        List<string> _routeJson;
        List<string> _knownSwdTechs;
        List<string> _knownBlitzes;
        List<string> _knownLores;
        List<string> _finalBattlePrep;
        List<int> _mapsVisited;
        List<(string, string)> _route;

        public Run()
        {
            _seedHasBeenAbandoned = false;
            _HasFinished = false;
            _steppedOnKTSwitches = false;
            _hasExpEgg = "No";
            _hasSuperBall = "No";
            _characterMaxLevel = 0;
            _menuOpenCounter = 0;
            _isMenuTimerRunning = false;
            _isBattleTimerRunning = false;
            _timeSpentOnMenus = new(0, 0, 0);
            _timeSpentOnBattles = new(0, 0, 0);
            _timeSpentOnAirship = new(0, 0, 0);
            _isAirshipTimerRunning = false;
            _airshipCounter = 0;
            _isWhelkPeeked = false;
            _isEsperMountainPeeked = false;
            _isSouthFigaroBasementPeeked = false;
            _isKefkaTowerUnlocked = false;
            _isKTSkipUnlocked = false;
            _isKefkaTowerStarted = false;
            _isFinalBattle = false;
            _tzenThiefBit = false;
            _inTzenThiefArea = false;
            _ktSkipUnlockTimeString = "";
            _battleFormation = "";
            _tzenThiefPeekWob = "Did not check";
            _tzenThiefPeekWor = "Did not check";
            _tzenThiefBought = "";
            _tzenThief = "Did not check";
            _tzenThiefReward = "Did not buy - Unknown";
            _coliseumVisit = "Did not visit";
            _auctionHouseEsperCount = 0;
            _auctionHouseEsperCountText = "Zero";
            _startingCharacters = new();
            _startingCommands = new();
            _dragonsKilled = new();
            _checksCompleted = new();
            _checksPeeked = new();
            _mapsVisited = new();
            _routeJson = new();
            _route = new();
            _knownBlitzes = new();
            _knownLores = new();
            _knownSwdTechs = new();
            _finalBattlePrep = new();
            _inventory = Array.Empty<byte>();
            _charactersBytes = Array.Empty<byte>();
            _characterData = Array.Empty<byte>();
            _characterSkillData = Array.Empty<byte>();
            _characterCommands = Array.Empty<byte>();
            _dragonsBytes = Array.Empty<byte>();
            _chestData = Array.Empty<byte>();
            _eventBitData = Array.Empty<byte>();
            _finalBattleLineup = Array.Empty<byte>();
            _monsterBytes = Array.Empty<byte>();
            _monsterBytesPrevious = Array.Empty<byte>();
            _finalBattleCharacters = new Character[4];
        }

        public bool IsMenuTimerRunning { get => _isMenuTimerRunning; set => _isMenuTimerRunning = value; }
        public bool IsAirshipTimerRunning { get => _isAirshipTimerRunning; set => _isAirshipTimerRunning = value; }
        public bool SeedHasBeenAbandoned { get => _seedHasBeenAbandoned; set => _seedHasBeenAbandoned = value; }
        public bool SteppedOnKTSwitches { get => _steppedOnKTSwitches; set => _steppedOnKTSwitches = value; }
        public bool HasFinished { get => _HasFinished; set => _HasFinished = value; }
        public string HasExpEgg { get => _hasExpEgg; set => _hasExpEgg = value; }
        public string HasSuperBall { get => _hasSuperBall; set => _hasSuperBall = value; }
        public bool IsWhelkPeeked { get => _isWhelkPeeked; set => _isWhelkPeeked = value; }
        public bool IsEsperMountainPeeked { get => _isEsperMountainPeeked; set => _isEsperMountainPeeked = value; }
        public bool IsSouthFigaroBasementPeeked { get => _isSouthFigaroBasementPeeked; set => _isSouthFigaroBasementPeeked = value; }
        public bool IsKefkaTowerUnlocked { get => _isKefkaTowerUnlocked; set => _isKefkaTowerUnlocked = value; }
        public bool IsKTSkipUnlocked { get => _isKTSkipUnlocked; set => _isKTSkipUnlocked = value; }
        public bool IsKefkaTowerStarted { get => _isKefkaTowerStarted; set => _isKefkaTowerStarted = value; }
        public bool IsFinalBattle { get => _isFinalBattle; set => _isFinalBattle = value; }
        public byte MenuNumber { get => _menuNumber; set => _menuNumber = value; }
        public byte NewGameSelected { get => _newGameSelected; set => _newGameSelected = value; }
        public byte BossCount { get => _bossCount; set => _bossCount = value; }
        public byte EsperCount { get => _esperCount; set => _esperCount = value; }
        public byte CheckCount { get => _checkCount; set => _checkCount = value; }
        public byte CharacterMaxLevel { get => _characterMaxLevel; set => _characterMaxLevel = value; }
        public byte DragonCount { get => _dragonCount; set => _dragonCount = value; }
        public byte IsKefkaDead { get => _isKefkaDead; set => _isKefkaDead = value; }
        public byte IsMenuActive { get => _isMenuActive; set => _isMenuActive = value; }
        public byte IsMenuActivePrevious { get => _isMenuActivePrevious; set => _isMenuActivePrevious = value; }
        public byte Character1Graphic { get => _character1Graphic; set => _character1Graphic = value; }
        public byte DialogWaitingForInput { get => _dialogWaitingForInput; set => _dialogWaitingForInput = value; }
        public byte DialogPointer { get => _dialogPointer; set => _dialogPointer = value; }
        public byte DialogChoiceSelected { get => _dialogChoiceSelected; set => _dialogChoiceSelected = value; }
        public byte AuctionHouseEsperCount { get => _auctionHouseEsperCount; set => _auctionHouseEsperCount = value; }
        public byte KefkaTowerEventByte { get => _kefkaTowerEventByte; set => _kefkaTowerEventByte = value; }
        public byte[] Inventory { get => _inventory; set => _inventory = value; }
        public byte[] CharactersBytes { get => _charactersBytes; set => _charactersBytes = value; }
        public byte[] CharacterData { get => _characterData; set => _characterData = value; }
        public byte[] CharacterCommands { get => _characterCommands; set => _characterCommands = value; }
        public byte[] DragonsBytes { get => _dragonsBytes; set => _dragonsBytes = value; }
        public byte[] ChestData { get => _chestData; set => _chestData = value; }
        public byte[] EventBitData { get => _eventBitData; set => _eventBitData = value; }
        public byte[] FinalBattleLineup { get => _finalBattleLineup; set => _finalBattleLineup = value; }
        public int ChestCount { get => _chestCount; set => _chestCount = value; }
        public int CharacterCount { get => _characterCount; set => _characterCount = value; }
        public int IsKefkaFight { get => _isKefkaFight; set => _isKefkaFight = value; }
        public int MenuOpenCounter { get => _menuOpenCounter; set => _menuOpenCounter = value; }
        public int AirshipCounter { get => _airshipCounter; set => _airshipCounter = value; }
        public int MapId { get => _mapId; set => _mapId = value; }
        public int DialogIndex { get => _dialogIndex; set => _dialogIndex = value; }
        public int GPCurrent { get => _gpCurrent; set => _gpCurrent = value; }
        public int GPPrevious { get => _gpPrevious; set => _gpPrevious = value; }
        public DateTime StartTime { get => _StartTime; set => _StartTime = value; }
        public DateTime EndTime { get => _EndTime; set => _EndTime = value; }
        public DateTime MenuOpen { get => _menuOpen; set => _menuOpen = value; }
        public DateTime MenuClose { get => _menuClose; set => _menuClose = value; }
        public DateTime AirshipStart { get => _airshipStart; set => _airshipStart = value; }
        public DateTime AirshipStop { get => _airshipStop; set => _airshipStop = value; }
        public DateTime KefkaTowerUnlockTime { get => _kefkaTowerUnlockTime; set => _kefkaTowerUnlockTime = value; }
        public DateTime KtSkipUnlockTime { get => _ktSkipUnlockTime; set => _ktSkipUnlockTime = value; }
        public DateTime KefkaTowerStartTime { get => _kefkaTowerStartTime; set => _kefkaTowerStartTime = value; }
        public DateTime KefkaStartTime { get => _kefkaStartTime; set => _kefkaStartTime = value; }
        public TimeSpan TimeSpentOnMenus { get => _timeSpentOnMenus; set => _timeSpentOnMenus = value; }
        public TimeSpan TimeSpentOnAirship { get => _timeSpentOnAirship; set => _timeSpentOnAirship = value; }
        public string TzenThiefPeekWob { get => _tzenThiefPeekWob; set => _tzenThiefPeekWob = value; }
        public string TzenThiefPeekWor { get => _tzenThiefPeekWor; set => _tzenThiefPeekWor = value; }
        public string TzenThiefBought { get => _tzenThiefBought; set => _tzenThiefBought = value; }
        public string TzenThief { get => _tzenThief; set => _tzenThief = value; }
        public string TzenThiefReward { get => _tzenThiefReward; set => _tzenThiefReward = value; }
        public string ColiseumVisit { get => _coliseumVisit; set => _coliseumVisit = value; }
        public List<string> StartingCharacters { get => _startingCharacters; set => _startingCharacters = value; }
        public List<string> StartingCommands { get => _startingCommands; set => _startingCommands = value; }
        public List<string> DragonsKilled { get => _dragonsKilled; set => _dragonsKilled = value; }
        public List<string> ChecksCompleted { get => _checksCompleted; set => _checksCompleted = value; }
        public List<string> ChecksPeeked { get => _checksPeeked; set => _checksPeeked = value; }
        public List<int> MapsVisited { get => _mapsVisited; set => _mapsVisited = value; }
        public byte EsperMountainPeekByte { get => _esperMountainPeekByte; set => _esperMountainPeekByte = value; }
        public byte PartyXPosition { get => _partyXPosition; set => _partyXPosition = value; }
        public bool WonColiseumMatch { get => _wonColiseumMatch; set => _wonColiseumMatch = value; }
        public string AuctionHouseEsperCountText { get => _auctionHouseEsperCountText; set => _auctionHouseEsperCountText = value; }
        public List<string> RouteJson { get => _routeJson; set => _routeJson = value; }
        public byte ScreenDisplayRegister { get => _screenDisplayRegister; set => _screenDisplayRegister = value; }
        public byte PartyYPosition { get => _partyYPosition; set => _partyYPosition = value; }
        public int ResetCount { get => _resetCount; set => _resetCount = value; }
        public string KtSkipUnlockTimeString { get => _ktSkipUnlockTimeString; set => _ktSkipUnlockTimeString = value; }
        public byte Character1GraphicPrevious { get => _character1GraphicPrevious; set => _character1GraphicPrevious = value; }
        public bool TzenThiefBit { get => _tzenThiefBit; set => _tzenThiefBit = value; }
        public byte EsperCountPrevious { get => _esperCountPrevious; set => _esperCountPrevious = value; }
        public bool InTzenThiefArea { get => _inTzenThiefArea; set => _inTzenThiefArea = value; }
        public byte EnableDialogWindow { get => _enableDialogWindow; set => _enableDialogWindow = value; }
        public Character[] FinalBattleCharacters { get => _finalBattleCharacters; set => _finalBattleCharacters = value; }
        public List<(string, string)> Route { get => _route; set => _route = value; }
        public DateTime LastMapTimestamp { get => _lastMapTimestamp; set => _lastMapTimestamp = value; }
        public byte[] CharacterSkillData { get => _characterSkillData; set => _characterSkillData = value; }
        public List<string> KnownSwdTechs { get => _knownSwdTechs; set => _knownSwdTechs = value; }
        public List<string> KnownBlitzes { get => _knownBlitzes; set => _knownBlitzes = value; }
        public List<string> KnownLores { get => _knownLores; set => _knownLores = value; }
        public List<string> FinalBattlePrep { get => _finalBattlePrep; set => _finalBattlePrep = value; }
        public bool IsBattleTimerRunning { get => _isBattleTimerRunning; set => _isBattleTimerRunning = value; }
        public TimeSpan TimeSpentOnBattles { get => _timeSpentOnBattles; set => _timeSpentOnBattles = value; }
        public byte BattleCounter { get => _battleCounter; set => _battleCounter = value; }
        public byte BattleCounterPrevious { get => _battleCounterPrevious; set => _battleCounterPrevious = value; }
        public DateTime BattleStart { get => _battleStart; set => _battleStart = value; }
        public DateTime BattleEnd { get => _battleEnd; set => _battleEnd = value; }
        public int BattlesFought { get => _battlesFought; set => _battlesFought = value; }
        public bool InAuctionHouse { get => _inAuctionHouse; set => _inAuctionHouse = value; }
        public byte[] MonsterBytes { get => _monsterBytes; set => _monsterBytes = value; }
        public byte[] MonsterBytesPrevious { get => _monsterBytesPrevious; set => _monsterBytesPrevious = value; }
        public string BattleFormation { get => _battleFormation; set => _battleFormation = value; }
        public int GPSpent { get => _gpSpent; set => _gpSpent = value; }
        public DateTime LastAddedBattleFormation { get => _lastAddedBattleFormation; set => _lastAddedBattleFormation = value; }
        public byte NextMenuState { get => _nextMenuState; set => _nextMenuState = value; }

        public bool CheckIfRunStarted()
        {
            if (MapId == 3 && NewGameSelected == 1 && MenuNumber == 9)
            {
                StartTime = DateTime.Now;
                return true;
            }
            return false;
        }
        
        public void CheckIfMenuOpen()
        {
            if (!IsMenuTimerRunning)
            {
                if (IsMenuActive - IsMenuActivePrevious > 0 &&
                    IsMenuActive - IsMenuActivePrevious < 10 &&
                    EnableDialogWindow == 0)
                {
                    MenuOpen = DateTime.Now;
                    IsMenuTimerRunning = true;
                    MenuOpenCounter++;
                } 
            }
            else
            {
                if (IsMenuActive == IsMenuActivePrevious &&
                     ScreenDisplayRegister == 0 &&
                     DateTime.Now - MenuOpen > WCData.TimeMenuFalsePositives)
                {
                    MenuClose = DateTime.Now;
                    IsMenuTimerRunning = false;
                    TimeSpentOnMenus += MenuClose - MenuOpen;
                }
            }
            IsMenuActivePrevious = IsMenuActive;
        }

        public void CheckIfInBattle()
        {
            if (!IsBattleTimerRunning)
            {
                if (BattleCounter > 30 && // values 0-30 are used for flashes, buckets and certain animations
                    BattleCounterPrevious != 0 && // sets to zero when in town
                    BattleCounter - BattleCounterPrevious > 0 && // only account for small increments
                    BattleCounter - BattleCounterPrevious < 45 &&
                    BattleCounter - BattleCounterPrevious != 4 && // Cursor false positive
                    MenuClose - DateTime.Now < WCData.TimeFromMenuToOverworld && // Menu exit false positive
                    BattleEnd - DateTime.Now < WCData.TimeFromBattleToOverworld && // Battle end false positive
                    !IsMenuTimerRunning) // && MapId != 0x150) // False positive on Kefka's cinematic before final battle

                {
                    BattleStart = DateTime.Now - WCData.TimeFromFadeToBattle;
                    BattlesFought++;
                    IsBattleTimerRunning = true;
                }
            }
            else
            {
                if ((BattleCounter == BattleCounterPrevious || HasFinished == true) &&
                    DateTime.Now - BattleStart > WCData.TimeBattleFalsePositives)
                {
                    BattleEnd = DateTime.Now;
                    MonsterBytesPrevious = Array.Empty<byte>();
                    BattleFormation = ""; // Clear battle formation after battle is done.
                    CleanupBattleFormationFalsePositives();
                    IsBattleTimerRunning = false;
                    TimeSpentOnBattles += BattleEnd - BattleStart;
                }
            }
            // Console.WriteLine(BattleCounter - BattleCounterPrevious); // For debugging the battle counter
            BattleCounterPrevious = BattleCounter;
        }

        /// <summary>
        /// Deletes the last battle formation from the route if less than 0.5 seconds elapsed since battle ended.
        /// </summary>
        private void CleanupBattleFormationFalsePositives()
        {
            TimeSpan timeDifference = BattleEnd - LastAddedBattleFormation;
            if (timeDifference < WCData.TimeBattleFormationFalsePositives &&
                timeDifference > WCData.TimeZero)
            {
                Route.RemoveAt(Route.Count - 1);
            }
        }

        /// <summary>
        /// Logs every battle in the event list.
        /// </summary>
        public void LogBattle()
        {
            if (!DataHandler.AreArraysEqual(MonsterBytes, MonsterBytesPrevious)) // Check for changes in monster indexes
            {
                int[] monsterIndexes = DataHandler.GetMonsterIndexes(MonsterBytes);
                BattleFormation = "Battle: ";
                for (int i = 0; i < 6; i++)
                {
                    if (monsterIndexes[i] <= 383) // Ignore if monster doesn't exist
                    {
                        BattleFormation += WCData.MonsterDict[monsterIndexes[i]] + ", ";
                    }
                }
                if (BattleFormation != "Battle: ")
                {
                    BattleFormation = BattleFormation.Remove(BattleFormation.Length - 2, 2);
                    LastAddedBattleFormation = DateTime.Now;
                    Route.Add((BattleFormation, (LastAddedBattleFormation - StartTime).ToString(@"hh\:mm\:ss")));
                }
            }
            MonsterBytesPrevious = MonsterBytes;
        }

        public void CheckIfFlyingAirship()
        {
            if (!IsAirshipTimerRunning)
            {
                if ((Character1Graphic == 6 || Character1Graphic == 1) && 
                    !CheckAirshipFalsePositives() && // ignore Cave in the Veldt, Serpent Trench
                    !IsBattleTimerRunning &&
                    DateTime.Now - BattleEnd > WCData.TimeBattleFalsePositives) // don't start timer after Search the Skies cutscene
                {
                    AirshipStart = DateTime.Now;
                    IsAirshipTimerRunning = true;
                    AirshipCounter++;
                } 
            }
            else
            {
                if ((Character1Graphic == 3 && Character1GraphicPrevious == 9) ||
                    (Character1Graphic == 0 && (MapId == 0x006 || MapId == 0x00B || MapId == 0x00A || MapId == 0x011)) ||
                    IsMenuTimerRunning || IsBattleTimerRunning)
                {
                    AirshipStop = DateTime.Now;
                    IsAirshipTimerRunning = false;
                    TimeSpentOnAirship += AirshipStop - AirshipStart;
                }
            }
            Character1GraphicPrevious = Character1Graphic;
        }

        public bool CheckAirshipFalsePositives()
        {
            bool airshipFalsePositive = false;
            if (MapsVisited.Count>2)
            {
                airshipFalsePositive = WCData.AirshipFalsePositives.Contains(MapsVisited[^2]);
            }
            return airshipFalsePositive;
        }

        public void LogKefkaStartTime()
        {
            KefkaStartTime = DateTime.Now - WCData.TimeFromSwitchesToKefkaLair;
            SteppedOnKTSwitches = true;
        }

        public void CheckKefkaKill()
        {
            if (IsKefkaFight == 0x0202 && IsKefkaDead == 0x01)
            {
                EndTime = DateTime.Now;
                HasFinished = true;
            }
        }

        public void UpdateMapsVisited()
        {
            if (MapId <= 0x19E)
            {
                if (MapsVisited.Count == 0)
                {
                    MapsVisited.Add(MapId);
                    LastMapTimestamp = DateTime.Now;
                    MapsVisitedWithTimestamps.Add((WCData.MapsDict[(uint)MapId], (LastMapTimestamp - StartTime).ToString(@"hh\:mm\:ss")));
                }
                else if (MapsVisited[MapsVisited.Count - 1] != MapId && !IsMenuTimerRunning)
                {
                    if (DateTime.Now - LastMapTimestamp > WCData.TimeBetweenMapChanges)
                    {
                        MapsVisited.Add(MapId);
                        LastMapTimestamp = DateTime.Now;
                        MapsVisitedWithTimestamps.Add((WCData.MapsDict[(uint)MapId], (LastMapTimestamp - StartTime).ToString(@"hh\:mm\:ss")));
                    }
                }
            }
        }

        public void CountAuctionHouseEspersBought()
        {
            if (EsperCount - EsperCountPrevious >= 0)
            {
                AuctionHouseEsperCount += (byte)(EsperCount - EsperCountPrevious);
            }
        }

        public void CheckForItemsInInventory()
        {
            if (HasExpEgg == "No")
            {
                bool expEgg = DataHandler.CheckIfItemExistsInInventory(Inventory, 228);
                if (expEgg == true)
                {
                    HasExpEgg = "Yes";
                }
                
            }
            if (HasSuperBall == "No")
            {
                bool superBall = DataHandler.CheckIfItemExistsInInventory(Inventory, 250);
                if (superBall == true)
                {
                    HasSuperBall = "Yes";
                }
            }
        }

        public void CheckKefkaTowerUnlock()
        {
            if (IsKefkaTowerUnlocked == false)
            {
                IsKefkaTowerUnlocked = DataHandler.CheckBitByOffset(KefkaTowerEventByte, 0x094);
                if (IsKefkaTowerUnlocked == true)
                {
                    KefkaTowerUnlockTime = DateTime.Now;
                }
            }
        }

        public void CheckKTSkipUnlock()
        {
            if (IsKTSkipUnlocked == false)
            {
                IsKTSkipUnlocked = DataHandler.CheckBitByOffset(KefkaTowerEventByte, 0x093);
                if (IsKTSkipUnlocked == true)
                {
                    KtSkipUnlockTime = DateTime.Now;
                }
            }
        }

        public void CheckKefkaTowerStart()
        {
            if ((MapId == 0x14E || MapId == 0x163) && IsKefkaTowerUnlocked == true && IsKefkaTowerStarted == false)
            {
                KefkaTowerStartTime = DateTime.Now;
                IsKefkaTowerStarted = true;
            }
        }

        public void GetListOfCompletedChecks()
        {
            foreach (var item in WCData.EventBitDict)
            {
                byte checkByte = EventBitData[item.Value / 8];
                bool checkDone = DataHandler.CheckBitByOffset(checkByte, item.Value);
                if (checkDone == true)
                {
                    ChecksCompleted.Add(WCData.CheckNamesDict[item.Key]);
                }
            }
        }

        public void GetListOfPeekedChecks()
        {
            // Peeks by map ID.
            foreach (var item in WCData.PeeksByMapId)
            {
                if (ChecksCompleted.Contains(item.Value) == false && MapsVisited.Contains(item.Key))
                {
                    ChecksPeeked.Add(item.Value);
                }
            }

            // Peeks by event bits.
            foreach (var item in WCData.PeeksByEventBit)
            {
                if (ChecksCompleted.Contains(item.Value) == false)
                {
                    byte eventByte = EventBitData[item.Key / 8];
                    bool eventBit = DataHandler.CheckBitByOffset(eventByte, item.Key);
                    if (eventBit == true)
                    {
                        ChecksPeeked.Add(item.Value);
                    }
                }
            }

            // Multiple condition peeks. Check manually for each.
            if (!ChecksCompleted.Contains("South Figaro Prisoner") && IsSouthFigaroBasementPeeked == true)
            {
                ChecksPeeked.Add("South Figaro Prisoner");
            }
            if (!ChecksCompleted.Contains("Esper Mountain") && IsEsperMountainPeeked == true)
            {
                ChecksPeeked.Add("Esper Mountain");
            }
            if (!ChecksCompleted.Contains("Whelk Gate") && IsWhelkPeeked == true)
            {
                ChecksPeeked.Add("Whelk Gate");
            }
            if (!ChecksCompleted.Contains("Umaro's Cave") && (MapsVisited.Contains(0x11B) || MapsVisited.Contains(0x015)))
            {
                ChecksPeeked.Add("Umaro's Cave");
            }
            if (!ChecksCompleted.Contains("Auction House 1") && !ChecksCompleted.Contains("Auction House 2") && MapsVisited.Contains(0x0C8))
            {
                ChecksPeeked.Add("Auction House");
            }
        }

        public void GetTzenThiefData()
        {
            if (TzenThiefBought == "")
            {
                if (TzenThiefPeekWob != "Did not check")
                {
                    TzenThiefReward = $"Did not buy - {TzenThiefPeekWob}";
                    ChecksPeeked.Add("Tzen Thief");
                }
                else if (TzenThiefPeekWor != "Did not check")
                {
                    TzenThiefReward = $"Did not buy - {TzenThiefPeekWor}";
                    ChecksPeeked.Add("Tzen Thief");
                }
                else
                {
                    TzenThiefReward = $"Did not buy - Unknown";
                }
            }
            else
            {
                TzenThiefReward = $"Bought {TzenThiefBought}";
            }

            if (TzenThiefPeekWob == "Did not check" && TzenThiefPeekWor == "Did not check")
            {
                TzenThief = "Did not check";
            }
            else if (TzenThiefPeekWob != "Did not check" && TzenThiefPeekWor == "Did not check")
            {
                TzenThief = "Checked WOB only";
            }
            else if (TzenThiefPeekWob == "Did not check" && TzenThiefPeekWor != "Did not check")
            {
                TzenThief = "Checked WOR only";
            }
            else
            {
                TzenThief = "Checked both";
            }
        }

        public void CheckColiseumVisit()
        {
            if (WonColiseumMatch == true && MapsVisited.Contains(0x19D))
            {
                ColiseumVisit = "Visited and fought";
            }
            else if (WonColiseumMatch == false && MapsVisited.Contains(0x19D))
            {
                ColiseumVisit = "Visited but didn't fight";
            }
        }

        public void WriteDebugInformation()
        {
            Console.Clear();
            Console.WriteLine("Stats Companion is now tracking your run...");
            Console.WriteLine("*** DO NOT close this window! ***");
            Console.WriteLine();
            Console.WriteLine($"In a menu: {IsMenuTimerRunning}");
            Console.WriteLine($"Time spent in a menu: {TimeSpentOnMenus}");
            Console.WriteLine($"Times you entered a menu: {MenuOpenCounter}");
            Console.WriteLine();
            Console.WriteLine($"Flying the airship: {IsAirshipTimerRunning}");
            Console.WriteLine($"Time spent flying the airship: {TimeSpentOnAirship}");
            Console.WriteLine($"Times you used the airship: {AirshipCounter}");
            Console.WriteLine();
            Console.WriteLine($"In battle: {IsBattleTimerRunning}");
            Console.WriteLine($"Time spent battling: {TimeSpentOnBattles}");
            Console.WriteLine($"Battles fought: {BattlesFought}");
            Console.WriteLine();
            Console.WriteLine($"Current map: {WCData.MapsDict[(uint)MapId]}");
            Console.WriteLine();
            Console.WriteLine("Last 5 maps visited:");
            for (int i = 0; i < 5; i++)
            {
                if (MapsVisitedWithTimestamps.Count -1 -i >= 0)
                {
                    Console.WriteLine(MapsVisitedWithTimestamps[MapsVisitedWithTimestamps.Count -1 -i]);
                }
            }
        }

        public void CreateAuctionHouseString()
        {
            switch (AuctionHouseEsperCount)
            {
                case 0:
                    AuctionHouseEsperCountText = "Zero";
                    break;
                case 1:
                    AuctionHouseEsperCountText = "One";
                    break;
                case 2:
                    AuctionHouseEsperCountText = "Two";
                    break;
            }
        }

        public void CreateTimestampedRoute()
        {
            foreach (var item in Route)
            {
                RouteJson.Add($"{item.Item2} {item.Item1}");
                if (item.Item1 == "Reset")
                {
                    ResetCount++;
                }
            }
        }

        public void GetFinalLineup()
        {
            for (int i = 0; i < 4; i++)
            {
                // Get character data.
                byte[] cData = CharacterData[(FinalBattleLineup[i] * 37)..(FinalBattleLineup[i] * 37 + 37)];

                // Get character skill data.
                byte[] cSkillData;
                
                if (FinalBattleLineup[i] < 0x0C)
                {
                    cSkillData = CharacterSkillData[(FinalBattleLineup[i] * 54)..(FinalBattleLineup[i] * 54 + 54)];
                }
                else
                {
                    // If character is Gogo or Umaro, create an empty array.
                    cSkillData = Array.Empty<byte>();
                }
                string name = WCData.CharacterNames[FinalBattleLineup[i]];
                FinalBattleCharacters[i] = new Character(cData, cSkillData, name);
            }
        }

        public void GetSwdTechList()
        {
            byte swdTechData = CharacterSkillData[WCData.SwdTechOffset];
            for (byte i = 0; i < 8; i++)
            {
                bool isSwdTechKnown = DataHandler.CheckBitSet(swdTechData, WCData.BitFlags[i]);
                if (isSwdTechKnown == true)
                {
                    KnownSwdTechs.Add(WCData.SwdTechDict[i]);
                }
            }
        }

        public void GetBlitzList()
        {
            byte blitzData = CharacterSkillData[WCData.BlitzOffset];
            for (byte i = 0; i < 8; i++)
            {
                bool isBlitzKnown = DataHandler.CheckBitSet(blitzData, WCData.BitFlags[i]);
                if (isBlitzKnown == true)
                {
                    KnownBlitzes.Add(WCData.BlitzDict[i]);
                }
            }
        }

        public void GetLoreList()
        {
            int loreData = DataHandler.ConcatenateByteArray(CharacterSkillData[WCData.LoreOffset..(WCData.LoreOffset+3)]);
            for (byte i = 0; i < 24; i++)
            {
                bool isLoreKnown = DataHandler.CheckBitSet(loreData, WCData.BitFlags[i%8] << 8*(i/8));
                if (isLoreKnown == true)
                {
                    KnownLores.Add(WCData.LoreDict[i]);
                }
            }
        }

        public void CheckForMute()
        {
            foreach (var character in FinalBattleCharacters)
            {
                if (character.Spells.Contains("Mute") ||
                    character.Esper == "Siren" ||
                   (character.Commands.Contains("Lore") && KnownLores.Contains("SourMouth")))
                {
                    FinalBattlePrep.Add("Mute");
                    return;
                }
            }
        }

        public void CheckForInstantDeath()
        {
            foreach (var character in FinalBattleCharacters)
            {
                if (character.Spells.Contains("X-Zone") || character.Spells.Contains("Doom") ||
                    character.Esper == "Raiden" || character.Esper == "Odin" ||
                   (character.Commands.Contains("SwdTech") && KnownSwdTechs.Contains("Cleave")) ||
                   (character.Commands.Contains("Tools") && DataHandler.CheckIfItemExistsInInventory(Inventory, 169) == true))
                {
                    FinalBattlePrep.Add("Instant death");
                    return;
                }
            }
        }

        public void CheckForCalmnessProtection()
        {
            foreach (var character in FinalBattleCharacters)
            {
                if (character.Esper == "Fenrir" || character.Esper == "Golem" || 
                    character.Esper == "Phantom" || character.Spells.Contains("Life3"))
                {
                    FinalBattlePrep.Add("Calmness protection");
                    return;
                }
            }
        }

        public void UpdateGPSpent()
        {
            if (GPCurrent < GPPrevious && !IsInSaveMenu())
            {
                GPSpent += (GPPrevious - GPCurrent);
            }
            GPPrevious = GPCurrent;
        }

        private bool IsInSaveMenu()
        {
            return MapId == 3 || (IsMenuTimerRunning && NextMenuState >= 19 && NextMenuState <= 22);
        }
    }
}