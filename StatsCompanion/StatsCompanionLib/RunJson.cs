using System.Collections.Generic;
using System.Text.RegularExpressions;
using FF6WCToolsLib;

namespace StatsCompanionLib
{
    /// <summary>
    /// Handles the final JSON export.
    /// </summary>
    public class RunJson
    {
        public string appVersion { get; set; }
        public string filename { get; set; }
        public string runTime { get; set; }
        public string runTimePrecise { get; set; }
        public string runDate { get; set; }
        public string kefkaTowerUnlockTime { get; set; }
        public bool skip { get; set; }
        public string ktSkipUnlockTime { get; set; }
        public string ktStartTime { get; set; }
        public string kefkaTime { get; set; }
        public int countResets { get; set; }
        public string timeSpentOnMenus { get; set; }
        public int countTimesMenuWasOpened { get; set; }
        public string timeSpentOnShops { get; set; }
        public int countTimesShopsWereVisited { get; set; }
        public string totalMenuTime { get; set; }
        public string timeSpentDrivingAirship { get; set; }
        public int countTimesAirshipWasUsed { get; set; }
        public string timeSpentonBattles { get; set; }
        public int countBattlesFought { get; set; }
        public int gpSpent { get; set; }
        public List<string> chars { get; set; }
        public List<string> abilities { get; set; }
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
        public string userId { get; set; }
        public string race { get; set; }
        public string mood { get; set; }
        public string? seed { get; set; }
        public string? seedRaw { get; set; }
        public string? flagsetRaw { get; set; }
        public string raceId { get; set; }
        public string flagset { get; set; }
        public string otherFlagset { get; set; }
        public List<string> checksCompleted { get; set; }
        public List<string> checksPeeked { get; set; }
        public Character[] finalBattleCharacters { get; set; }
        public List<string> knownSwdTechs { get; set; }
        public List<string> knownBlitzes { get; set; }
        public List<string> knownLores { get; set; }
        public List<string> route { get; set; }
        
        private const string TIME_FORMAT = "hh\\:mm\\:ss";
        private const string TIME_FORMAT_PRECISE = "hh\\:mm\\:ss\\.fff";
        private const string DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss";

        public RunJson(Run run, string appVersion, string seedFilename)
        {
            this.appVersion = appVersion;
            filename = seedFilename;
            if (this.filename.Length > 4) filename = filename[..^4];
            runTime = (run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@TIME_FORMAT);
            runTimePrecise = (run.EndTime - run.StartTime - WCData.TimeFromKefkaFlashToAnimation).ToString(@TIME_FORMAT_PRECISE);
            runDate = run.StartTime.ToString(DATE_FORMAT);
            flagset = "Other";
            otherFlagset = "";
            ktStartTime = (run.KefkaTowerStartTime - run.StartTime).ToString(@TIME_FORMAT);
            kefkaTime = (run.KefkaStartTime - run.StartTime).ToString(@TIME_FORMAT);
            userId = "";
            chars = run.StartingCharacters;
            abilities = ReplaceCharactersInList(run.StartingCommands);
            disableAbilityCheck = false;
            numOfChars = run.CharacterCount;
            numOfEspers = run.EsperCount;
            numOfChecks = run.CheckCount;
            numOfPeekedChecks = run.ChecksPeeked.Count;
            numOfBosses = run.BossCount;
            numOfChests = run.ChestCount;
            kefkaTowerUnlockTime = (run.KefkaTowerUnlockTime - run.StartTime).ToString(@TIME_FORMAT);
            skip = run.IsKTSkipUnlocked;
            ktSkipUnlockTime = run.KtSkipUnlockTimeString;
            dragons = ReplaceCharactersInList(run.DragonsKilled);
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
            raceId = "";
            countResets = run.ResetCount;
            timeSpentOnMenus = run.TimeSpentOnMenus.ToString(@TIME_FORMAT);
            countTimesMenuWasOpened = run.MenuOpenCounter;
            timeSpentOnShops = run.TimeSpentOnShops.ToString(@TIME_FORMAT);
            countTimesShopsWereVisited = run.ShopOpenCounter;
            totalMenuTime = (run.TimeSpentOnMenus + run.TimeSpentOnShops).ToString(@TIME_FORMAT);
            timeSpentDrivingAirship = run.TimeSpentOnAirship.ToString(@TIME_FORMAT);
            countTimesAirshipWasUsed = run.AirshipCounter;
            timeSpentonBattles = run.TimeSpentOnBattles.ToString(@TIME_FORMAT);
            countBattlesFought = run.BattlesFought;
            gpSpent = run.GPSpent;
            checksCompleted = run.ChecksCompleted;
            checksPeeked = run.ChecksPeeked;
            finalBattleCharacters = run.FinalBattleCharacters;
            route = run.RouteJson;
            knownSwdTechs = run.KnownSwdTechs;
            knownBlitzes = run.KnownBlitzes;
            knownLores = run.KnownLores;
            seed = null;
            seedRaw = null;
            flagsetRaw = null;
            GetSeedInfo(run.SeedInfo);
        }

        /// <summary>
        /// Extracts the seed ID and flagset from the given file.
        /// If the seed folder is not setup properly, seedInfo will be null and no info will be collected.
        /// </summary>
        /// <param name="seedInfo">String array containing the first 9 lines of the seed txt file.</param>
        private void GetSeedInfo(string[] seedInfo)
        {
            for (int i = 0; i < 9; i++)
            {
                string line = seedInfo[i];
                if (line == null)
                {
                    return;
                }
                else if (line.StartsWith("Seed"))
                {
                    seedRaw = seedInfo[i].Substring(10);
                }
                else if (line.StartsWith("Flags"))
                {
                    string flags = seedInfo[i].Substring(10);
                    flagsetRaw = flags;
                    flagset = ReplaceCharacters(GetFlagset(flags));
                }
                else if (line.StartsWith("Website"))
                {
                    seed = line.Substring(line.Length - 12);
                }
            }
        }

        /// <summary>
        /// Gets the flagset from the seed flags.
        /// </summary>
        /// <param name="flags">The flags of the seed.</param>
        public static string GetFlagset(string flags)
        {
            string flagset = "Other";
            string flagsReplaced = flags;
            List<string> patterns = new() {
                @"(-cpal [0123456789\.]* )",
                @"(-cpor [0123456789\.]* )",
                @"(-cspr [0123456789\.]* )",
                @"(-cspp [0123456789\.]* )"
            };
            
            // Regex replace palette, portrait and sprite swaps.
            foreach (var pattern in patterns)
            {
                flagsReplaced = Regex.Replace(flags, pattern, match =>
                    {
                        string value = match.Value;
                        string replacedValue = value.Replace(value, "");
                        return replacedValue;
                    }); 
            }
            
            // Find a matching flagset.
            foreach (var flagsetValue in WCData.FlagsetDict)
            {
                if (flagsReplaced == flagsetValue.Value)
                {
                    flagset = flagsetValue.Key;
                    return flagset;
                }
            }
            return flagset;
        }

        /// <summary>
        /// Replaces characters from a given string.
        /// StatsCollide requires that all spaces are replaced by underscores,
        /// and " - " are replaced by double underscores.
        /// </summary>
        /// <param name="input">The string to replace.</param>
        /// <returns>a string with replaced characters.</returns>
        private string ReplaceCharacters (string input)
        {
            string output;
            output = input.Replace(" - ", "__").Replace(" ", "_");
            return output;
        }

        /// <summary>
        /// Takes a list of strings and replaces the corresponding characters.
        /// Required for submitting JSONs to StatsCollide.
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private List<string> ReplaceCharactersInList (List<string> inputList)
        {
            List<string> replacedList = new List<string>();

            foreach (string item in inputList)
            {
                replacedList.Add(ReplaceCharacters(item));
            }

            return replacedList;
        }
    }
}