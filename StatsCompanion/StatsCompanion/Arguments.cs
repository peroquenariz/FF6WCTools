using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StatsCompanion
{
    internal class Arguments
    {
        public string runTime { get; set; }
        public string runDate { get; set; }
        public string flagset { get; set; }
        public string otherFlagset { get; set; }
        public string kefkaTowerUnlockTime { get; set; }
        public bool skip { get; set; }
        public string ktSkipUnlockTime { get; set; }
        public string ktStartTime { get; set; }
        public string kefkaTime { get; set; }
        public string userId { get; set; }
        public int countResets { get; set; }
        public string timeSpentOnMenu { get; set; }
        public int countTimesMenuWasOpened { get; set; }
        public string timeSpentDrivingAirship { get; set; }
        public int countTimesAirshipWasUsed { get; set; }
        public string timeSpentonBattles { get; set; }
        public int countBattlesFought { get; set; }
        public List<string> startingChars { get; set; }
        public List<string> startingAbilities { get; set; }
        public bool disableAbilityCheck { get; set; }
        public int numOfChars { get; set; }
        public int numOfEspers { get; set; }
        public int numOfChecks { get; set; }
        public int numOfPeekedChecks { get; set; }
        public int numOfBosses { get; set; }
        public int numOfChests { get; set; }
        public List<string> dragons { get; set; }
        public List<string> finalBattle { get; set; }
        public int highestLevel { get; set; }
        public string superBalls { get; set; }
        public string egg { get; set; }
        public string auction { get; set; }
        public string thiefPeek { get; set; }
        public string thiefReward { get; set; }
        public string coliseum { get; set; }
        public string race { get; set; }
        public string mood { get; set; }
        public string seed { get; set; }
        public string raceId { get; set; }
        public List<string> checksCompleted { get; set; }
        public List<string> checksPeeked { get; set; }
        public Character[] finalBattleCharacters { get; set; }
        public List<string> knownSwdTechs { get; set; }
        public List<string> knownBlitzes { get; set; }
        public List<string> knownLores { get; set; }
        public List<string> mapsVisited { get; set; }

        public Arguments(Run run)
        {
            runTime = (run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@"hh\:mm\:ss\.ff");
            runDate = run.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");
            flagset = "";
            otherFlagset = "";
            ktStartTime = (run.KefkaTowerStartTime - run.StartTime).ToString(@"hh\:mm\:ss\.ff");
            kefkaTime = (run.KefkaStartTime - run.StartTime).ToString(@"hh\:mm\:ss\.ff");
            userId = "";
            startingChars = run.StartingCharacters;
            startingAbilities = run.StartingCommands;
            disableAbilityCheck = false;
            numOfChars = run.CharacterCount;
            numOfEspers = run.EsperCount;
            numOfChecks = run.CheckCount;
            numOfPeekedChecks = run.ChecksPeeked.Count;
            numOfBosses = run.BossCount;
            numOfChests = run.ChestCount;
            kefkaTowerUnlockTime = (run.KefkaTowerUnlockTime - run.StartTime).ToString(@"hh\:mm\:ss\.ff");
            skip = run.IsKTSkipUnlocked;
            ktSkipUnlockTime = run.KtSkipUnlockTimeString;
            dragons = run.DragonsKilled;
            finalBattle = run.FinalBattlePrep;
            highestLevel = run.CharacterMaxLevel;
            superBalls = run.HasSuperBall;
            egg = run.HasExpEgg;
            auction = run.AuctionHouseEsperCountText;
            thiefPeek = run.TzenThief;
            thiefReward = run.TzenThiefReward;
            coliseum = run.ColiseumVisit;
            race = "No_Race";
            mood = "Not_recorded";
            seed = "";
            raceId = "";
            countResets = run.ResetCount;
            timeSpentOnMenu = run.TimeSpentOnMenus.ToString(@"hh\:mm\:ss\.ff");
            countTimesMenuWasOpened = run.MenuOpenCounter;
            timeSpentDrivingAirship = run.TimeSpentOnAirship.ToString(@"hh\:mm\:ss\.ff");
            countTimesAirshipWasUsed = run.AirshipCounter;
            timeSpentonBattles = run.TimeSpentOnBattles.ToString(@"hh\:mm\:ss\.ff");
            countBattlesFought = run.BattlesFought;
            checksCompleted = run.ChecksCompleted;
            checksPeeked = run.ChecksPeeked;
            finalBattleCharacters = run.FinalBattleCharacters;
            mapsVisited = run.MapsVisitedJson;
            knownSwdTechs = run.KnownSwdTechs;
            knownBlitzes = run.KnownBlitzes;
            knownLores = run.KnownLores;
        }
    }
}