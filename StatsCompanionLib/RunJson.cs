using System;
using System.Collections.Generic;
using System.Text.Json;
using FF6WCToolsLib;

namespace StatsCompanionLib;

/// <summary>
/// Handles the final JSON export.
/// </summary>
public class RunJson
{
    public string? appVersion { get; set; }
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
    public int startingGp { get; set; }
    public int gpSpent { get; set; }
    public int gpEarned { get; set; }
    public int countTimesGameWasSaved { get; set; }
    public int stepsTaken { get; set; }
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

    private const string JSON_TIMESTAMP_FORMAT = "yyyy_MM_dd - HH_mm_ss";
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { WriteIndented = true };

    private const string TIME_FORMAT = @"hh\:mm\:ss";
    private const string TIME_FORMAT_PRECISE = @"hh\:mm\:ss\.fff";
    private const string DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss";

    public RunJson(Run run, string? appVersion, string seedFilename)
    {
        this.appVersion = appVersion;
        filename = seedFilename;
        if (filename.Length > 4) filename = filename[..^4];
        runTime = run.FinalTime.ToString(TIME_FORMAT);
        runTimePrecise = run.FinalTime.ToString(TIME_FORMAT_PRECISE);
        runDate = run.StartTime.ToString(DATE_FORMAT);
        flagset = "Other";
        otherFlagset = "";
        ktStartTime = (run.KefkaTowerStartTime - run.StartTime).ToString(TIME_FORMAT);
        kefkaTime = (run.KefkaStartTime - run.StartTime).ToString(TIME_FORMAT);
        userId = "";
        chars = ReplaceCharactersInList(run.StartingCharacters);
        abilities = ReplaceCharactersInList(run.StartingCommands);
        disableAbilityCheck = false;
        numOfChars = run.CharacterCount;
        numOfEspers = run.EsperCount;
        numOfChecks = run.CheckCount;
        numOfPeekedChecks = run.ChecksPeeked.Count;
        numOfBosses = run.BossCount;
        numOfChests = run.ChestCount;
        kefkaTowerUnlockTime = (run.KefkaTowerUnlockTime - run.StartTime).ToString(TIME_FORMAT);
        skip = run.IsKTSkipUnlocked;
        ktSkipUnlockTime = run.KtSkipUnlockTimeString;
        dragons = ReplaceCharactersInList(run.DragonsKilled);
        finalBattle = ReplaceCharactersInList(run.FinalBattlePrep);
        highestLevel = run.CharacterMaxLevel;
        superBalls = run.HasSuperBall ? "Yes" : "No";
        egg = run.HasExpEgg ? "Yes" : "No";
        auction = ReplaceCharacters(run.AuctionHouseEsperCountText);
        thiefPeek = run.TzenThief.ToString();
        thiefReward = run.TzenThiefReward.ToString();
        coliseum = run.ColiseumVisit.ToString();
        race = ReplaceCharacters("No Race");
        mood = ReplaceCharacters("Not recorded");
        raceId = "";
        countResets = run.ResetCount;
        timeSpentOnMenus = run.TimeSpentOnMenus.ToString(TIME_FORMAT);
        countTimesMenuWasOpened = run.MenuOpenCounter;
        timeSpentOnShops = run.TimeSpentOnShops.ToString(TIME_FORMAT);
        countTimesShopsWereVisited = run.ShopOpenCounter;
        totalMenuTime = (run.TimeSpentOnMenus + run.TimeSpentOnShops).ToString(TIME_FORMAT);
        timeSpentDrivingAirship = run.TimeSpentOnAirship.ToString(TIME_FORMAT);
        countTimesAirshipWasUsed = run.AirshipCounter;
        timeSpentonBattles = run.TimeSpentOnBattles.ToString(TIME_FORMAT);
        countBattlesFought = run.BattlesFought;
        countTimesGameWasSaved = run.SaveCount;
        stepsTaken = run.StepsTaken;
        startingGp = run.StartingGp;
        gpSpent = run.GPSpent;
        gpEarned = run.GPEarned;
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
    private void GetSeedInfo(string?[] seedInfo)
    {
        for (int i = 0; i < 9; i++)
        {
            if (seedInfo[i] == null)
            {
                continue;
            }
            
            string line = seedInfo[i]!;
            
            if (line.StartsWith("Seed")) // Randomizer seed
            {
                seedRaw = line.Substring(10);
            }
            else if (line.StartsWith("Flags")) // Flagset
            {
                string flags = line.Substring(10);
                flagsetRaw = flags;
                flagset = ReplaceCharacters(FlagHandler.GetFlagset(flags));
            }
            else if (line.StartsWith("Website")) // Website seed ID
            {
                seed = line.Substring(line.Length - 12);
            }
        }
    }

    /// <summary>
    /// Replaces characters from a given string.
    /// StatsCollide requires that all spaces are replaced by underscores,
    /// and " - " are replaced by double underscores.
    /// </summary>
    /// <param name="input">The string to replace.</param>
    /// <returns>a string with replaced characters.</returns>
    private string ReplaceCharacters(string input)
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
    /// <returns>A list of strings with replaced characters.</returns>
    private List<string> ReplaceCharactersInList (List<string> inputList)
    {
        List<string> replacedList = new List<string>();

        foreach (string item in inputList)
        {
            replacedList.Add(ReplaceCharacters(item));
        }

        return replacedList;
    }

    /// <summary>
    /// Writes the run JSON file.
    /// TODO: move to FileHandler?
    /// </summary>
    /// <param name="endTime">Timestamp of the end of the run.</param>
    /// <param name="runJson">The run JSON data to write.</param>
    public void WriteJSONFile(DateTime endTime, RunJson runJson, FileHandler fileHandler)
    {
        // Serialize the JSON.
        string jsonRunData = SerializeJson(runJson);

        // Create a timestamped filename.
        string jsonPath = $"{fileHandler.RunsDirectory}{fileHandler.DirectorySeparator}{endTime.ToString(JSON_TIMESTAMP_FORMAT)}.json";

        // Write to a .json file.
        FileHandler.WriteStringToFile(jsonPath, jsonRunData);
    }

    /// <summary>
    /// Takes the run data and serializes the JSON with pretty format.
    /// </summary>
    /// <param name="runArguments">The run data.</param>
    /// <returns>A formatted JSON string, ready to write to a file.</returns>
    public string SerializeJson(RunJson runArguments)
    {
        string serializedJson = JsonSerializer.Serialize(runArguments, _jsonOptions);
        return serializedJson;
    }
}