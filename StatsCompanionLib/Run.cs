using System;
using System.Collections.Generic;
using FF6WCToolsLib;
using static FF6WCToolsLib.WCData;

namespace StatsCompanionLib;

/// <summary>
/// A class representing a Worlds Collide run.
/// </summary>
public class Run
{
    // TODO: move all game memory-related stuff to a new class
    private const string TIME_FORMAT = @"hh\:mm\:ss";

    private bool _isMenuTimerRunning;
    private bool _isBattleTimerRunning;
    private bool _isAirshipTimerRunning;
    private bool _seedHasBeenAbandoned;
    private bool _steppedOnKTSwitches;
    private bool _hasFinished;
    private bool _isWhelkPeeked;
    private bool _isEsperMountainPeeked;
    private bool _isSouthFigaroBasementPeeked;
    private bool _isKefkaTowerUnlocked;
    private bool _isKTSkipUnlocked;
    private bool _hasKefkaTowerStarted;
    private bool _isFinalBattle;
    private bool _wonColiseumMatch;
    private bool _tzenThiefBit;
    private bool _inTzenThiefArea;
    private bool _inAuctionHouse;
    private bool _isReset;
    private byte _auctionHouseEsperCount;
    private byte _menuNumber;
    private byte _newGameSelected;
    private byte _bossCount;
    private byte _esperCount;
    private byte _esperCountPrevious;
    private byte _checkCount;
    private byte _characterMaxLevel;
    private byte _dragonCount;
    private byte _isKefkaDead;
    private byte _nextMenuState;
    private byte _enableDialogWindow;
    private byte _character1Graphic;
    private byte _character1GraphicPrevious;
    private byte _dialogWaitingForInput;
    private byte _dialogPointer;
    private byte _dialogChoiceSelected;
    private byte _kefkaTowerEventByte;
    private byte _esperMountainPeekByte;
    private byte _partyXPosition;
    private byte _partyYPosition;
    private byte _screenDisplayRegister;
    private byte[] _inventory;
    private byte[] _charactersBytes;
    private byte[] _characterData;
    private byte[] _characterSkillData;
    private byte[] _characterCommands;
    private byte[] _dragonsBytes;
    private byte[] _chestData;
    private byte[] _eventBitData;
    private byte[] _finalBattleLineup;
    private byte[] _monsterBytes;
    private byte[] _monsterBytesPrevious;
    private byte[] _gameStatusData;
    private Character[] _finalBattleCharacters;
    private string[] _seedInfo;
    private int _chestCount;
    private int _characterCount;
    private int _isKefkaFight;
    private int _menuOpenCounter;
    private int _shopOpenCounter;
    private int _airshipCounter;
    private int _battlesFought;
    private int _mapId;
    private int _dialogIndex;
    private int _gpCurrent;
    private int _gpPrevious;
    private int _gpSpent;
    private int _resetCount;
    private DateTime _startTime;
    private DateTime _endTime;
    private DateTime _menuOpen;
    private DateTime _menuClose;
    private DateTime _battleStart;
    private DateTime _battleEnd;
    private DateTime _airshipStart;
    private DateTime _airshipStop;
    private DateTime _kefkaTowerUnlockTime;
    private DateTime _ktSkipUnlockTime;
    private DateTime _kefkaTowerStartTime;
    private DateTime _kefkaStartTime;
    private DateTime _lastMapTimestamp;
    private DateTime _lastAddedBattleFormation;
    private TimeSpan _timeSpentOnMenus;
    private TimeSpan _timeSpentOnShops;
    private TimeSpan _timeSpentOnBattles;
    private TimeSpan _timeSpentOnAirship;
    private TimeSpan _finalTime;
    private string _hasExpEgg;
    private string _hasSuperBall;
    private string _auctionHouseEsperCountText;
    private string _ktSkipUnlockTimeString;
    private string _battleFormation;
    private string _gameStatus;
    private List<string> _startingCharacters;
    private List<string> _startingCommands;
    private List<string> _dragonsKilled;
    private List<string> _checksCompleted;
    private List<string> _checksPeeked;
    private List<string> _eventBitsPeeked;
    private List<string> _routeJson;
    private List<string> _knownSwdTechs;
    private List<string> _knownBlitzes;
    private List<string> _knownLores;
    private List<string> _finalBattlePrep;
    private List<int> _mapsVisited;
    private List<(string EventName, string Time)> _route;

    private TzenThiefPeekWob _tzenThiefPeekWob;
    private TzenThiefPeekWor _tzenThiefPeekWor;
    private TzenThiefBought _tzenThiefBought;
    private ThiefPeek _tzenThief;
    private ThiefReward _tzenThiefReward;
    private ColiseumVisit _coliseumVisit;

    public bool IsMenuTimerRunning { get => _isMenuTimerRunning; set => _isMenuTimerRunning = value; }
    public bool IsAirshipTimerRunning { get => _isAirshipTimerRunning; set => _isAirshipTimerRunning = value; }
    public bool SeedHasBeenAbandoned { get => _seedHasBeenAbandoned; set => _seedHasBeenAbandoned = value; }
    public bool SteppedOnKTSwitches { get => _steppedOnKTSwitches; set => _steppedOnKTSwitches = value; }
    public bool HasFinished { get => _hasFinished; set => _hasFinished = value; }
    public string HasExpEgg { get => _hasExpEgg; set => _hasExpEgg = value; }
    public string HasSuperBall { get => _hasSuperBall; set => _hasSuperBall = value; }
    public bool IsWhelkPeeked { get => _isWhelkPeeked; set => _isWhelkPeeked = value; }
    public bool IsEsperMountainPeeked { get => _isEsperMountainPeeked; set => _isEsperMountainPeeked = value; }
    public bool IsSouthFigaroBasementPeeked { get => _isSouthFigaroBasementPeeked; set => _isSouthFigaroBasementPeeked = value; }
    public bool IsKefkaTowerUnlocked { get => _isKefkaTowerUnlocked; set => _isKefkaTowerUnlocked = value; }
    public bool IsKTSkipUnlocked { get => _isKTSkipUnlocked; set => _isKTSkipUnlocked = value; }
    public bool IsFinalBattle { get => _isFinalBattle; set => _isFinalBattle = value; }
    public byte MenuNumber { get => _menuNumber; set => _menuNumber = value; }
    public byte NewGameSelected { get => _newGameSelected; set => _newGameSelected = value; }
    public byte BossCount { get => _bossCount; set => _bossCount = value; }
    public byte EsperCount { get => _esperCount; set => _esperCount = value; }
    public byte CheckCount { get => _checkCount; set => _checkCount = value; }
    public byte CharacterMaxLevel { get => _characterMaxLevel; set => _characterMaxLevel = value; }
    public byte DragonCount { get => _dragonCount; set => _dragonCount = value; }
    public byte IsKefkaDead { get => _isKefkaDead; set => _isKefkaDead = value; }
    public byte Character1Graphic { get => _character1Graphic; set => _character1Graphic = value; }
    public byte DialogWaitingForInput { get => _dialogWaitingForInput; set => _dialogWaitingForInput = value; }
    public byte DialogPointer { get => _dialogPointer; set => _dialogPointer = value; }
    public byte DialogChoiceSelected { get => _dialogChoiceSelected; set => _dialogChoiceSelected = value; }
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
    public DateTime StartTime { get => _startTime; set => _startTime = value; }
    public DateTime EndTime { get => _endTime; set => _endTime = value; }
    public DateTime KefkaTowerUnlockTime { get => _kefkaTowerUnlockTime; set => _kefkaTowerUnlockTime = value; }
    public DateTime KtSkipUnlockTime { get => _ktSkipUnlockTime; set => _ktSkipUnlockTime = value; }
    public DateTime KefkaTowerStartTime { get => _kefkaTowerStartTime; set => _kefkaTowerStartTime = value; }
    public DateTime KefkaStartTime { get => _kefkaStartTime; set => _kefkaStartTime = value; }
    public TimeSpan TimeSpentOnAirship { get => _timeSpentOnAirship; set => _timeSpentOnAirship = value; }
    public TzenThiefPeekWob TzenThiefPeekWob { get => _tzenThiefPeekWob; set => _tzenThiefPeekWob = value; }
    public TzenThiefPeekWor TzenThiefPeekWor { get => _tzenThiefPeekWor; set => _tzenThiefPeekWor = value; }
    public TzenThiefBought TzenThiefBought { get => _tzenThiefBought; set => _tzenThiefBought = value; }
    public ThiefPeek TzenThief { get => _tzenThief; set => _tzenThief = value; }
    public ThiefReward TzenThiefReward { get => _tzenThiefReward; set => _tzenThiefReward = value; }
    public ColiseumVisit ColiseumVisit { get => _coliseumVisit; set => _coliseumVisit = value; }
    public List<string> StartingCharacters { get => _startingCharacters; set => _startingCharacters = value; }
    public List<string> StartingCommands { get => _startingCommands; set => _startingCommands = value; }
    public List<string> DragonsKilled { get => _dragonsKilled; set => _dragonsKilled = value; }
    public List<string> ChecksCompleted { get => _checksCompleted; set => _checksCompleted = value; }
    public List<string> ChecksPeeked { get => _checksPeeked; set => _checksPeeked = value; }
    public byte EsperMountainPeekByte { get => _esperMountainPeekByte; set => _esperMountainPeekByte = value; }
    public byte PartyXPosition { get => _partyXPosition; set => _partyXPosition = value; }
    public bool WonColiseumMatch { get => _wonColiseumMatch; set => _wonColiseumMatch = value; }
    public string AuctionHouseEsperCountText { get => _auctionHouseEsperCountText; set => _auctionHouseEsperCountText = value; }
    public List<string> RouteJson { get => _routeJson; set => _routeJson = value; }
    public byte ScreenDisplayRegister { get => _screenDisplayRegister; set => _screenDisplayRegister = value; }
    public byte PartyYPosition { get => _partyYPosition; set => _partyYPosition = value; }
    public int ResetCount { get => _resetCount; set => _resetCount = value; }
    public string KtSkipUnlockTimeString { get => _ktSkipUnlockTimeString; set => _ktSkipUnlockTimeString = value; }
    public bool TzenThiefBit { get => _tzenThiefBit; set => _tzenThiefBit = value; }
    public byte EsperCountPrevious { get => _esperCountPrevious; set => _esperCountPrevious = value; }
    public bool InTzenThiefArea { get => _inTzenThiefArea; set => _inTzenThiefArea = value; }
    public byte EnableDialogWindow { get => _enableDialogWindow; set => _enableDialogWindow = value; }
    public Character[] FinalBattleCharacters { get => _finalBattleCharacters; set => _finalBattleCharacters = value; }
    public List<(string, string)> Route { get => _route; set => _route = value; }
    public byte[] CharacterSkillData { get => _characterSkillData; set => _characterSkillData = value; }
    public List<string> KnownSwdTechs { get => _knownSwdTechs; set => _knownSwdTechs = value; }
    public List<string> KnownBlitzes { get => _knownBlitzes; set => _knownBlitzes = value; }
    public List<string> KnownLores { get => _knownLores; set => _knownLores = value; }
    public List<string> FinalBattlePrep { get => _finalBattlePrep; set => _finalBattlePrep = value; }
    public bool IsBattleTimerRunning { get => _isBattleTimerRunning; set => _isBattleTimerRunning = value; }
    public TimeSpan TimeSpentOnBattles { get => _timeSpentOnBattles; set => _timeSpentOnBattles = value; }
    public int BattlesFought { get => _battlesFought; set => _battlesFought = value; }
    public bool InAuctionHouse { get => _inAuctionHouse; set => _inAuctionHouse = value; }
    public byte[] MonsterBytes { get => _monsterBytes; set => _monsterBytes = value; }
    public int GPSpent { get => _gpSpent; set => _gpSpent = value; }
    public byte NextMenuState { get => _nextMenuState; set => _nextMenuState = value; }
    public TimeSpan TimeSpentOnMenus { get => _timeSpentOnMenus; set => _timeSpentOnMenus = value; }
    public TimeSpan TimeSpentOnShops { get => _timeSpentOnShops; set => _timeSpentOnShops = value; }
    public int ShopOpenCounter { get => _shopOpenCounter; set => _shopOpenCounter = value; }
    public byte[] GameStatusData { get => _gameStatusData; set => _gameStatusData = value; }
    public string GameStatus { get => _gameStatus; set => _gameStatus = value; }
    public bool IsReset { get => _isReset; set => _isReset = value; }
    public string[] SeedInfo { get => _seedInfo; set => _seedInfo = value; }
    public TimeSpan FinalTime { get => _finalTime; set => _finalTime = value; }

    public Run()
    {
        // Initialize all default run parameters.

        // DateTimes
        _menuOpen = DateTime.Now;
        _menuClose = DateTime.Now;
        _airshipStart = DateTime.Now;
        _airshipStop = DateTime.Now;
        _battleStart = DateTime.Now;
        _battleEnd = DateTime.Now;

        // Strings
        _hasExpEgg = "No"; // TODO: switch to booleans
        _hasSuperBall = "No";
        _ktSkipUnlockTimeString = "";
        _battleFormation = "";
        _auctionHouseEsperCountText = "Zero";
        _gameStatus = WCData.FIELD_KEY;

        // Lists
        _startingCharacters = new List<string>();
        _startingCommands = new List<string>();
        _dragonsKilled = new List<string>();
        _checksCompleted = new List<string>();
        _checksPeeked = new List<string>();
        _eventBitsPeeked = new List<string>();
        _mapsVisited = new List<int>();
        _routeJson = new List<string>();
        _route = new List<(string, string)>();
        _knownBlitzes = new List<string>();
        _knownLores = new List<string>();
        _knownSwdTechs = new List<string>();
        _finalBattlePrep = new List<string>();

        // Arrays
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
        _gameStatusData = Array.Empty<byte>();
        _finalBattleCharacters = new Character[4];
        _seedInfo = new string[9];

        // Enums
        _tzenThiefBought = TzenThiefBought.None;
        _tzenThiefPeekWob = TzenThiefPeekWob.Did_not_check;
        _tzenThiefPeekWor = TzenThiefPeekWor.Did_not_check;
        _tzenThief = ThiefPeek.Did_not_check;
        _tzenThiefReward = ThiefReward.Did_not_buy__Unknown;
        _coliseumVisit = ColiseumVisit.Did_not_visit;
    }

    public bool CheckIfRunStarted()
    {
        if (_mapId == 3 && _newGameSelected == 1 && _menuNumber == 9)
        {
            _startTime = DateTime.Now;
            return true;
        }
        return false;
    }
    
    public void CheckIfMenuIsOpen()
    {
        if (!_isMenuTimerRunning)
        {
            if (_gameStatus == WCData.MENU_KEY)
            {
                _menuOpen = DateTime.Now;
                _isMenuTimerRunning = true;
            }
        }
        else
        {
            if (_gameStatus != WCData.MENU_KEY)
            {
                _menuClose = DateTime.Now;
                _isMenuTimerRunning = false;
                if (_menuNumber == 3)
                {
                    _timeSpentOnShops += _menuClose - _menuOpen;
                    _shopOpenCounter++;
                }
                else
                {
                    _timeSpentOnMenus += _menuClose - _menuOpen;
                    _menuOpenCounter++;
                }
            }
        }
    }

    public void CheckIfInBattle()
    {
        if (!_isBattleTimerRunning)
        {
            if (_gameStatus == WCData.BATTLE_KEY)
            {
                _battleStart = DateTime.Now;
                _battlesFought++;
                _isBattleTimerRunning = true;
            }
        }
        else
        {
            if (_gameStatus != WCData.BATTLE_KEY || _hasFinished)
            {
                _battleEnd = DateTime.Now;
                _monsterBytesPrevious = Array.Empty<byte>();
                _battleFormation = ""; // Clear battle formation after battle is done.
                CleanupBattleFormationFalsePositives();
                _isBattleTimerRunning = false;
                _timeSpentOnBattles += _battleEnd - _battleStart;
            }
        }
    }

    /// <summary>
    /// Deletes the last battle formation from the route if less than 0.5 seconds elapsed since battle ended.
    /// </summary>
    private void CleanupBattleFormationFalsePositives()
    {
        TimeSpan timeDifference = _battleEnd - _lastAddedBattleFormation;
        if (timeDifference < WCData.TIME_BATTLE_FORMATION_FALSE_POSITIVES &&
            timeDifference > TimeSpan.Zero)
        {
            _route.RemoveAt(_route.Count - 1);
        }
    }

    /// <summary>
    /// Logs every battle in the event list.
    /// </summary>
    public void LogBattle()
    {
        if (!DataHandler.AreArraysEqual(_monsterBytes, _monsterBytesPrevious)) // Check for changes in monster indexes
        {
            int[] monsterIndexes = DataHandler.GetMonsterIndexes(_monsterBytes);
            _battleFormation = "Battle: ";
            for (int i = 0; i < 6; i++)
            {
                if (monsterIndexes[i] <= 383) // Ignore if monster doesn't exist
                {
                    _battleFormation += WCData.MONSTER_DICT[monsterIndexes[i]] + ", ";
                }
            }
            if (_battleFormation != "Battle: ")
            {
                _battleFormation = _battleFormation.Remove(_battleFormation.Length - 2, 2);
                _lastAddedBattleFormation = DateTime.Now;
                _route.Add((_battleFormation, (_lastAddedBattleFormation - _startTime).ToString(TIME_FORMAT)));
            }
        }
        _monsterBytesPrevious = _monsterBytes;
    }

    public void CheckIfFlyingAirship()
    {
        if (!_isAirshipTimerRunning)
        {
            if ((_character1Graphic == 6 || // value when taking off from overworld
                _character1Graphic == 1 && !CheckAirshipFalsePositives()) && // ignore Cave in the Veldt, Serpent Trench, South Figaro cave, Ebot's Rock
                !_isBattleTimerRunning &&
                DateTime.Now - _battleEnd > WCData.TIME_BATTLE_FALSE_POSITIVES) // don't start timer after Search the Skies cutscene
            {
                _airshipStart = DateTime.Now;
                _isAirshipTimerRunning = true;
                _airshipCounter++;
            }
        }
        else
        {
            if ((_character1Graphic == 3 && _character1GraphicPrevious == 9) ||
                (_character1Graphic == 0 && WCData.AIRSHIP_DECK_MAP_IDS.Contains(_mapId)) ||
                _isMenuTimerRunning || _isBattleTimerRunning)
            {
                _airshipStop = DateTime.Now;
                _isAirshipTimerRunning = false;
                _timeSpentOnAirship += _airshipStop - _airshipStart;
            }
        }
        _character1GraphicPrevious = _character1Graphic;
    }

    public bool CheckAirshipFalsePositives()
    {
        bool airshipFalsePositive = false;
        if (_mapsVisited.Count>2)
        {
            airshipFalsePositive = WCData.AIRSHIP_FALSE_POSITIVES.Contains(_mapsVisited[^2]);
        }
        return airshipFalsePositive;
    }

    public void LogKefkaStartTime()
    {
        _kefkaStartTime = DateTime.Now - WCData.TIME_FROM_SWITCHES_TO_KEFKA_LAIR;
        _steppedOnKTSwitches = true;
    }

    public void CheckKefkaKill()
    {
        if (_isKefkaFight == 0x0202 && _isKefkaDead == 0x01)
        {
            _endTime = DateTime.Now;
            _finalTime = _endTime - _startTime - WCData.TIME_FROM_KEFKA_FLASH_TO_ANIMATION;
            _route.Add(("Kefka kill", _finalTime.ToString(TIME_FORMAT)));
            _hasFinished = true;
        }
    }

    public void UpdateMapsVisited()
    {
        if (_mapId <= 0x19E)
        {
            if (_mapsVisited.Count == 0 || (_mapsVisited[_mapsVisited.Count - 1] != _mapId && !_isMenuTimerRunning))
            {
                _mapsVisited.Add(_mapId);
                _lastMapTimestamp = DateTime.Now;
                _route.Add((WCData.MAPS_DICT[(uint)_mapId], (_lastMapTimestamp - _startTime).ToString(TIME_FORMAT)));
            }
        }
    }

    public void CountAuctionHouseEspersBought()
    {
        if (_esperCount - _esperCountPrevious >= 0)
        {
            _auctionHouseEsperCount += (byte)(_esperCount - _esperCountPrevious);
        }
    }

    public void CheckForItemsInInventory()
    {
        if (_hasExpEgg == "No")
        {
            bool hasExpEgg = DataHandler.CheckIfItemExistsInInventory(_inventory, 228);
            if (hasExpEgg)
            {
                _hasExpEgg = "Yes";
            }
            
        }
        if (_hasSuperBall == "No")
        {
            bool hasSuperBall = DataHandler.CheckIfItemExistsInInventory(_inventory, 250);
            if (hasSuperBall)
            {
                _hasSuperBall = "Yes";
            }
        }
    }

    public void CheckKefkaTowerUnlock()
    {
        if (!_isKefkaTowerUnlocked)
        {
            _isKefkaTowerUnlocked = DataHandler.CheckBitByOffset(KefkaTowerEventByte, 0x094);
            if (_isKefkaTowerUnlocked)
            {
                _kefkaTowerUnlockTime = DateTime.Now;
            }
        }
    }

    public void CheckKTSkipUnlock()
    {
        if (!_isKTSkipUnlocked)
        {
            _isKTSkipUnlocked = DataHandler.CheckBitByOffset(KefkaTowerEventByte, 0x093);
            if (_isKTSkipUnlocked)
            {
                _ktSkipUnlockTime = DateTime.Now;
            }
        }
    }

    public void CheckKefkaTowerStart()
    {
        if ((_mapId == 0x14E || _mapId == 0x163) && _isKefkaTowerUnlocked  && !_hasKefkaTowerStarted)
        {
            _kefkaTowerStartTime = DateTime.Now;
            _hasKefkaTowerStarted = true;
        }
    }

    public void GetListOfCompletedChecks()
    {
        foreach (var eventBitOffset in WCData.EVENT_BIT_OFFSETS_DICT)
        {
            byte checkByte = _eventBitData[eventBitOffset.Value / 8];
            bool isCheckCompleted = DataHandler.CheckBitByOffset(checkByte, eventBitOffset.Value);
            if (isCheckCompleted)
            {
                _checksCompleted.Add(WCData.EVENT_BIT_CHECK_NAMES_DICT[eventBitOffset.Key]);
            }
        }
    }

    public void GetListOfPeekedChecks()
    {
        // Peeks by map ID.
        foreach (var mapIdPeek in WCData.PEEKS_BY_MAP_ID)
        {
            if (!_checksCompleted.Contains(mapIdPeek.Value) && _mapsVisited.Contains(mapIdPeek.Key))
            {
                _checksPeeked.Add(mapIdPeek.Value);
            }
        }

        // Peeks by event bits.
        foreach (var eventBitPeek in _eventBitsPeeked)
        {
            if (!_checksCompleted.Contains(eventBitPeek))
            {
                _checksPeeked.Add(eventBitPeek);
            }
        }

        // Multiple condition peeks. Check manually for each.
        if (!_checksCompleted.Contains("South Figaro Prisoner") &&
            _isSouthFigaroBasementPeeked)
        {
            _checksPeeked.Add("South Figaro Prisoner");
        }
        if (!_checksCompleted.Contains("Esper Mountain") &&
            _isEsperMountainPeeked)
        {
            _checksPeeked.Add("Esper Mountain");
        }
        if (!_checksCompleted.Contains("Whelk Gate") &&
            _isWhelkPeeked)
        {
            _checksPeeked.Add("Whelk Gate");
        }
        if (!_checksCompleted.Contains("Auction House 1") &&
            !_checksCompleted.Contains("Auction House 2") &&
            _mapsVisited.Contains(0x0C8))
        {
            _checksPeeked.Add("Auction House");
        }
    }

    public void CheckEventBitPeeks()
    {
        foreach (var eventBit in WCData.PEEKS_BY_EVENT_BIT)
        {
            if (!_eventBitsPeeked.Contains(eventBit.Value))
            {
                byte eventByte = _eventBitData[eventBit.Key / 8];
                bool isEventBitSet = DataHandler.CheckBitByOffset(eventByte, eventBit.Key);
                if (isEventBitSet)
                {
                    _eventBitsPeeked.Add(eventBit.Value);
                }
            }
        }
    }

    public void GetTzenThiefData()
    {
        // Tzen Thief reward - peeked/bought logic.
        // If Tzen Thief wasn't bought, check if it was peeked.
        if (_tzenThiefBought == TzenThiefBought.None)
        {
            // If WoB Thief was peeked, store the appropiate esper/item peek and add it to the peek list.
            if (_tzenThiefPeekWob != TzenThiefPeekWob.Did_not_check)
            {
                switch (_tzenThiefPeekWob)
                {
                    case TzenThiefPeekWob.Esper:
                        _tzenThiefReward = ThiefReward.Did_not_buy__Esper;
                        _checksPeeked.Add("Tzen Thief");
                        break;
                    case TzenThiefPeekWob.Item:
                        _tzenThiefReward = ThiefReward.Did_not_buy__Item;
                        _checksPeeked.Add("Tzen Thief");
                        break;
                    default:
                        break;
                }
            }
            // If WoR Thief was peeked, store it as Unknown BUT add it to the peek list.
            else if (_tzenThiefPeekWor != TzenThiefPeekWor.Did_not_check)
            {
                _tzenThiefReward = ThiefReward.Did_not_buy__Unknown;
                _checksPeeked.Add("Tzen Thief");
            }
            // If none of the thieves were peeked, don't store the peek.
            else
            {
                _tzenThiefReward = ThiefReward.Did_not_buy__Unknown;
            }
        }
        // If the reward was bought, store the reward.
        else
        {
            switch (_tzenThiefBought)
            {
                case TzenThiefBought.Esper:
                    _tzenThiefReward = ThiefReward.Bought_Esper;
                    break;
                case TzenThiefBought.Item:
                    _tzenThiefReward = ThiefReward.Bought_Item;
                    break;
                default:
                    break;
            }
        }

        // If none of the thieves were visited
        if (_tzenThiefPeekWob == TzenThiefPeekWob.Did_not_check &&
            _tzenThiefPeekWor == TzenThiefPeekWor.Did_not_check)
        {
            _tzenThief = ThiefPeek.Did_not_check;
        }
        // If WoB Thief was visited
        else if (_tzenThiefPeekWob != TzenThiefPeekWob.Did_not_check &&
                 _tzenThiefPeekWor == TzenThiefPeekWor.Did_not_check)
        {
            _tzenThief = ThiefPeek.Checked_WOB_only;
        }
        // If WoR Thief was visited
        else if (_tzenThiefPeekWob == TzenThiefPeekWob.Did_not_check &&
                 _tzenThiefPeekWor != TzenThiefPeekWor.Did_not_check)
        {
            _tzenThief = ThiefPeek.Checked_WOR_only;
        }
        // If both thieves were visited
        else
        {
            _tzenThief = ThiefPeek.Checked_both;
        }
    }

    public void CheckColiseumVisit()
    {
        if (_wonColiseumMatch  && _mapsVisited.Contains(0x19D))
        {
            _coliseumVisit = ColiseumVisit.Visited_and_fought;
        }
        else if (!_wonColiseumMatch && _mapsVisited.Contains(0x19D))
        {
            _coliseumVisit = ColiseumVisit.Visited_but_did_not_fight;
        }
    }

    public void CreateAuctionHouseString()
    {
        switch (_auctionHouseEsperCount)
        {
            case 0:
                _auctionHouseEsperCountText = "Zero";
                break;
            case 1:
                _auctionHouseEsperCountText = "One";
                break;
            case 2:
                _auctionHouseEsperCountText = "Two";
                break;
        }
    }

    public void CreateTimestampedRoute()
    {
        foreach (var routeEvent in _route)
        {
            _routeJson.Add($"{routeEvent.Time} {routeEvent.EventName}");
            if (routeEvent.EventName == "Reset")
            {
                _resetCount++;
            }
        }
    }

    public void GetFinalLineup()
    {
        for (int i = 0; i < 4; i++)
        {
            // Get character data.
            byte[] characterData = _characterData[(_finalBattleLineup[i] * 37)..(_finalBattleLineup[i] * 37 + 37)];

            // Get character skill data.
            byte[] characterSkillData;
            
            if (_finalBattleLineup[i] < 0x0C)
            {
                characterSkillData = _characterSkillData[(_finalBattleLineup[i] * 54)..(_finalBattleLineup[i] * 54 + 54)];
            }
            else
            {
                // If character is Gogo or Umaro, create an empty array.
                characterSkillData = Array.Empty<byte>();
            }
            
            // Get character name.
            string name = WCData.CHARACTER_NAMES[_finalBattleLineup[i]];

            // Add character to the final battle character array.
            _finalBattleCharacters[i] = new Character(characterData, characterSkillData, name);
        }
    }

    public void GetSwdTechList()
    {
        byte swdTechData = _characterSkillData[WCData.SWDTECH_OFFSET];
        for (byte i = 0; i < 8; i++)
        {
            bool isSwdTechKnown = DataHandler.CheckBitSet(swdTechData, WCData.BIT_FLAGS[i]);
            if (isSwdTechKnown)
            {
                _knownSwdTechs.Add(WCData.SWDTECH_DICT[i]);
            }
        }
    }

    public void GetBlitzList()
    {
        byte blitzData = _characterSkillData[WCData.BLITZ_OFFSET];
        for (byte i = 0; i < 8; i++)
        {
            bool isBlitzKnown = DataHandler.CheckBitSet(blitzData, WCData.BIT_FLAGS[i]);
            if (isBlitzKnown)
            {
                _knownBlitzes.Add(WCData.BLITZ_DICT[i]);
            }
        }
    }

    public void GetLoreList()
    {
        int loreData = DataHandler.ConcatenateByteArray(_characterSkillData[WCData.LORE_OFFSET..(WCData.LORE_OFFSET+3)]);
        for (byte i = 0; i < 24; i++)
        {
            bool isLoreKnown = DataHandler.CheckBitSet(loreData, WCData.BIT_FLAGS[i%8] << 8*(i/8));
            if (isLoreKnown)
            {
                _knownLores.Add(WCData.LORE_DICT[i]);
            }
        }
    }

    public void CheckForMute()
    {
        foreach (var character in _finalBattleCharacters)
        {
            if (character.Spells.Contains("Mute") ||
                character.Esper == "Siren" ||
               (character.Commands.Contains("Lore") && _knownLores.Contains("SourMouth")))
            {
                _finalBattlePrep.Add("Mute");
                return;
            }
        }
    }

    public void CheckForInstantDeath()
    {
        foreach (var character in _finalBattleCharacters)
        {
            if (character.Spells.Contains("X-Zone") || character.Spells.Contains("Doom") ||
                character.Esper == "Raiden" || character.Esper == "Odin" ||
               (character.Commands.Contains("SwdTech") && _knownSwdTechs.Contains("Cleave")) ||
               (character.Commands.Contains("Tools") && DataHandler.CheckIfItemExistsInInventory(_inventory, 169)))
            {
                _finalBattlePrep.Add("Instant Death");
                return;
            }
        }
    }

    public void CheckForCalmnessProtection()
    {
        foreach (var character in _finalBattleCharacters)
        {
            if (character.Esper == "Fenrir" || character.Esper == "Golem" || 
                character.Esper == "Phantom" || character.Spells.Contains("Life3"))
            {
                _finalBattlePrep.Add("Calmness Protection");
                return;
            }
        }
    }

    public void UpdateGPSpent()
    {
        if (_gpCurrent < _gpPrevious && !IsInSaveMenu())
        {
            _gpSpent += (_gpPrevious - _gpCurrent);
        }
        _gpPrevious = _gpCurrent;
    }

    private bool IsInSaveMenu()
    {
        return _mapId == 3 || (_isMenuTimerRunning && _nextMenuState >= 19 && _nextMenuState <= 22);
    }

    /// <summary>
    /// Takes the 3 bytes array and gets the game status.
    /// </summary>
    public void GetGameStatus()
    {
        int firstTwoBytes = DataHandler.ConcatenateByteArray(_gameStatusData[0..2]);
        byte lastByte = _gameStatusData[2];

        if (firstTwoBytes == 0x0ba7 && lastByte == 0xC1)
        {
            _gameStatus = WCData.BATTLE_KEY;
        }
        else if (firstTwoBytes == 0x0182 && lastByte == 0xC0)
        {
            _gameStatus = WCData.FIELD_KEY;
        }
        else if (firstTwoBytes == 0xa728 && lastByte == 0xEE)
        {
            _gameStatus = WCData.WORLD_KEY;
        }
        else if (firstTwoBytes == 0x1387 && lastByte == 0xC3)
        {
            _gameStatus = WCData.MENU_KEY;
        }
        else if ((firstTwoBytes == 0xa509 && lastByte == 0xEE) ||
                 (firstTwoBytes == 0xa94d && lastByte == 0xEE))
        {
            _gameStatus = WCData.MODE7_KEY;
        }
    }

    public void CheckResetFalsePositive()
    {
        if (_route.Count > 1 && _menuNumber != 2)
        {
            for (int i = 0; i < 2; i++)
            {
                _route.RemoveAt(_route.Count - 1);
            }
        }
        _isReset = false;
    }
}