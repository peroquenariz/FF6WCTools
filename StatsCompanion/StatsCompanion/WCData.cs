using System;
using System.Collections.Generic;

namespace StatsCompanion
{
    /// <summary>
    /// A class holding information about Final Fantasy 6: Worlds Collide.
    /// </summary>
    internal static class WCData
    {
        /// <summary>
        /// Address that contains the start of the character skill data.
        /// </summary>
        public const uint CharacterSkillData = 0x7E1A6E;

        public const int SwdTechOffset = 649;
        public const int BlitzOffset = 698;
        public const int LoreOffset = 699; // 3 bytes


        /// <summary>
        /// Address that contains the size of the character skill data.
        /// </summary>
        public const int CharacterSkillDataSize = 734;

        /// <summary>
        /// Time interval to avoid false positives on map changes.
        /// </summary>
        public static readonly TimeSpan TimeBetweenMapChanges = new(0, 0, 0, 0, 500);
        
        /// <summary>
        /// Address that changes when a dialog box is opened.
        /// </summary>
        public const uint EnableDialogWindow = 0x7E00BA;
        
        /// <summary>
        /// Address that contains the party X position on the field.
        /// </summary>
        public const uint PartyXPosition = 0x7E00AF;

        /// <summary>
        /// Address that contains the party X position on the field.
        /// </summary>
        public const uint PartyYPosition = 0x7E00B0;

        /// <summary>
        /// Address that contains the beginning of the character list selected right before the final battle.
        /// Character values range from 0x00 (Terra) to 0x0D (Umaro).
        /// Size: 12 bytes.
        /// </summary>
        public const uint FinalBattleCharacterListStart = 0x7E0205;
        
        /// <summary>
        /// Address that contains the state of the character overworld sprite.
        /// 1: flying the airship. 6-9: airship transitions.
        /// </summary>
        public const uint Character1Graphic = 0x7E00CA;
        
        /// <summary>
        /// Address that contains the current amount of GP. 3 bytes.
        /// </summary>
        public const uint CurrentGP = 0x7E1860;
        
        /// <summary>
        /// Address that contains the start of the known espers. Size: 4 bytes.
        /// </summary>
        public const uint KnownEspers = 0x7E1A69;
        
        /// <summary>
        /// Address that contains the start of the known lores. Size: 3 bytes.
        /// </summary>
        public const uint KnownLores = 0x7E1D29;
        
        /// <summary>
        /// Address that contains the known SwdTechs.
        /// </summary>
        public const uint KnownSwdTechs = 0x7E1CF7;
        
        /// <summary>
        /// Address that contains the start of the known spells.
        /// 12 characters, 54 spells each, 1 byte per spell.
        /// </summary>
        public const uint KnownSpells = 0x7E1A6E;

        /// <summary>
        /// Address that contains which choice is selected in a dialogue.
        /// </summary>
        public const uint DialogChoiceSelected = 0x7E056E;

        /// <summary>
        /// Address that contains the pointer to current tile in VRAM.
        /// 2: WoB Tzen Thief is an item, 4: is an esper.
        /// </summary>
        public const uint DialogPointer = 0x7E00C4;

        /// <summary>
        /// Address that contains the dialog index. Check alongside MapId to avoid false positives.
        /// Size: 2 bytes.
        /// </summary>
        public const uint DialogIndex = 0x7E00D0;

        /// <summary>
        /// Address that changes depending if the dialog box is waiting for an input.
        /// 0: not waiting, 1: waiting for key press, 2: waiting for key release.
        /// 0x04 = Item, 0x06 = Esper.
        /// </summary>
        public const uint DialogWaitingForInput = 0x7E00D3;

        /// <summary>
        /// Address that contains the map identifier. Size: 2 bytes.
        /// Top 7 bits of the second byte have complementary information.
        /// </summary>
        public const uint MapId = 0x7E1F64;

        /// <summary>
        /// Address that contains the start of the event bits.
        /// </summary>
        public const uint EventBitStartAddress = 0x7E1E80;

        /// <summary>
        /// Size of the event bits data.
        /// </summary>
        public const byte EventBitDataSize = 223;

        /// <summary>
        /// Address that contains the start of the field object data. 50 items, 41 bytes each.
        /// $00-$0F are characters, $10-$2F are NPC's, $30 is camera ($07B0), $31 is showing character or for unused objects ($07D9)
        /// </summary>
        public const uint FieldObjectStartAddress = 0x7E0867;

        /// <summary>
        /// Menu number address (value is 9 when the custom FF6WC new game screen is up).
        /// </summary>
        public const uint MenuNumber = 0x7E0200;

        /// <summary>
        /// Address that changes value from 0 to 1 on the new game screen, but also changes if you select a save game.
        /// Combined with MenuNumber, should be easy to determine when a new game has been started.
        /// </summary>
        public const uint NewGameSelected = 0x7E0224;

        // Addresses for checking if Kefka has been killed.
        public const uint BattleIndex = 0x7E11E0; // 2 bytes. Set to 0x0202 at tier 4. Shared between field, world and battle modules.
        public const uint EnableKefkaFinalAnimation = 0x7E009A; // 1 byte. Set to 01 when enabling Kefka's death animation.

        /// <summary>
        /// Address that contains a counter that counts up when in a battle.
        /// Also gets used to control screen effects like flashes, Kefka lair animation, etc.
        /// </summary>
        public const uint BattleCounter = 0x7E000E;
        
        /// <summary>
        /// Address that contains a counter that only changes if the player is currently in a menu or shop.
        /// Size: 2 bytes.
        /// </summary>
        public const uint MenuCounter = 0x7E00CF;

        /// <summary>
        /// Address that appears to control the brightness of the screen during menus. Goes from 1 to 15 in a menu.
        /// Is 0 when outside a menu.
        /// </summary>
        public const uint ScreenDisplayRegister = 0x7E0044;

        // Counters.
        public const uint BossCount = 0x7E1FF8;
        public const uint CheckCount = 0x7E1FCA;
        public const uint DragonCount = 0x7E1FCE;
        public const uint EsperCount = 0x7E1FC8;

        /// <summary>
        /// Address that contains the start of the chest data.
        /// Size: 48 bytes.
        /// </summary>
        public const uint ChestDataStart = 0x7E1E40;

        /// <summary>
        /// Size of the chest data.
        /// </summary>
        public const byte ChestDataSize = 48;

        /// <summary>
        /// Address that contains the start of the character data.
        /// From this address you can offset to get the rest of the data.
        /// Each character takes up 37 bytes. More info in the SRAM section: 
        /// https://www.ff6hacking.com/wiki/doku.php?id=ff3:ff3us:doc:asm:ram:field_ram
        /// </summary>
        public const uint CharacterDataStart = 0x7E1600;
        
        /// <summary>
        /// Size of the memory area that contains character data. 14 characters, 37 bytes each: 518 bytes total.
        /// </summary>
        public const int CharacterDataSize = 519;

        /// <summary>
        /// Address that contains the start of the inventory. Ends at 0x7E1967 (254 bytes).
        /// </summary>
        public const uint InventoryStart = 0x7E1869;

        /// <summary>
        /// Size of the inventory in memory.
        /// </summary>
        public const byte InventorySize = 254;

        /// <summary>
        /// Memory address that contains which character has been found.
        /// Info is split through 2 bytes. Top 2 bits of second byte aren't used.
        /// </summary>
        public const uint CharactersByte = 0x7E1EDE;

        /// <summary>
        /// Array that contains the flags for each bit.
        /// </summary>
        static public readonly byte[] BitFlags = new byte[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        /// <summary>
        /// Memory address that contains which dragon has been killed.
        /// Info is split through 2 bytes.
        /// </summary>
        public const uint DragonsByte = 0x7E1EA3;

        // Dragon flags.
        static public readonly byte[] DragonFlags1 = new byte[] { 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
        static public readonly byte[] DragonFlags2 = new byte[] { 0x01, 0x02 };
        
        /// <summary>
        /// Array that contains an ordered list of the characters for ease of indexing.
        /// </summary>
        static public readonly string[] CharacterNames = new string[] {
            "Terra",
            "Locke",
            "Cyan",
            "Shadow",
            "Edgar",
            "Sabin",
            "Celes",
            "Strago",
            "Relm",
            "Setzer",
            "Mog",
            "Gau",
            "Gogo",
            "Umaro"
        };

        static public readonly TimeSpan TimeFromKefkaFlashToAnimation = new(0, 0, 0, 4, 967); // 298 frames at 60FPS.
        static public readonly TimeSpan TimeFromSwitchesToKefkaLair = new(0, 0, 0, 13, 50); // 783 frames at 60FPS.

        /// <summary>
        /// List containing the maps that are used to determine if the airship is being flown.
        /// </summary>
        public static readonly List<int> AirshipMapIds = new() { 0x000, 0x001, 0x006, 0x00B, 0x00A, 0x011 };

        public static readonly List<int> AirshipFalsePositives = new() { 0x161, 0x0BB };

        /// <summary>
        /// Dictionary holding the bit offsets of every check event.
        /// </summary>
        public static readonly Dictionary<string, int> EventBitDict = new()
        {
            {"GOT_RAIDEN", 0x2dd},
            {"AUCTION_BOUGHT_ESPER1", 0x16c},
            {"AUCTION_BOUGHT_ESPER2", 0x16d},
            {"NAMED_GAU", 0x03f},
            {"DEFEATED_FLAME_EATER", 0x090},
            {"FINISHED_COLLAPSING_HOUSE", 0x28a},
            {"DEFEATED_DULLAHAN", 0x2b2},
            {"FINISHED_DOMA_WOB", 0x040},
            {"DEFEATED_STOOGES", 0x0d8},
            {"FINISHED_DOMA_WOR", 0x0da},
            {"GOT_ALEXANDR", 0x0db},
            {"DEFEATED_HIDON", 0x19c},
            {"DEFEATED_ULTROS_ESPER_MOUNTAIN", 0x095},
            {"DEFEATED_MAGIMASTER", 0x2db},
            {"RECRUITED_STRAGO_FANATICS_TOWER", 0x0ba},
            {"NAMED_EDGAR", 0x004},
            {"DEFEATED_TENTACLES_FIGARO", 0x0c6},
            {"RECRUITED_SHADOW_FLOATING_CONTINENT", 0x02a},
            {"DEFEATED_ATMAWEAPON", 0x0a1},
            {"FINISHED_FLOATING_CONTINENT", 0x0a5},
            {"RECRUITED_SHADOW_GAU_FATHER_HOUSE", 0x162},
            {"FINISHED_IMPERIAL_CAMP", 0x037},
            {"DEFEATED_ATMA", 0x0a2},
            {"RECRUITED_SHADOW_KOHLINGEN", 0x18e},
            {"RODE_RAFT_LETE_RIVER", 0x257},
            {"CHASING_LONE_WOLF7", 0x23f},
            {"GOT_BOTH_REWARDS_LONE_WOLF", 0x241},
            {"GOT_IFRIT_SHIVA", 0x061},
            {"DEFEATED_NUMBER_024", 0x05f},
            {"DEFEATED_CRANES", 0x06b},
            {"RECRUITED_TERRA_MOBLIZ", 0x0bf},
            {"DEFEATED_VARGAS", 0x010},
            {"FINISHED_MT_ZOZO", 0x0d2},
            {"FINISHED_NARSHE_BATTLE", 0x046},
            {"GOT_RAGNAROK", 0x0b6},
            {"GOT_BOTH_REWARDS_WEAPON_SHOP", 0x0b7},
            {"FINISHED_OPERA_DISRUPTION", 0x05b},
            {"DEFEATED_CHADARNOOK", 0x253},
            {"GOT_PHANTOM_TRAIN_REWARD", 0x192},
            {"RECRUITED_LOCKE_PHOENIX_CAVE", 0x0d7},
            {"BLOCK_SEALED_GATE", 0x471},
            {"DEFEATED_DOOM_GAZE", 0x2a1},
            {"GOT_SERPENT_TRENCH_REWARD", 0x050},
            {"FREED_CELES", 0x01d},
            {"DEFEATED_TUNNEL_ARMOR", 0x0b1},
            {"GOT_TRITOCH", 0x29e},
            {"BOUGHT_ESPER_TZEN", 0x27c},
            {"RECRUITED_UMARO_WOR", 0x07e},
            {"VELDT_REWARD_OBTAINED", 0x1bc},
            {"DEFEATED_SR_BEHEMOTH", 0x199},
            {"DEFEATED_WHELK", 0x135},
            {"RECRUITED_GOGO_WOR", 0x0d4},
            {"GOT_ZOZO_REWARD", 0x052},
            {"FINISHED_MOOGLE_DEFENSE", 0x12e}
        };

        /// <summary>
        /// Dictionary holding the names of each event bit.
        /// </summary>
        public static readonly Dictionary<string, string> CheckNamesDict = new()
        {
            {"GOT_RAIDEN", "Ancient Castle"},
            {"DEFEATED_ANCIENT_CASTLE_DRAGON", "Ancient Castle Dragon"},
            {"AUCTION_BOUGHT_ESPER1", "Auction House 1"},
            {"AUCTION_BOUGHT_ESPER2", "Auction House 2"},
            {"NAMED_GAU", "Baren Falls"},
            {"DEFEATED_FLAME_EATER", "Burning House"},
            {"FINISHED_COLLAPSING_HOUSE", "Collapsing House"},
            {"DEFEATED_DULLAHAN", "Darill's Tomb"},
            {"FINISHED_DOMA_WOB", "Doma Siege"},
            {"DEFEATED_STOOGES", "Doma Dream Door"},
            {"FINISHED_DOMA_WOR", "Doma Dream Awaken"},
            {"GOT_ALEXANDR", "Doma Dream Throne"},
            {"DEFEATED_HIDON", "Ebot's Rock"},
            {"DEFEATED_ULTROS_ESPER_MOUNTAIN", "Esper Mountain"},
            {"DEFEATED_FANATICS_TOWER_DRAGON", "Fanatic's Tower Dragon"},
            {"DEFEATED_MAGIMASTER", "Fanatic's Tower Leader"},
            {"RECRUITED_STRAGO_FANATICS_TOWER", "Fanatic's Tower Follower"},
            {"NAMED_EDGAR", "Figaro Castle Throne"},
            {"DEFEATED_TENTACLES_FIGARO", "Figaro Castle Engine"},
            {"RECRUITED_SHADOW_FLOATING_CONTINENT", "Floating Cont. Arrive"},
            {"DEFEATED_ATMAWEAPON", "Floating Cont. Beast"},
            {"FINISHED_FLOATING_CONTINENT", "Floating Cont. Escape"},
            {"RECRUITED_SHADOW_GAU_FATHER_HOUSE", "Gau's Father's House"},
            {"FINISHED_IMPERIAL_CAMP", "Imperial Camp"},
            {"DEFEATED_ATMA", "Kefka's Tower Cell Beast"},
            {"DEFEATED_KEFKA_TOWER_DRAGON_G", "Kefka's Tower Dragon G"},
            {"DEFEATED_KEFKA_TOWER_DRAGON_S", "Kefka's Tower Dragon S"},
            {"RECRUITED_SHADOW_KOHLINGEN", "Kohlingen Cafe"},
            {"RODE_RAFT_LETE_RIVER", "Lete River"},
            {"CHASING_LONE_WOLF7", "Lone Wolf Chase"},
            {"GOT_BOTH_REWARDS_LONE_WOLF", "Lone Wolf Moogle Room"},
            {"GOT_IFRIT_SHIVA", "Magitek Factory Trash"},
            {"DEFEATED_NUMBER_024", "Magitek Factory Guard"},
            {"DEFEATED_CRANES", "Magitek Factory Finish"},
            {"RECRUITED_TERRA_MOBLIZ", "Mobliz Attack"},
            {"DEFEATED_VARGAS", "Mt. Kolts"},
            {"FINISHED_MT_ZOZO", "Mt. Zozo"},
            {"DEFEATED_MT_ZOZO_DRAGON", "Mt. Zozo Dragon"},
            {"FINISHED_NARSHE_BATTLE", "Narshe Battle"},
            {"DEFEATED_NARSHE_DRAGON", "Narshe Dragon"},
            {"GOT_RAGNAROK", "Narshe Weapon Shop"},
            {"GOT_BOTH_REWARDS_WEAPON_SHOP", "Narshe Weapon Shop Mines"},
            {"FINISHED_OPERA_DISRUPTION", "Opera House Disruption"},
            {"DEFEATED_OPERA_HOUSE_DRAGON", "Opera House Dragon"},
            {"DEFEATED_CHADARNOOK", "Owzer's Mansion"},
            {"GOT_PHANTOM_TRAIN_REWARD", "Phantom Train"},
            {"RECRUITED_LOCKE_PHOENIX_CAVE", "Phoenix Cave"},
            {"DEFEATED_PHOENIX_CAVE_DRAGON", "Phoenix Cave Dragon"},
            {"BLOCK_SEALED_GATE", "Sealed Gate"},
            {"DEFEATED_DOOM_GAZE", "Search The Skies"},
            {"GOT_SERPENT_TRENCH_REWARD", "Serpent Trench"},
            {"FREED_CELES", "South Figaro Prisoner"},
            {"DEFEATED_TUNNEL_ARMOR", "South Figaro Cave"},
            {"GOT_TRITOCH", "Tritoch Cliff"},
            {"BOUGHT_ESPER_TZEN", "Tzen Thief"},
            {"RECRUITED_UMARO_WOR", "Umaro's Cave"},
            {"VELDT_REWARD_OBTAINED", "Veldt"},
            {"DEFEATED_SR_BEHEMOTH", "Veldt Cave"},
            {"DEFEATED_WHELK", "Whelk Gate"},
            {"RECRUITED_GOGO_WOR", "Zone Eater"},
            {"GOT_ZOZO_REWARD", "Zozo Tower"},
            {"FINISHED_MOOGLE_DEFENSE", "Narshe Moogle Defense"}
        };

        /// <summary>
        /// Dictionary holding the bytes for each dragon.
        /// </summary>
        public static readonly Dictionary<byte, string> DragonDict = new() {
            [0x04] = "Narshe",
            [0x08] = "Mt Zozo",
            [0x10] = "Opera House",
            [0x20] = "Kefka Tower - Left",
            [0x40] = "Kefka Tower - Right",
            [0x80] = "Ancient Castle",
            [0x01] = "Phoenix Cave",
            [0x02] = "Fanatics Tower"
        };

        /// <summary>
        /// Dictionary holding the map names for each value.
        /// </summary>
        public static readonly Dictionary<uint, string> MapsDict = new()
        {
            {0x000, "World of Balance, World Map"},
            {0x001, "World of Ruin, World Map"},
            {0x002, "Serpent Trench, World Map"},
            {0x003, "Reset"},
            {0x004, "Shadow's Dream 1"},
            {0x005, "Mog's Explanation"},
            {0x006, "Blackjack, Upper Deck (general use)"},
            {0x007, "Blackjack, Inside"},
            {0x008, "Blackjack, Back Room"},
            {0x009, "“Choose A Scenario”"},
            {0x00A, "Blackjack, Upper Deck (IAF sequence)"},
            {0x00B, "Falcon, Upper Deck"},
            {0x00C, "Falcon, Inside"},
            {0x00D, "Falcon, Roof, Landing on Kefka's Tower"},
            {0x00E, "Chocobo Stable, Outside (WOB, forested)"},
            {0x00F, "Chocobo Stable, Inside"},
            {0x010, "Chocobo Stable, Outside (WOR, forested)"},
            {0x011, "Falcon, Upper Deck"},
            {0x012, "Narshe, Northern Cliff, Intro"},
            {0x013, "Narshe, Town, Beginning (WOB)"},
            {0x014, "Narshe, Town (WOB)"},
            {0x015, "Narshe, North of Town, Beginning (WOB)"},
            {0x016, "Narshe, Snow Battlefield (WOB)"},
            {0x017, "Narshe, Northern Cliff (WOB)"},
            {0x018, "Narshe, Weapons Shop"},
            {0x019, "Narshe, Armor Shop"},
            {0x01A, "Narshe, Item Shop"},
            {0x01B, "Narshe, Relics Shop"},
            {0x01C, "Narshe, Inn"},
            {0x01D, "Narshe, Pub"},
            {0x01E, "Narshe, Elder's House"},
            {0x01F, "DUMMIED - Chocobo Stable"},
            {0x020, "Narshe, Outside (WOR)"},
            {0x021, "Narshe, Outside, North (WOR)"},
            {0x022, "Narshe, Outside, Battlefield (WOR)"},
            {0x023, "Narshe, Outside, Northern Cliff (WOR)"},
            {0x024, "Narshe, Mines Cart Rail Rooms (WOR)"},
            {0x025, "Narshe, Mines Battle Room (WOR)"},
            {0x026, "Narshe, Mines 1 (WOR)"},
            {0x027, "Narshe, Outside, North, Beginning"},
            {0x028, "Narshe, Mines 1, Beginning (WOB)"},
            {0x029, "Narshe, Mines 1 (WOB)"},
            {0x02A, "Narshe, Mines Esper Room, Beginning (WOB)"},
            {0x02B, "Narshe, Mines Esper Room, Empty (WOB)"},
            {0x02C, "Narshe, Moogle's Room (WOR)"},
            {0x02D, "DUMMIED"},
            {0x02E, "DUMMIED"},
            {0x02F, "DUMMIED"},
            {0x030, "Narshe, Mines 2, From Secret (WOB)"},
            {0x031, "Narshe, Mines Checkpoint (WOB)"},
            {0x032, "Narshe, Mines 2 (WOB)"},
            {0x033, "Narshe, Mines Battle Room (WOB)"},
            {0x034, "Narshe, Moogle's Room (WOB)"},
            {0x035, "Cave to Figaro Castle, Room 2"},
            {0x036, "Figaro Castle, Submerging"},
            {0x037, "Figaro Castle, Outside"},
            {0x038, "Figaro Desert, Kefka & Troops"},
            {0x039, "Figaro Castle, King's Bedroom"},
            {0x03A, "Figaro Castle, Throne Room"},
            {0x03B, "Figaro Castle, Inside"},
            {0x03C, "Figaro Castle, Library"},
            {0x03D, "Figaro Castle, Basement 1"},
            {0x03E, "Figaro Castle, Basement 2"},
            {0x03F, "Figaro Castle, Basement 3"},
            {0x040, "Figaro Castle, Engine Room"},
            {0x041, "Figaro Castle, Treasure Room"},
            {0x042, "Figaro Castle, Regal Crown Room"},
            {0x043, "Figaro Castle, Outside, Nighttime"},
            {0x044, "Cave to South Figaro (WOR)"},
            {0x045, "Cave to South Figaro, Rooms"},
            {0x046, "Cave to South Figaro (WOB)"},
            {0x047, "Cave to South Figaro, Entrance"},
            {0x048, "Cave to South Figaro, Room 2"},
            {0x049, "Cave to South Figaro, Room 1"},
            {0x04A, "South Figaro, Outside (WOR)"},
            {0x04B, "South Figaro, Outside (WOB)"},
            {0x04C, "South Figaro, Inn/Relics"},
            {0x04D, "South Figaro, Arsenal"},
            {0x04E, "South Figaro, Pub"},
            {0x04F, "DUMMIED"},
            {0x050, "South Figaro, Chocobo Stable"},
            {0x051, "South Figaro, Rich Man's House"},
            {0x052, "DUMMIED"},
            {0x053, "South Figaro, Basement 1"},
            {0x054, "South Figaro, Basement 1 Clock Room"},
            {0x055, "South Figaro, Item Shop"},
            {0x056, "South Figaro, Duncan's House"},
            {0x057, "South Figaro, Basement 1 Monster Room"},
            {0x058, "South Figaro, Basement 1 Save Point"},
            {0x059, "South Figaro, Basement 2"},
            {0x05A, "Cave to Figaro Castle, Entrance (WOR)"},
            {0x05B, "South Figaro, Dock (WOR)"},
            {0x05C, "Cave to Figaro Castle, Room 1"},
            {0x05D, "Sabin's House, Outside"},
            {0x05E, "Sabin's House, Inside"},
            {0x05F, "Mt.Kolts, Entrance"},
            {0x060, "Mt.Kolts, Outside"},
            {0x061, "Mt.Kolts, Outside Bridge"},
            {0x062, "Mt.Kolts, Outside Vargas Area"},
            {0x063, "DUMMIED"},
            {0x064, "Mt.Kolts, Cave"},
            {0x065, "Mt.Kolts, Exit"},
            {0x066, "Mt.Kolts, Cliffs"},
            {0x067, "Mt.Kolts, Cave Save Point"},
            {0x068, "Narshe, School"},
            {0x069, "Narshe, School, Adv. Battle Tactics"},
            {0x06A, "Narshe, School, Battle Tactics"},
            {0x06B, "Narshe, School, Environmental Science"},
            {0x06C, "Returner's Hideout, Entrance"},
            {0x06D, "Returner's Hideout, Inside"},
            {0x06E, "Returner's Hideout, Rooms"},
            {0x06F, "Returner's Hideout, Inn"},
            {0x070, "Tunnel to Lete River"},
            {0x071, "Lete River"},
            {0x072, "Lete River Cave"},
            {0x073, "Gau's Father's House, Outside"},
            {0x074, "Gau's Father's House, Inside"},
            {0x075, "Military Base Camp"},
            {0x076, "DUMMIED"},
            {0x077, "Military Base Camp 2"},
            {0x078, "Doma Castle, Outside (WOB)"},
            {0x079, "Doma Castle, Outside Poisoning (WOB)"},
            {0x07A, "DUMMIED"},
            {0x07B, "Doma Castle, Inside"},
            {0x07C, "Doma Castle, Cyan's Room"},
            {0x07D, "Cyan's Dream, Doma Castle, Outside"},
            {0x07E, "Cyan's Dream, Doma Castle Rooms"},
            {0x07F, "Duncan's House, Outside"},
            {0x080, "Duncan's House, Inside"},
            {0x081, "Ending, sky with birds"},
            {0x082, "Ending, Phantom Forest"},
            {0x083, "Gau's Father's House, Outside (WOR)"},
            {0x084, "Phantom Forest 1"},
            {0x085, "Phantom Forest 2"},
            {0x086, "Phantom Forest 3"},
            {0x087, "Phantom Forest 4"},
            {0x088, "Ending, mountain bridge"},
            {0x089, "Phantom Train, Docking Station 4"},
            {0x08A, "Phantom Train, Docking Station 2"},
            {0x08B, "Phantom Train, Docking Station 3"},
            {0x08C, "Phantom Train, Docking Station 1"},
            {0x08D, "Phantom Train, Outside 2"},
            {0x08E, "Phantom Train, Outside 1"},
            {0x08F, "Cyan's Dream, Phantom Train, Outside"},
            {0x090, "Cyan's Dream, Phantom Train, Inside"},
            {0x091, "Phantom Train, Inside 1"},
            {0x092, "Phantom Train, Engineer's Room"},
            {0x093, "Phantom Train, Restaurant"},
            {0x094, "Mobliz, Outside during Light of Judgement"},
            {0x095, "Phantom Train, Inside 2"},
            {0x096, "Mobliz, Basement 2, After Phunbaba 1"},
            {0x097, "Phantom Train, Inside 3"},
            {0x098, "Phantom Train, Inside 4"},
            {0x099, "Phantom Train, Inside Rooms"},
            {0x09A, "Mobliz, Basement 2"},
            {0x09B, "Waterfall Cave"},
            {0x09C, "Waterfall Cliff"},
            {0x09D, "Mobliz, Outside (WOB)"},
            {0x09E, "Mobliz, Outside (WOR)"},
            {0x09F, "Veldt Shore"},
            {0x0A0, "Mobliz, Inn"},
            {0x0A1, "Mobliz, Arsenal"},
            {0x0A2, "Mobliz, Relics"},
            {0x0A3, "Mobliz, Mail House"},
            {0x0A4, "Mobliz, Item Shop"},
            {0x0A5, "Mobliz, Basement 1"},
            {0x0A6, "Waterfall Entrance"},
            {0x0A7, "Veldt Cave to Waterfall"},
            {0x0A8, "Veldt Waterfall"},
            {0x0A9, "Nikeah, Outside"},
            {0x0AA, "Veldt Shore"},
            {0x0AB, "Nikeah, Inn"},
            {0x0AC, "Nikeah, Pub"},
            {0x0AD, "Nikeah, Chocobo Stable"},
            {0x0AE, "Shadow's Dream 2, Phantom Forest"},
            {0x0AF, "Serpent Trench Cave 1"},
            {0x0B0, "Mt.Zozo, Outside Bridge"},
            {0x0B1, "Mt.Zozo, Outside 1"},
            {0x0B2, "Mt.Zozo, Outside 2"},
            {0x0B3, "Mt.Zozo, Inside"},
            {0x0B4, "Mt.Zozo, Cyan's Room"},
            {0x0B5, "Mt.Zozo, Cliff"},
            {0x0B6, "Ending, Relm, Shadow, Strago"},
            {0x0B7, "Mobliz, Basement 2, Before Kefka Fight"},
            {0x0B8, "Mobliz, Outside, Before Kefka Fight"},
            {0x0B9, "Crazy Man's House, Outside"},
            {0x0BA, "Crazy Man's House, Inside"},
            {0x0BB, "Nikeah, Ferry (WOB)"},
            {0x0BC, "Kohlingen, Outside (WOB)"},
            {0x0BD, "Kohlingen, Outside (WOR)"},
            {0x0BE, "Ending, Rapids"},
            {0x0BF, "Kohlingen, Inn (WOR)"},
            {0x0C0, "DUMMIED - Kohlingen"},
            {0x0C1, "DUMMIED - Kohlingen, Arsenal"},
            {0x0C2, "Kohlingen, General Store"},
            {0x0C3, "Kohlingen, Chemist's House"},
            {0x0C4, "Maranda, Flower Girl's House (WOR)"},
            {0x0C5, "Kohlingen, Rachel's House"},
            {0x0C6, "Jidoor, Outside"},
            {0x0C7, "FREE TO USE"},
            {0x0C8, "Jidoor, Auction House"},
            {0x0C9, "Jidoor, Item Shop"},
            {0x0CA, "Jidoor, Relics"},
            {0x0CB, "Jidoor, Armor Shop"},
            {0x0CC, "Jidoor, Weapon Shop"},
            {0x0CD, "Jidoor, Chocobo Stable"},
            {0x0CE, "Jidoor, Inn"},
            {0x0CF, "Owzer's House, Basement"},
            {0x0D0, "Owzer's House, Art Room"},
            {0x0D1, "Owzer's House, F1"},
            {0x0D2, "Ending, Cyan 1"},
            {0x0D3, "Ending, Umaro, Mog 1"},
            {0x0D4, "Ending, Gogo 1"},
            {0x0D5, "Ending, Gau 1"},
            {0x0D6, "Cliff, Setzer waits for Darill"},
            {0x0D7, "Ending, Falcon flying through sky"},
            {0x0D8, "Esper World, Lake"},
            {0x0D9, "Esper World, Outside"},
            {0x0DA, "Esper World, Gate"},
            {0x0DB, "Esper World, House"},
            {0x0DC, "DUMMIED - boat dock"},
            {0x0DD, "Zozo, Outside"},
            {0x0DE, "FREE TO USE"},
            {0x0DF, "FREE TO USE"},
            {0x0E0, "FREE TO USE"},
            {0x0E1, "Zozo, Inside House"},
            {0x0E2, "Zozo, Terra's Room"},
            {0x0E3, "FREE TO USE"},
            {0x0E4, "FREE TO USE"},
            {0x0E5, "FREE TO USE"},
            {0x0E6, "FREE TO USE"},
            {0x0E7, "Opera House, Theater"},
            {0x0E8, "Opera House, Switch Room"},
            {0x0E9, "Opera House, Theater Play 2"},
            {0x0EA, "Opera House, Theater Play 1"},
            {0x0EB, "Opera House, Ceiling"},
            {0x0EC, "Opera House, Castle Balcony"},
            {0x0ED, "Opera House, Main"},
            {0x0EE, "Opera House, Dressing Room"},
            {0x0EF, "DUMMIED - Opera House"},
            {0x0F0, "Vector, after Magitek Factory"},
            {0x0F1, "Imperial Castle, Cranes Activating"},
            {0x0F2, "Vector, Outside"},
            {0x0F3, "Imperial Castle"},
            {0x0F4, "Imperial Castle, Outside Roof"},
            {0x0F5, "Vector, Inn"},
            {0x0F6, "Vector, Weapon Shop"},
            {0x0F7, "Vector, Pub"},
            {0x0F8, "Vector, Armor Shop"},
            {0x0F9, "Vector, Healer's House"},
            {0x0FA, "Imperial Castle, Inside"},
            {0x0FB, "Imperial Castle, Banquet"},
            {0x0FC, "Imperial Castle, Bedroom"},
            {0x0FD, "Vector, Outside, Burning"},
            {0x0FE, "Ending, Cyan, Mog, Gogo 2"},
            {0x0FF, "Ending, Setzer"},
            {0x100, "Ending, Umaro 2"},
            {0x101, "DUMMIED - Magitek Factory"},
            {0x102, "Ending, Edgar and Sabin"},
            {0x103, "Ending, Falcon taking flight from Kefka's Tower"},
            {0x104, "Ending, Locke and Celes 1"},
            {0x105, "Ending, Shadow, Strago, Gau 2"},
            {0x106, "Magitek Factory, Room 1"},
            {0x107, "Magitek Factory, Room 2"},
            {0x108, "Magitek Factory, Room 3 (Espers)"},
            {0x109, "DUMMIED - Magitek Factory"},
            {0x10A, "Magitek Factory, Elevator"},
            {0x10B, "DUMMIED - Magitek Factory"},
            {0x10C, "Ending, Terra"},
            {0x10D, "Magitek Factory, Room 4"},
            {0x10E, "Magitek Factory, Room 5 (Save)"},
            {0x10F, "Magitek Res. Facility, Room 1"},
            {0x110, "Magitek Factory, Minecart Dock"},
            {0x111, "Magitek Res. Facility, Room 2 (Number 042)"},
            {0x112, "Magitek Res. Facility, Espers"},
            {0x113, "DUMMIED"},
            {0x114, "Zone Eater's Stomach, Room 1"},
            {0x115, "Zone Eater's Stomach, Room 4"},
            {0x116, "Zone Eater's Stomach, Room 6 (Gogo)"},
            {0x117, "Zone Eater's Stomach, Room 2"},
            {0x118, "Zone Eater's Stomach, Room 3,5"},
            {0x119, "Narshe, Umaro's Cave 1"},
            {0x11A, "Narshe, Umaro's Cave 2"},
            {0x11B, "Narshe, Umaro's Lair"},
            {0x11C, "Maranda, Outside (WOR)"},
            {0x11D, "Doma Castle, Outside, Abandoned"},
            {0x11E, "FREE TO USE"},
            {0x11F, "Kefka's Tower, Inside From Guardian"},
            {0x120, "Maranda, Inn (WOR)"},
            {0x121, "Maranda, Weapon Shop (WOR)"},
            {0x122, "Maranda, Armor Shop (WOR)"},
            {0x123, "Kefka's Tower, Guardian's Room"},
            {0x124, "Kefka's Tower, Junk Room"},
            {0x125, "Kefka's Tower, Inside from Factory Room"},
            {0x126, "Kefka's Tower, Inside 2"},
            {0x127, "Kefka's Tower, Inside 3"},
            {0x128, "Kefka's Tower, Factory Room (Top Level)"},
            {0x129, "Darill's Tomb, Outside"},
            {0x12A, "Darill's Tomb, Basement 1"},
            {0x12B, "Darill's Tomb, Basement 2"},
            {0x12C, "Darill's Tomb, Basement 3"},
            {0x12D, "Darill's Tomb, Staircase"},
            {0x12E, "Ending, Thamasa, Repairing burned house"},
            {0x12F, "Kefka's Tower, Inside 1"},
            {0x130, "Kefka's Tower, Assassin Room"},
            {0x131, "Tzen, Outside (WOR)"},
            {0x132, "Tzen, Outside, Before House Collapse (WOR)"},
            {0x133, "Tzen, Item Shop (WOR)"},
            {0x134, "Tzen, Inn (WOR)"},
            {0x135, "Tzen, Weapon Shop (WOR)"},
            {0x136, "Tzen, Armor Shop (WOR)"},
            {0x137, "Tzen, Inside Collapsing House"},
            {0x138, "Tzen, Relics (WOR)"},
            {0x139, "Phoenix Cave, Big Lava Room, Drained"},
            {0x13A, "Phoenix Cave, Big Lava Room"},
            {0x13B, "Phoenix Cave, Main Room, Drained"},
            {0x13C, "Phoenix Cave, Main Room"},
            {0x13D, "Cyan's Dream, Three Stooges"},
            {0x13E, "Phoenix Cave, Outside Entrance"},
            {0x13F, "Cyan's Dream, Magitek Caves, Outside"},
            {0x140, "Cyan's Dream, Magitek Caves, Inside"},
            {0x141, "Cyan's Dream, Phantom Train, Inside 1"},
            {0x142, "Cyan's Dream, Phantom Train, Inside 2"},
            {0x143, "Albrook, Outside (WOB)"},
            {0x144, "Albrook, Outside (WOR)"},
            {0x145, "Albrook, Inn"},
            {0x146, "Albrook, Weapon Shop"},
            {0x147, "Albrook, Armor Shop"},
            {0x148, "Albrook, Item Shop"},
            {0x149, "Kefka's Tower, Room with Movers"},
            {0x14A, "Albrook, Pub"},
            {0x14B, "Kefka's Tower, Atma's Room"},
            {0x14C, "Albrook, Ship Deck"},
            {0x14D, "DUMMIED - Kefka's Tower, factory"},
            {0x14E, "Kefka's Tower, Outside"},
            {0x14F, "Kefka's Tower, Gold Dragon Room"},
            {0x150, "Kefka's Tower, Kefka's Lair 1"},
            {0x151, "Kefka's Tower, 4Ton Switch Room"},
            {0x152, "Kefka's Tower, Inside from Central Group"},
            {0x153, "Kefka's Tower, Inside from Eastern Group"},
            {0x154, "Thamasa, Outside, at Leo's Grave"},
            {0x155, "Thamasa, Outside, Kefka attacks"},
            {0x156, "DUMMIED - Thamasa"},
            {0x157, "Thamasa, Outside (WOB)"},
            {0x158, "Thamasa, Outside (WOR)"},
            {0x159, "Thamasa, Arsenal"},
            {0x15A, "Thamasa, Inn"},
            {0x15B, "Thamasa, Item Shop"},
            {0x15C, "Thamasa, Elder's House"},
            {0x15D, "Thamasa, Strago's House"},
            {0x15E, "Thamasa, Relics"},
            {0x15F, "Thamasa, Inside Burning House"},
            {0x160, "Kefka's Tower, Broken Capsules"},
            {0x161, "Cave in the Veldt (WOR)"},
            {0x162, "Kefka's Tower, Red Carpet Rooms"},
            {0x163, "Kefka's Tower, Three Switch Room"},
            {0x164, "Kefka's Tower, Left Statue Room"},
            {0x165, "Kefka's Tower, Kefka's Lair 2"},
            {0x166, "Floating Continent, Save Point"},
            {0x167, "Fanatic's Tower, Level 2"},
            {0x168, "Fanatic's Tower, Level 3"},
            {0x169, "Fanatic's Tower, Level 4"},
            {0x16A, "Fanatic's Tower, Entrance"},
            {0x16B, "Fanatic's Tower, Level 1"},
            {0x16C, "Fanatic's Tower, Roof"},
            {0x16D, "Fanatic's Tower, Treasure Room 2"},
            {0x16E, "Fanatic's Tower, Gem Box Room"},
            {0x16F, "Fanatic's Tower, Treasure Room 3"},
            {0x170, "Fanatic's Tower, Treasure Room 4"},
            {0x171, "Fanatic's Tower, Treasure Room 5"},
            {0x172, "Fanatic's Tower, Treasure Room 1"},
            {0x173, "Esper Cave, Statue Room"},
            {0x174, "Esper Cave, Outside 2"},
            {0x175, "Esper Cave, Outside 1"},
            {0x176, "Esper Cave, Outside 3"},
            {0x177, "Esper Cave"},
            {0x178, "Floating Continent, Destruction"},
            {0x179, "Imperial Base"},
            {0x17A, "Imperial Base, House"},
            {0x17B, "DUMMIED - Imperial Base"},
            {0x17C, "Overhead View of World Map"},
            {0x17D, "Mountains Destruction"},
            {0x17E, "Cave to the Sealed Gate, Room 1"},
            {0x17F, "Cave to the Sealed Gate, Basement 1"},
            {0x180, "Cave to the Sealed Gate, Basement 3"},
            {0x181, "Cave to the Sealed Gate, Basement 2"},
            {0x182, "Cave to the Sealed Gate, Save Point"},
            {0x183, "DUMMIED - Cave to the Sealed Gate, Basement 4"},
            {0x184, "DUMMIED - Cave to the Sealed Gate"},
            {0x185, "Grasslands Destruction"},
            {0x186, "Highlands Destruction"},
            {0x187, "Sealed Gate"},
            {0x188, "Floating Continent shadow cast above Jidoor"},
            {0x189, "Floating Continent, Breaking Apart"},
            {0x18A, "Floating Continent, Outside"},
            {0x18B, "Floating Continent Breakaway 1"},
            {0x18C, "Cid's Island, Outside"},
            {0x18D, "Cid's Island, Inside House"},
            {0x18E, "Cid's Island, Beach"},
            {0x18F, "Cid's Island, Cliff"},
            {0x190, "Cid's Island, Beach, no fish"},
            {0x191, "Ancient Cave 1"},
            {0x192, "Ancient Cave 2"},
            {0x193, "DUMMIED - Cid's Island, Beach"},
            {0x194, "Hidon's Cave"},
            {0x195, "Hidon's Cave, Entrance"},
            {0x196, "Ancient Castle, Inside"},
            {0x197, "Ancient Castle, Outside"},
            {0x198, "Ancient Castle, Eastern Room"},
            {0x199, "Kefka's Tower, Pipe Room"},
            {0x19A, "Kefka's Tower, Factory Room (Bottom Level)"},
            {0x19B, "Kefka's Tower, Final Room"},
            {0x19C, "Kefka's Tower, Guardian Path Save Point"},
            {0x19D, "Colosseum"},
            {0x19E, "DUMMIED"}
        };

        /// <summary>
        /// A dictionary holding the check peeks that are determined by map ID.
        /// </summary>
        public static readonly Dictionary<int, string> PeeksByMapId = new()
        {
            {0x198, "Ancient Castle"},
            {0x09C, "Baren Falls"},
            {0x15F, "Burning House"},
            {0x078, "Doma Siege"},
            {0x13D, "Doma Dream Door"},
            {0x07E, "Doma Dream Awaken"},
            {0x195, "Ebot's Rock"},
            {0x16A, "Fanatic's Tower Follower"},
            {0x03A, "Figaro Castle Throne"},
            {0x05A, "Figaro Castle Engine"},
            {0x18A, "Floating Cont. Beast"},
            {0x189, "Floating Cont. Escape"},
            {0x073, "Gau's Father's House"},
            {0x0BF, "Kohlingen Cafe"},
            {0x071, "Lete River"},
            {0x017, "Lone Wolf Chase"},
            {0x108, "Magitek Factory Trash"},
            {0x111, "Magitek Factory Guard"},
            {0x110, "Magitek Factory Finish"},
            {0x0A5, "Mobliz Attack"},
            {0x062, "Mt. Kolts"},
            {0x0B5, "Mt. Zozo"},
            {0x016, "Narshe Battle"},
            {0x0EB, "Opera House Disruption"},
            {0x0D0, "Owzer's Mansion"},
            {0x08E, "Phantom Train"},
            {0x139, "Phoenix Cave"},
            {0x187, "Sealed Gate"},
            {0x002, "Serpent Trench"},
            {0x023, "Tritoch Cliff"},
            {0x0E2, "Zozo Tower"},
            {0x032, "Narshe Moogle Defense"},
            {0x116, "Zone Eater"}
        };

        /// <summary>
        /// A dictionary holding the check peeks that are determined by event bits.
        /// </summary>
        public static readonly Dictionary<int, string> PeeksByEventBit = new()
        {
            {0x27d, "Collapsing House"},
            {0x2b6, "Darill's Tomb"},
            {0x155, "Imperial Camp"},
            {0x0b0, "South Figaro Cave"},
            {0x195, "Veldt Cave"}
        };

        /// <summary>
        /// A dictionary holding the names for each item value.
        /// </summary>
        public static readonly Dictionary<byte, string> ItemDict = new()
        {
            {0, "Dirk"},
            {1, "MithrilKnife"},
            {2, "Guardian"},
            {3, "Air Lancet"},
            {4, "ThiefKnife"},
            {5, "Assassin"},
            {6, "Man Eater"},
            {7, "SwordBreaker"},
            {8, "Graedus"},
            {9, "ValiantKnife"},
            {10, "MithrilBlade"},
            {11, "RegalCutlass"},
            {12, "Rune Edge"},
            {13, "Flame Sabre"},
            {14, "Blizzard"},
            {15, "ThunderBlade"},
            {16, "Epee"},
            {17, "Break Blade"},
            {18, "Drainer"},
            {19, "Enhancer"},
            {20, "Crystal"},
            {21, "Falchion"},
            {22, "Soul Sabre"},
            {23, "Ogre Nix"},
            {24, "Excalibur"},
            {25, "Scimitar"},
            {26, "Illumina"},
            {27, "Ragnarok"},
            {28, "Atma Weapon"},
            {29, "Mithril Pike"},
            {30, "Trident"},
            {31, "Stout Spear"},
            {32, "Partisan"},
            {33, "Pearl Lance"},
            {34, "Gold Lance"},
            {35, "Aura Lance"},
            {36, "Imp Halberd"},
            {37, "Imperial"},
            {38, "Kodachi"},
            {39, "Blossom"},
            {40, "Hardened"},
            {41, "Striker"},
            {42, "Stunner"},
            {43, "Ashura"},
            {44, "Kotetsu"},
            {45, "Forged"},
            {46, "Tempest"},
            {47, "Murasame"},
            {48, "Aura"},
            {49, "Strato"},
            {50, "Sky Render"},
            {51, "Heal Rod"},
            {52, "Mithril Rod"},
            {53, "Fire Rod"},
            {54, "Ice Rod"},
            {55, "Thunder Rod"},
            {56, "Poison Rod"},
            {57, "Pearl Rod"},
            {58, "Gravity Rod"},
            {59, "Punisher"},
            {60, "Magus Rod"},
            {61, "Chocobo Brsh"},
            {62, "DaVinci Brsh"},
            {63, "Magical Brsh"},
            {64, "Rainbow Brsh"},
            {65, "Shuriken"},
            {66, "Ninja Star"},
            {67, "Tack Star"},
            {68, "Flail"},
            {69, "Full Moon"},
            {70, "Morning Star"},
            {71, "Boomerang"},
            {72, "Rising Sun"},
            {73, "Hawk Eye"},
            {74, "Bone Club"},
            {75, "Sniper"},
            {76, "Wing Edge"},
            {77, "Cards"},
            {78, "Darts"},
            {79, "Doom Darts"},
            {80, "Trump"},
            {81, "Dice"},
            {82, "Fixed Dice"},
            {83, "MetalKnuckle"},
            {84, "Mithril Claw"},
            {85, "Kaiser"},
            {86, "Poison Claw"},
            {87, "Fire Knuckle"},
            {88, "Dragon Claw"},
            {89, "Tiger Fangs"},
            {90, "Buckler"},
            {91, "Heavy Shld"},
            {92, "Mithril Shld"},
            {93, "Gold Shld"},
            {94, "Aegis Shld"},
            {95, "Diamond Shld"},
            {96, "Flame Shld"},
            {97, "Ice Shld"},
            {98, "Thunder Shld"},
            {99, "Crystal Shld"},
            {100, "Genji Shld"},
            {101, "TortoiseShld"},
            {102, "Cursed Shld"},
            {103, "Paladin Shld"},
            {104, "Force Shld"},
            {105, "Leather Hat"},
            {106, "Hair Band"},
            {107, "Plumed Hat"},
            {108, "Beret"},
            {109, "Magus Hat"},
            {110, "Bandana"},
            {111, "Iron Helmet"},
            {112, "Coronet"},
            {113, "Bard's Hat"},
            {114, "Green Beret"},
            {115, "Head Band"},
            {116, "Mithril Helm"},
            {117, "Tiara"},
            {118, "Gold Helmet"},
            {119, "Tiger Mask"},
            {120, "Red Cap"},
            {121, "Mystery Veil"},
            {122, "Circlet"},
            {123, "Regal Crown"},
            {124, "Diamond Helm"},
            {125, "Dark Hood"},
            {126, "Crystal Helm"},
            {127, "Oath Veil"},
            {128, "Cat Hood"},
            {129, "Genji Helmet"},
            {130, "Thornlet"},
            {131, "Titanium"},
            {132, "LeatherArmor"},
            {133, "Cotton Robe"},
            {134, "Kung Fu Suit"},
            {135, "Iron Armor"},
            {136, "Silk Robe"},
            {137, "Mithril Vest"},
            {138, "Ninja Gear"},
            {139, "White Dress"},
            {140, "Mithril Mail"},
            {141, "Gaia Gear"},
            {142, "Mirage Vest"},
            {143, "Gold Armor"},
            {144, "Power Sash"},
            {145, "Light Robe"},
            {146, "Diamond Vest"},
            {147, "Red Jacket"},
            {148, "Force Armor"},
            {149, "DiamondArmor"},
            {150, "Dark Gear"},
            {151, "Tao Robe"},
            {152, "Crystal Mail"},
            {153, "Czarina Gown"},
            {154, "Genji Armor"},
            {155, "Imp's Armor"},
            {156, "Minerva"},
            {157, "Tabby Suit"},
            {158, "Chocobo Suit"},
            {159, "Moogle Suit"},
            {160, "Nutkin Suit"},
            {161, "BehemothSuit"},
            {162, "Snow Muffler"},
            {163, "NoiseBlaster"},
            {164, "Bio Blaster"},
            {165, "Flash"},
            {166, "Chain Saw"},
            {167, "Debilitator"},
            {168, "Drill"},
            {169, "Air Anchor"},
            {170, "AutoCrossbow"},
            {171, "Fire Skean"},
            {172, "Water Edge"},
            {173, "Bolt Edge"},
            {174, "Inviz Edge"},
            {175, "Shadow Edge"},
            {176, "Goggles"},
            {177, "Star Pendant"},
            {178, "Peace Ring"},
            {179, "Amulet"},
            {180, "White Cape"},
            {181, "Jewel Ring"},
            {182, "Fairy Ring"},
            {183, "Barrier Ring"},
            {184, "MithrilGlove"},
            {185, "Guard Ring"},
            {186, "RunningShoes"},
            {187, "Wall Ring"},
            {188, "Cherub Down"},
            {189, "Cure Ring"},
            {190, "True Knight"},
            {191, "DragoonBoots"},
            {192, "Zephyr Cape"},
            {193, "Czarina Ring"},
            {194, "Cursed Ring"},
            {195, "Earrings"},
            {196, "Atlas Armlet"},
            {197, "Blizzard Orb"},
            {198, "Rage Ring"},
            {199, "Sneak Ring"},
            {200, "Pod Bracelet"},
            {201, "Hero Ring"},
            {202, "Ribbon"},
            {203, "Muscle Belt"},
            {204, "Crystal Orb"},
            {205, "Gold Hairpin"},
            {206, "Economizer"},
            {207, "Thief Glove"},
            {208, "Gauntlet"},
            {209, "Genji Glove"},
            {210, "Hyper Wrist"},
            {211, "Offering"},
            {212, "Beads"},
            {213, "Black Belt"},
            {214, "Coin Toss"},
            {215, "FakeMustache"},
            {216, "Gem Box"},
            {217, "Dragon Horn"},
            {218, "Merit Award"},
            {219, "Memento Ring"},
            {220, "Safety Bit"},
            {221, "Relic Ring"},
            {222, "Moogle Charm"},
            {223, "Charm Bangle"},
            {224, "Marvel Shoes"},
            {225, "Back Guard"},
            {226, "Gale Hairpin"},
            {227, "Sniper Sight"},
            {228, "Exp. Egg"},
            {229, "Tintinabar"},
            {230, "Sprint Shoes"},
            {231, "Rename Card"},
            {232, "Tonic"},
            {233, "Potion"},
            {234, "X-Potion"},
            {235, "Tincture"},
            {236, "Ether"},
            {237, "X-Ether"},
            {238, "Elixir"},
            {239, "Megalixir"},
            {240, "Fenix Down"},
            {241, "Revivify"},
            {242, "Antidote"},
            {243, "Eyedrop"},
            {244, "Soft"},
            {245, "Remedy"},
            {246, "Sleeping Bag"},
            {247, "Tent"},
            {248, "Green Cherry"},
            {249, "Magicite"},
            {250, "Super Ball"},
            {251, "Echo Screen"},
            {252, "Smoke Bomb"},
            {253, "Warp Stone"},
            {254, "Dried Meat"},
            {255, "Empty"}
        };

        /// <summary>
        /// Dictionary holding the index of each spell.
        /// </summary>
        public static readonly Dictionary<byte, string> SpellDict = new()
        {
            {0x00, "Fire"},
            {0x01, "Ice"},
            {0x02, "Bolt"},
            {0x03, "Poison"},
            {0x04, "Drain"},
            {0x05, "Fire2"},
            {0x06, "Ice2"},
            {0x07, "Bolt2"},
            {0x08, "Bio"},
            {0x09, "Fire3"},
            {0x0A, "Ice3"},
            {0x0B, "Bolt3"},
            {0x0C, "Break"},
            {0x0D, "Doom"},
            {0x0E, "Pearl"},
            {0x0F, "Flare"},
            {0x10, "Demi"},
            {0x11, "Quartr"},
            {0x12, "X-Zone"},
            {0x13, "Meteor"},
            {0x14, "Ultima"},
            {0x15, "Quake"},
            {0x16, "WWind"},
            {0x17, "Merton"},
            {0x18, "Scan"},
            {0x19, "Slow"},
            {0x1A, "Rasp"},
            {0x1B, "Mute"},
            {0x1C, "Safe"},
            {0x1D, "Sleep"},
            {0x1E, "Muddle"},
            {0x1F, "Haste"},
            {0x20, "Stop"},
            {0x21, "Bserk"},
            {0x22, "Float"},
            {0x23, "Imp"},
            {0x24, "Rflect"},
            {0x25, "Shell"},
            {0x26, "Vanish"},
            {0x27, "Haste2"},
            {0x28, "Slow2"},
            {0x29, "Osmose"},
            {0x2A, "Warp"},
            {0x2B, "Quick"},
            {0x2C, "Dispel"},
            {0x2D, "Cure"},
            {0x2E, "Cure2"},
            {0x2F, "Cure3"},
            {0x30, "Life"},
            {0x31, "Life2"},
            {0x32, "Antdot"},
            {0x33, "Remedy"},
            {0x34, "Regen"},
            {0x35, "Life3"}
        };

        /// <summary>
        /// Dictionary holding the index of each SwdTech.
        /// </summary>
        public static readonly Dictionary<byte, string> SwdTechDict = new()
        {
            { 0, "Dispatch" },
            { 1, "Retort" },
            { 2, "Slash" },
            { 3, "QuadraSlam" },
            { 4, "Empowerer" },
            { 5, "Stunner" },
            { 6, "QuadraSlice" },
            { 7, "Cleave" }
        };

        /// <summary>
        /// Dictionary holding the index of each Blitz.
        /// </summary>
        public static readonly Dictionary<byte, string> BlitzDict = new()
        {
            { 0, "Pummel" },
            { 1, "AuraBolt" },
            { 2, "Suplex" },
            { 3, "FireDance" },
            { 4, "Mantra" },
            { 5, "AirBlade" },
            { 6, "Spiraler" },
            { 7, "BumRush" }
        };

        /// <summary>
        /// Dictionary holding the index of each Lore.
        /// </summary>
        public static readonly Dictionary<byte, string> LoreDict = new()
        {
            { 0, "Condemned" },
            { 1, "Roulette" },
            { 2, "CleanSweep" },
            { 3, "AquaRake" },
            { 4, "Aero" },
            { 5, "BlowFish" },
            { 6, "BigGuard" },
            { 7, "Revenge" },
            { 8, "PearlWind" },
            { 9, "L.5Doom" },
            { 10, "L.4Flare" },
            { 11, "L.3Muddle" },
            { 12, "Reflect???" },
            { 13, "L.?Pearl" },
            { 14, "StepMine" },
            { 15, "ForceField" },
            { 16, "Dischord" },
            { 17, "SourMouth" },
            { 18, "PepUp" },
            { 19, "Rippler" },
            { 20, "Stone" },
            { 21, "Quasar" },
            { 22, "GrandTrain" },
            { 23, "Exploder" }
        };

        /// <summary>
        /// Dictionary holding the name for each esper byte.
        /// </summary>
        public static readonly Dictionary<byte, string> EsperDict = new()
        {
            {0x00, "Ramuh"},
            {0x01, "Ifrit"},
            {0x02, "Shiva"},
            {0x03, "Siren"},
            {0x04, "Terrato"},
            {0x05, "Shoat"},
            {0x06, "Maduin"},
            {0x07, "Bismark"},
            {0x08, "Stray"},
            {0x09, "Palidor"},
            {0x0A, "Tritoch"},
            {0x0B, "Odin"},
            {0x0C, "Raiden"},
            {0x0D, "Bahamut"},
            {0x0E, "Alexandr"},
            {0x0F, "Crusader"},
            {0x10, "Ragnarok"},
            {0x11, "Kirin"},
            {0x12, "ZoneSeek"},
            {0x13, "Carbunkl"},
            {0x14, "Phantom"},
            {0x15, "Sraphim"},
            {0x16, "Golem"},
            {0x17, "Unicorn"},
            {0x18, "Fenrir"},
            {0x19, "Starlet"},
            {0x1A, "Phoenix"},
            {0xFF, "Empty"}
        };

        /// <summary>
        /// Dictionary holding the byte value for each command in memory.
        /// </summary>
        public static readonly Dictionary<byte, string> CommandDict = new()
        {
            [0] = "Fight",
            [1] = "Item",
            [2] = "Magic",
            [3] = "Morph",
            [4] = "Revert",
            [5] = "Steal",
            [6] = "Capture",
            [7] = "SwdTech",
            [8] = "Throw",
            [9] = "Tools",
            [10] = "Blitz",
            [11] = "Runic",
            [12] = "Lore",
            [13] = "Sketch",
            [14] = "Control",
            [15] = "Slot",
            [16] = "Rage",
            [17] = "Leap",
            [18] = "Mimic",
            [19] = "Dance",
            [20] = "Row",
            [21] = "Def.",
            [22] = "Jump",
            [23] = "X Magic",
            [24] = "GP Rain",
            [25] = "Summon",
            [26] = "Health",
            [27] = "Shock",
            [28] = "Possess",
            [29] = "Magitek",
            [255] = "Empty"
        };

        /// <summary>
        /// Dictionary holding the byte value for each letter and symbol.
        /// </summary>
        public static readonly Dictionary<byte, string> CharDict = new()
        {
            { 0x80, "A" },
            { 0x81, "B" },
            { 0x82, "C" },
            { 0x83, "D" },
            { 0x84, "E" },
            { 0x85, "F" },
            { 0x86, "G" },
            { 0x87, "H" },
            { 0x88, "I" },
            { 0x89, "J" },
            { 0x8A, "K" },
            { 0x8B, "L" },
            { 0x8C, "M" },
            { 0x8D, "N" },
            { 0x8E, "O" },
            { 0x8F, "P" },
            { 0x90, "Q" },
            { 0x91, "R" },
            { 0x92, "S" },
            { 0x93, "T" },
            { 0x94, "U" },
            { 0x95, "V" },
            { 0x96, "W" },
            { 0x97, "X" },
            { 0x98, "Y" },
            { 0x99, "Z" },
            { 0x9A, "a" },
            { 0x9B, "b" },
            { 0x9C, "c" },
            { 0x9D, "d" },
            { 0x9E, "e" },
            { 0x9F, "f" },
            { 0xA0, "g" },
            { 0xA1, "h" },
            { 0xA2, "i" },
            { 0xA3, "j" },
            { 0xA4, "k" },
            { 0xA5, "l" },
            { 0xA6, "m" },
            { 0xA7, "n" },
            { 0xA8, "o" },
            { 0xA9, "p" },
            { 0xAA, "q" },
            { 0xAB, "r" },
            { 0xAC, "s" },
            { 0xAD, "t" },
            { 0xAE, "u" },
            { 0xAF, "v" },
            { 0xB0, "w" },
            { 0xB1, "x" },
            { 0xB2, "y" },
            { 0xB3, "z" },
            { 0xB4, "0" },
            { 0xB5, "1" },
            { 0xB6, "2" },
            { 0xB7, "3" },
            { 0xB8, "4" },
            { 0xB9, "5" },
            { 0xBA, "6" },
            { 0xBB, "7" },
            { 0xBC, "8" },
            { 0xBD, "9" },
            { 0xBE, "!" },
            { 0xBF, "?" },
            { 0xC0, "/" },
            { 0xC1, ":" },
            { 0xC2, "“" },
            { 0xC3, "'" },
            { 0xC4, "-" },
            { 0xC5, "." },
            { 0xC6, "." },
            { 0xC8, ";" },
            { 0xC9, "#" },
            { 0xCA, "+" },
            { 0xCB, "(" },
            { 0xCC, ")" },
            { 0xCD, "%" },
            { 0xCE, "~" },
            { 0xCF, " " },
            { 0xD0, " " },
            { 0xD1, " " },
            { 0xD2, "=" }
        };
    }
}