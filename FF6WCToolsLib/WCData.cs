using System;
using System.Collections.Generic;

namespace FF6WCToolsLib;

/// <summary>
/// Holds data about Final Fantasy 6: Worlds Collide.
/// </summary>
public static class WCData
{
    // ROM data and addresses
    public const int MAP_NAMES_START = 0xCEF100; // 73 items, variable size
    
    public const uint SPELL_DATA_START = 0xC46AC0;
    public const byte SPELL_DATA_BLOCK_SIZE = 14;
    public const int SPELL_DATA_BLOCK_COUNT = 256;

    public const uint ITEM_DATA_START = 0xD85000;
    public const byte ITEM_DATA_BLOCK_SIZE = 30;
    public const int ITEM_DATA_BLOCK_COUNT = 256;
    
    public const uint ESPER_DATA_START = 0xD86E00;
    public const byte ESPER_DATA_BLOCK_SIZE = 0x0B;
    public const int ESPER_DATA_BLOCK_COUNT = 27;

    // Names are in a different section of memory
    public const uint ITEM_NAMES_START = 0xD2B300;
    /// <summary>
    /// Byte 1: icon, 2-13 name
    /// </summary>
    public const byte ITEM_NAMES_BLOCK_SIZE = 13;
    public const int ITEM_NAMES_BLOCK_COUNT = 256;
    
    // For spells, use the start address and add the offset for each section
    public const uint SPELLS_ALL_NAMES_START = 0xE6F567;
    public const uint SPELLS_ALL_NAMES_SIZE = 2710;
    
    public const uint SPELLS_MAGICAL_NAMES_OFFSET = 0;
    public const byte SPELLS_MAGICAL_NAMES_BLOCK_SIZE = 7; // Byte 1: icon, 2-7: name
    public const int SPELLS_MAGICAL_NAMES_BLOCK_COUNT = 54;

    public const uint SPELLS_ESPER_NAMES_OFFSET = 378;
    public const byte SPELLS_ESPER_NAMES_BLOCK_SIZE = 8;
    public const int SPELLS_ESPER_NAMES_BLOCK_COUNT = 27;

    public const uint SPELLS_ATTACK_NAMES_OFFSET = 594;
    public const byte SPELLS_ATTACK_NAMES_BLOCK_SIZE = 10;
    public const int SPELLS_ATTACK_NAMES_BLOCK_COUNT = 175;

    public const uint SPELLS_ESPER_ATTACK_NAMES_OFFSET = 2344;
    public const byte SPELLS_ESPER_ATTACK_NAMES_BLOCK_SIZE = 10;
    public const int SPELLS_ESPER_ATTACK_NAMES_BLOCK_COUNT = 27;

    // FF6Hacking rom map claims dance names data ends at 0xE6FFFF.
    // But at 0xE6FFFD to 0xE6FFFF there's 3 bytes of unknown data.
    public const uint SPELLS_DANCE_NAMES_OFFSET = 2614;
    public const byte SPELLS_DANCE_NAMES_BLOCK_SIZE = 12;
    public const int SPELLS_DANCE_NAMES_BLOCK_COUNT = 8;

    /// <summary>
    /// Low 4 bits contain the window background. High 4 bits are config settings.
    /// </summary>
    public const uint WALLPAPER = 0x7E1D4E;

    /// <summary>
    /// Holds the text color data. 2 bytes (3 colors, 5 bits each, values 0-31).
    /// </summary>
    public const uint FONT_COLOR = 0x7E1D55;

    /// <summary>
    /// Start of the window palettes. 8 palettes, 7 colors each.
    /// Each color combo is 2 bytes (3 colors, 5 bits each, values 0-31).
    /// </summary>
    public const uint WINDOW_COLOR_START = 0x7E1D57;
    public const byte WINDOW_COLOR_BLOCK_SIZE = 14;
    public const byte WINDOW_COLOR_BLOCK_COUNT = 8;

    public enum TzenThiefBought
    {
        None,
        Esper,
        Item
    }

    public enum TzenThiefPeekWob
    {
        Did_not_check,
        Esper,
        Item
    }

    public enum TzenThiefPeekWor
    {
        Did_not_check,
        Unknown
    }

    public enum ThiefPeek
    {
        Did_not_check,
        Checked_WOB_only,
        Checked_WOR_only,
        Checked_both
    }

    public enum ThiefReward
    {
        Did_not_buy__Item,
        Did_not_buy__Esper,
        Did_not_buy__Unknown,
        Bought_Item,
        Bought_Esper
    }

    public enum ColiseumVisit
    {
        Did_not_visit,
        Visited_but_did_not_fight,
        Visited_and_fought
    }

    public const int EVENT_BIT_OFFSET_WON_COLISEUM_MATCH = 0x1EF;
    public const int EVENT_BIT_OFFSET_KT_SKIP_UNLOCK = 0x093;
    public const int EVENT_BIT_OFFSET_KEFKA_TOWER_UNLOCK = 0x094;

    // Game mode constants.
    public const string BATTLE_KEY = "battle";
    public const string MENU_KEY = "menu";
    public const string WORLD_KEY = "world";
    public const string FIELD_KEY = "field";
    public const string MODE7_KEY = "mode7";
    
    /// <summary>
    /// Data used to determine game mode. Size: 3 bytes.
    /// </summary>
    public const uint NMI_JUMP_CODE = 0x7E1501;

    /// <summary>
    /// Start of the monster indexes.
    /// 6 monsters, 2 bytes each: 12 bytes total.
    /// </summary>
    public const uint MONSTER_INDEX_START = 0x7E2001;

    /// <summary>
    /// Size of the monster index data.
    /// </summary>
    public const byte MONSTER_INDEX_DATA_SIZE = 12;
    
    /// <summary>
    /// Start of the character skill data.
    /// </summary>
    public const uint CHARACTER_SKILL_DATA_START = 0x7E1A6E;
    
    /// <summary>
    /// Size of the character skill data.
    /// </summary>
    public const int CHARACTER_SKILL_DATA_SIZE = 734;
    public const int CHARACTER_SKILL_DATA_BLOCK_COUNT = 12;
    public const byte CHARACTER_SKILL_DATA_SPELL_BLOCK_SIZE = 54;

    public const int SWDTECH_OFFSET = 649;
    public const int BLITZ_OFFSET = 698;
    public const int LORE_OFFSET = 699; // 3 bytes

    /// <summary>
    /// Time interval to avoid false positives on map changes.
    /// </summary>
    public static readonly TimeSpan TIME_BETWEEN_MAP_CHANGES = new(0, 0, 0, 0, 500);
    
    /// <summary>
    /// Changes when a dialog box is opened.
    /// </summary>
    public const uint ENABLE_DIALOG_WINDOW = 0x7E00BA;
    
    /// <summary>
    /// Party X position on the field.
    /// </summary>
    public const uint PARTY_X_POSITION = 0x7E00AF;

    /// <summary>
    /// Party Y position on the field.
    /// </summary>
    public const uint PARTY_Y_POSITION = 0x7E00B0;

    /// <summary>
    /// Start of the character list selected right before the final battle.
    /// Character values range from 0x00 (Terra) to 0x0D (Umaro).
    /// </summary>
    public const uint FINAL_BATTLE_CHARACTER_LIST_START = 0x7E0205;

    /// <summary>
    /// Size of the final battle character list.
    /// </summary>
    public const byte FINAL_BATTLE_CHARACTER_LIST_SIZE = 12;
    
    /// <summary>
    /// State of the character overworld sprite.
    /// 1: flying the airship. 6-9: airship transitions.
    /// </summary>
    public const uint CHARACTER_1_GRAPHIC = 0x7E00CA;
    
    /// <summary>
    /// Current amount of GP. 3 bytes.
    /// </summary>
    public const uint CURRENT_GP_START = 0x7E1860;

    public const byte CURRENT_GP_SIZE = 3;
    
    /// <summary>
    /// Start of the known espers.
    /// </summary>
    public const uint KNOWN_ESPERS_START = 0x7E1A69;
    
    /// <summary>
    /// Size of the known espers data.
    /// </summary>
    public const byte KNOWN_ESPERS_SIZE = 4;
    
    /// <summary>
    /// Start of the known lores.
    /// </summary>
    public const uint KNOWN_LORES_START = 0x7E1D29;
    
    /// <summary>
    /// Size of the known lores data.
    /// </summary>
    public const byte KNOWN_LORES_SIZE = 3;
    
    /// <summary>
    /// Known SwdTechs.
    /// </summary>
    public const uint KNOWN_SWDTECHS = 0x7E1CF7;
    
    /// <summary>
    /// Known spells.
    /// 12 characters, 54 spells each, 1 byte per spell.
    /// </summary>
    public const uint KNOWN_SPELLS_DATA = 0x7E1A6E;

    public const uint KNOWN_SPELLS_SIZE = 648;

    /// <summary>
    /// Which choice is selected in a dialogue.
    /// </summary>
    public const uint DIALOG_CHOICE_SELECTED = 0x7E056E;

    /// <summary>
    /// Pointer to current tile in VRAM.
    /// Position of the dialog choice cursor.
    /// 2: WoB Tzen Thief is an item, 4: is an esper.
    /// </summary>
    public const uint DIALOG_POINTER = 0x7E00C4;

    /// <summary>
    /// Dialog index. Check alongside MapId to avoid false positives.
    /// </summary>
    public const uint DIALOG_INDEX_START = 0x7E00D0;
    
    public const byte DIALOG_INDEX_SIZE = 2;

    public const int DIALOG_INDEX_WOB_TZEN_THIEF = 1569;
    public const int DIALOG_INDEX_WOR_TZEN_THIEF = 1570;

    /// <summary>
    /// Changes depending if the dialog box is waiting for an input.
    /// 0: not waiting, 1: waiting for key press, 2: waiting for key release.
    /// 0x04 = Item, 0x06 = Esper.
    /// </summary>
    public const uint DIALOG_WAITING_FOR_INPUT = 0x7E00D3;

    /// <summary>
    /// Data for the map ID.
    /// Top 7 bits of the second byte have complementary information.
    /// </summary>
    public const uint MAP_ID_START = 0x7E1F64;
    
    /// <summary>
    /// Size of the map ID data.
    /// </summary>
    public const byte MAP_ID_SIZE = 2;

    /// <summary>
    /// Start of the event bits data.
    /// </summary>
    public const uint EVENT_BIT_START_ADDRESS = 0x7E1E80;

    /// <summary>
    /// Size of the event bits data.
    /// </summary>
    public const byte EVENT_BIT_DATA_SIZE = 223;

    /// <summary>
    /// Start of the field object data. 50 items, 41 bytes each.
    /// $00-$0F are characters, $10-$2F are NPC's, $30 is camera ($07B0), $31 is showing character or for unused objects ($07D9)
    /// </summary>
    public const uint FIELD_OBJECT_START_ADDRESS = 0x7E0867;

    /// <summary>
    /// Menu number address. Doesn't get cleared on menu close.
    /// 9 = pregame custom menu, 3 = shop, 2 = restore save game.
    /// </summary>
    public const uint MENU_NUMBER = 0x7E0200;

    /// <summary>
    /// Contains the "next menu state". Value changes depending on which menu is active.
    /// Values 19-22 are for the save menu ingame (NOT the load screen on reset).
    /// </summary>
    public const uint NEXT_MENU_STATE = 0x7E0027;
    
    /// <summary>
    /// Changes value from 0 to 1 on the new game screen, but also changes if you select a save game.
    /// Combined with MenuNumber, can be used to determine when a new game has been started.
    /// </summary>
    public const uint NEW_GAME_SELECTED = 0x7E0224;

    // Addresses for checking if Kefka has been killed.
    public const uint ENABLE_KEFKA_FINAL_ANIMATION = 0x7E009A; // 1 byte. Set to 01 when enabling Kefka's death animation.
    public const uint BATTLE_INDEX_START = 0x7E11E0; // Set to 0x0202 at tier 4. Shared between field, world and battle modules.
    public const byte BATTLE_INDEX_SIZE = 2;

    /// <summary>
    /// Counter that goes up by 1 when in a battle.
    /// Also gets used to control screen effects like flashes, Kefka lair animation, etc.
    /// Deprecated due to the NMI Jump Code game mode detection.
    /// </summary>
    public const uint BATTLE_COUNTER = 0x7E000E;

    /// <summary>
    /// contains a counter that only changes if the player is currently in a menu or shop.
    /// Size: 2 bytes.
    /// Deprecated due to the NMI Jump Code game mode detection.
    /// </summary>
    public const uint MENU_COUNTER = 0x7E00CF;

    /// <summary>
    /// Appears to control the brightness of the screen during menus. Goes from 1 to 15 in a menu.
    /// Is 0 when outside a menu.
    /// </summary>
    public const uint SCREEN_DISPLAY_REGISTER = 0x7E0044;

    // Counters.
    public const uint BOSS_COUNT = 0x7E1FF8;
    public const uint CHECK_COUNT = 0x7E1FCA;
    public const uint DRAGON_COUNT = 0x7E1FCE;
    public const uint ESPER_COUNT = 0x7E1FC8;

    /// <summary>
    /// Start of the chest data.
    /// </summary>
    public const uint CHEST_DATA_START = 0x7E1E40;

    /// <summary>
    /// Size of the chest data.
    /// </summary>
    public const byte CHEST_DATA_SIZE = 48;

    /// <summary>
    /// Start of the character data.
    /// From this address you can offset to get the rest of the data.
    /// Each character takes up 37 bytes.
    /// More info in the SRAM section:
    /// https://www.ff6hacking.com/wiki/doku.php?id=ff3:ff3us:doc:asm:ram:field_ram
    /// </summary>
    public const uint CHARACTER_DATA_START = 0x7E1600;
    
    /// <summary>
    /// Size of the memory area that contains the character data.
    /// 14 characters, 37 bytes each.
    /// There's memory allocated for 2 extra characters but I have no idea
    /// what that extra memory is used for. This value is for the 14 playable characters.
    /// </summary>
    public const int CHARACTER_DATA_SIZE = 518;

    public const byte CHARACTER_DATA_BLOCK_SIZE = 37;
    public const int CHARACTER_DATA_BLOCK_COUNT = 14;

    public const byte CHARACTER_DATA_NAME_OFFSET = 0x02;
    public const byte CHARACTER_DATA_NAME_SIZE = 6;
    public const byte CHARACTER_DATA_LEVEL_OFFSET = 0x08;

    /// <summary>
    /// Start of the inventory.
    /// </summary>
    public const uint INVENTORY_START = 0x7E1869;

    /// <summary>
    /// Start of the inventory item count.
    /// </summary>
    public const uint INVENTORY_COUNT_START = 0x7E1969;
    
    /// <summary>
    /// Size of the inventory (and inventory count).
    /// There's 1 "separator" Empty item after each of the blocks.
    /// </summary>
    public const byte INVENTORY_SIZE = 255;
    

    /// <summary>
    /// Data of which character has been found.
    /// Info is split through 2 bytes. Top 2 bits of second byte aren't used.
    /// </summary>
    public const uint CHARACTERS_AVAILABLE_START = 0x7E1EDE;

    /// <summary>
    /// Size of the characters found data.
    /// </summary>
    public const byte CHARACTERS_AVAILABLE_SIZE = 2;

    /// <summary>
    /// Contains the flags for each bit.
    /// </summary>
    public static readonly byte[] BIT_FLAGS = new byte[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

    /// <summary>
    /// Data of which dragon has been killed.
    /// Info is split through 2 bytes.
    /// </summary>
    public const uint DRAGONS_KILLED_DATA = 0x7E1EA3;

    // Dragon flags.
    public static readonly byte[] DRAGON_FLAGS_1 = new byte[] { 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
    public static readonly byte[] DRAGON_FLAGS_2 = new byte[] { 0x01, 0x02 };
    
    /// <summary>
    /// Ordered list of the characters for ease of indexing.
    /// </summary>
    public static readonly string[] CHARACTER_NAMES = new string[] {
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

    public enum Character : byte
    {
        Terra,
        Locke,
        Cyan,
        Shadow,
        Edgar,
        Sabin,
        Celes,
        Strago,
        Relm,
        Setzer,
        Mog,
        Gau,
        Gogo,
        Umaro
    }

    // These are time offsets for the JSON data.
    public static readonly TimeSpan TIME_FROM_KEFKA_FLASH_TO_ANIMATION = new(0, 0, 0, 4, 967); // 298 frames at 60FPS.
    public static readonly TimeSpan TIME_FROM_SWITCHES_TO_KEFKA_LAIR = new(0, 0, 0, 13, 50); // 783 frames at 60FPS.
    
    // These are used to avoid false positives with frame counters stopping and resuming too fast.
    public static readonly TimeSpan TIME_BATTLE_FALSE_POSITIVES = new(0, 0, 3);
    public static readonly TimeSpan TIME_BATTLE_FORMATION_FALSE_POSITIVES = new(0, 0, 2);

    /// <summary>
    /// Maps that are used to determine if the airship is being flown.
    /// </summary>
    public static readonly List<int> AIRSHIP_MAP_IDS = new List<int>() { 0x000, 0x001, 0x006, 0x00B, 0x00A, 0x011 };

    /// <summary>
    /// Overworld maps.
    /// </summary>
    public static readonly List<int> OVERWORLD_MAP_IDS = new List<int>() { 0x000, 0x001 };

    /// <summary>
    /// Airship deck maps.
    /// </summary>
    public static readonly List<int> AIRSHIP_DECK_MAP_IDS = new List<int>() { 0x006, 0x00B, 0x00A, 0x011 };

    /// <summary>
    /// Maps that are excluded in the airship check.
    /// </summary>
    public static readonly List<int> AIRSHIP_FALSE_POSITIVES = new List<int>() { 0x161, 0x0BB, 0x046, 0x195 };

    /// <summary>
    /// Bit offsets of every check event.
    /// </summary>
    public static readonly Dictionary<string, int> EVENT_BIT_OFFSETS_DICT = new Dictionary<string, int>()
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
    /// Names of each event bit.
    /// </summary>
    public static readonly Dictionary<string, string> EVENT_BIT_CHECK_NAMES_DICT = new Dictionary<string, string>()
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
    /// Bytes for each dragon.
    /// </summary>
    public static readonly Dictionary<byte, string> DRAGON_DICT = new Dictionary<byte, string>() {
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
    /// Map names for each map ID.
    /// </summary>
    public static readonly Dictionary<uint, string> MAPS_DICT = new Dictionary<uint, string>()
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
    /// Check peeks that are determined by map ID.
    /// </summary>
    public static readonly Dictionary<int, string> PEEKS_BY_MAP_ID = new Dictionary<int, string>()
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
        {0x116, "Zone Eater"},
        {0x11B, "Umaro's Cave"}
    };

    /// <summary>
    /// Check peeks that are determined by event bits.
    /// </summary>
    public static readonly Dictionary<int, string> PEEKS_BY_EVENT_BIT = new Dictionary<int, string>()
    {
        {0x27d, "Collapsing House"},
        {0x2b6, "Darill's Tomb"},
        {0x155, "Imperial Camp"},
        {0x0b0, "South Figaro Cave"},
        {0x195, "Veldt Cave"}
    };

    /// <summary>
    /// Names for each item ID.
    /// </summary>
    public static readonly Dictionary<byte, string> ITEM_DICT = new Dictionary<byte, string>()
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
    /// Index of each spell.
    /// </summary>
    public static readonly Dictionary<byte, string> SPELL_DICT = new Dictionary<byte, string>()
    {
        { 0, "Fire" },
        { 1, "Ice" },
        { 2, "Bolt" },
        { 3, "Poison" },
        { 4, "Drain" },
        { 5, "Fire2" },
        { 6, "Ice2" },
        { 7, "Bolt2" },
        { 8, "Bio" },
        { 9, "Fire3" },
        { 10, "Ice3" },
        { 11, "Bolt3" },
        { 12, "Break" },
        { 13, "Doom" },
        { 14, "Pearl" },
        { 15, "Flare" },
        { 16, "Demi" },
        { 17, "Quartr" },
        { 18, "X-Zone" },
        { 19, "Meteor" },
        { 20, "Ultima" },
        { 21, "Quake" },
        { 22, "WWind" },
        { 23, "Merton" },
        { 24, "Scan" },
        { 25, "Slow" },
        { 26, "Rasp" },
        { 27, "Mute" },
        { 28, "Safe" },
        { 29, "Sleep" },
        { 30, "Muddle" },
        { 31, "Haste" },
        { 32, "Stop" },
        { 33, "Bserk" },
        { 34, "Float" },
        { 35, "Imp" },
        { 36, "Rflect" },
        { 37, "Shell" },
        { 38, "Vanish" },
        { 39, "Haste2" },
        { 40, "Slow2" },
        { 41, "Osmose" },
        { 42, "Warp" },
        { 43, "Quick" },
        { 44, "Dispel" },
        { 45, "Cure" },
        { 46, "Cure2" },
        { 47, "Cure3" },
        { 48, "Life" },
        { 49, "Life2" },
        { 50, "Antdot" },
        { 51, "Remedy" },
        { 52, "Regen" },
        { 53, "Life3" },
        { 54, "Ramuh" },
        { 55, "Ifrit" },
        { 56, "Shiva" },
        { 57, "Siren" },
        { 58, "Terrato" },
        { 59, "Shoat" },
        { 60, "Maduin" },
        { 61, "Bismark" },
        { 62, "Stray" },
        { 63, "Palidor" },
        { 64, "Tritoch" },
        { 65, "Odin" },
        { 66, "Raiden" },
        { 67, "Bahamut" },
        { 68, "Alexandr" },
        { 69, "Crusader" },
        { 70, "Ragnarok" },
        { 71, "Kirin" },
        { 72, "ZoneSeek" },
        { 73, "Carbunkl" },
        { 74, "Phantom" },
        { 75, "Sraphim" },
        { 76, "Golem" },
        { 77, "Unicorn" },
        { 78, "Fenrir" },
        { 79, "Starlet" },
        { 80, "Phoenix" },
        { 81, "FireSkean" },
        { 82, "WaterEdge" },
        { 83, "BoltEdge" },
        { 84, "Storm" },
        { 85, "Dispatch" },
        { 86, "Retort" },
        { 87, "Slash" },
        { 88, "QuadraSlam" },
        { 89, "Empowerer" },
        { 90, "Stunner" },
        { 91, "QuadraSlice" },
        { 92, "Cleave" },
        { 93, "Pummel" },
        { 94, "AuraBolt" },
        { 95, "Suplex" },
        { 96, "FireDance" },
        { 97, "Mantra" },
        { 98, "AirBlade" },
        { 99, "Spiraler" },
        { 100, "BumRush" },
        { 101, "WindSlash" },
        { 102, "SunBath" },
        { 103, "Rage" },
        { 104, "Harvester" },
        { 105, "SandStorm" },
        { 106, "Antlion" },
        { 107, "ElfFire" },
        { 108, "Specter" },
        { 109, "LandSlide" },
        { 110, "SonicBoom" },
        { 111, "ElNino" },
        { 112, "Plasma" },
        { 113, "Snare" },
        { 114, "CaveIn" },
        { 115, "Snowball" },
        { 116, "Surge" },
        { 117, "Cokatrice" },
        { 118, "Wombat" },
        { 119, "Kitty" },
        { 120, "Tapir" },
        { 121, "Whump" },
        { 122, "WildBear" },
        { 123, "Pois.Frog" },
        { 124, "IceRabbit" },
        { 125, "SuperBall" },
        { 126, "Flash" },
        { 127, "Chocobop" },
        { 128, "H-Bomb" },
        { 129, "7-Flush" },
        { 130, "Megahit" },
        { 131, "FireBeam" },
        { 132, "BoltBeam" },
        { 133, "IceBeam" },
        { 134, "BioBlast" },
        { 135, "HealForce" },
        { 136, "Confuser" },
        { 137, "X-Fer" },
        { 138, "TekMissile" },
        { 139, "Condemned" },
        { 140, "Roulette" },
        { 141, "CleanSweep" },
        { 142, "AquaRake" },
        { 143, "Aero" },
        { 144, "BlowFish" },
        { 145, "BigGuard" },
        { 146, "Revenge" },
        { 147, "PearlWind" },
        { 148, "L.5Doom" },
        { 149, "L.4Flare" },
        { 150, "L.3Muddle" },
        { 151, "Reflect???" },
        { 152, "L.?Pearl" },
        { 153, "StepMine" },
        { 154, "ForceField" },
        { 155, "Dischord" },
        { 156, "SourMouth" },
        { 157, "PepUp" },
        { 158, "Rippler" },
        { 159, "Stone" },
        { 160, "Quasar" },
        { 161, "GrandTrain" },
        { 162, "Exploder" },
        { 163, "ImpSong" },
        { 164, "Clear" },
        { 165, "Virite" },
        { 166, "ChokeSmoke" },
        { 167, "Schiller" },
        { 168, "Lullaby" },
        { 169, "AcidRain" },
        { 170, "Confusion" },
        { 171, "Megazerk" },
        { 172, "Mute" },
        { 173, "Net" },
        { 174, "Slimer" },
        { 175, "DeltaHit" },
        { 176, "Entwine" },
        { 177, "Blaster" },
        { 178, "Cyclonic" },
        { 179, "FireBall" },
        { 180, "AtomicRay" },
        { 181, "TekLaser" },
        { 182, "Diffuser" },
        { 183, "WaveCannon" },
        { 184, "MegaVolt" },
        { 185, "GigaVolt" },
        { 186, "Blizzard" },
        { 187, "Absolute0" },
        { 188, "Magnitude8" },
        { 189, "Raid" },
        { 190, "FlashRain" },
        { 191, "TekBarrier" },
        { 192, "FallenOne" },
        { 193, "WallChange" },
        { 194, "Escape" },
        { 195, "50Gs" },
        { 196, "MindBlast" },
        { 197, "N.Cross" },
        { 198, "FlareStar" },
        { 199, "LoveToken" },
        { 200, "Seize" },
        { 201, "R.Polarity" },
        { 202, "Targetting" },
        { 203, "Sneeze" },
        { 204, "S.Cross" },
        { 205, "Launcher" },
        { 206, "Charm" },
        { 207, "ColdDust" },
        { 208, "Tentacle" },
        { 209, "HyperDrive" },
        { 210, "Train" },
        { 211, "EvilToot" },
        { 212, "GravBomb" },
        { 213, "Engulf" },
        { 214, "Disaster" },
        { 215, "Shrapnel" },
        { 216, "Bomblet" },
        { 217, "HeartBurn" },
        { 218, "Zinger" },
        { 219, "Discard" },
        { 220, "Overcast" },
        { 221, "Missile" },
        { 222, "Goner" },
        { 223, "Meteo" },
        { 224, "Revenger" },
        { 225, "Phantasm" },
        { 226, "Dread" },
        { 227, "ShockWave" },
        { 228, "Blaze" },
        { 229, "SoulOut" },
        { 230, "GaleCut" },
        { 231, "Shimsham" },
        { 232, "LodeStone" },
        { 233, "ScarBeam" },
        { 234, "BabaBreath" },
        { 235, "LifeShaver" },
        { 236, "FireWall" },
        { 237, "Slide" },
        { 238, "Battle" },
        { 239, "Special" },
        { 240, "RiotBlade" },
        { 241, "Mirager" },
        { 242, "BackBlade" },
        { 243, "ShadowFang" },
        { 244, "RoyalShock" },
        { 245, "TigerBreak" },
        { 246, "SpinEdge" },
        { 247, "SabreSoul" },
        { 248, "StarPrism" },
        { 249, "RedCard" },
        { 250, "MoogleRush" },
        { 251, "X-Meteo" },
        { 252, "Takedown" },
        { 253, "WildFang" },
        { 254, "Lagomorph" },
        { 255, "(Nothing)" }
    };

    /// <summary>
    /// Index of each SwdTech.
    /// </summary>
    public static readonly Dictionary<byte, string> SWDTECH_DICT = new Dictionary<byte, string>()
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
    /// Index of each Blitz.
    /// </summary>
    public static readonly Dictionary<byte, string> BLITZ_DICT = new Dictionary<byte, string>()
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
    /// Index of each Lore.
    /// </summary>
    public static readonly Dictionary<byte, string> LORE_DICT = new Dictionary<byte, string>()
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
    /// Name for each esper byte.
    /// </summary>
    public static readonly Dictionary<byte, string> ESPER_DICT = new Dictionary<byte, string>()
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
    /// Byte value for each command in memory.
    /// </summary>
    public static readonly Dictionary<byte, string> COMMAND_DICT = new Dictionary<byte, string>()
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
        [29] = "MagiTek",
        [255] = "Empty"
    };

    /// <summary>
    /// Byte value for each letter and symbol.
    /// </summary>
    public static readonly Dictionary<byte, char> CHAR_DICT = new Dictionary<byte, char>()
    {
        { 0x80, 'A' },
        { 0x81, 'B' },
        { 0x82, 'C' },
        { 0x83, 'D' },
        { 0x84, 'E' },
        { 0x85, 'F' },
        { 0x86, 'G' },
        { 0x87, 'H' },
        { 0x88, 'I' },
        { 0x89, 'J' },
        { 0x8A, 'K' },
        { 0x8B, 'L' },
        { 0x8C, 'M' },
        { 0x8D, 'N' },
        { 0x8E, 'O' },
        { 0x8F, 'P' },
        { 0x90, 'Q' },
        { 0x91, 'R' },
        { 0x92, 'S' },
        { 0x93, 'T' },
        { 0x94, 'U' },
        { 0x95, 'V' },
        { 0x96, 'W' },
        { 0x97, 'X' },
        { 0x98, 'Y' },
        { 0x99, 'Z' },
        { 0x9A, 'a' },
        { 0x9B, 'b' },
        { 0x9C, 'c' },
        { 0x9D, 'd' },
        { 0x9E, 'e' },
        { 0x9F, 'f' },
        { 0xA0, 'g' },
        { 0xA1, 'h' },
        { 0xA2, 'i' },
        { 0xA3, 'j' },
        { 0xA4, 'k' },
        { 0xA5, 'l' },
        { 0xA6, 'm' },
        { 0xA7, 'n' },
        { 0xA8, 'o' },
        { 0xA9, 'p' },
        { 0xAA, 'q' },
        { 0xAB, 'r' },
        { 0xAC, 's' },
        { 0xAD, 't' },
        { 0xAE, 'u' },
        { 0xAF, 'v' },
        { 0xB0, 'w' },
        { 0xB1, 'x' },
        { 0xB2, 'y' },
        { 0xB3, 'z' },
        { 0xB4, '0' },
        { 0xB5, '1' },
        { 0xB6, '2' },
        { 0xB7, '3' },
        { 0xB8, '4' },
        { 0xB9, '5' },
        { 0xBA, '6' },
        { 0xBB, '7' },
        { 0xBC, '8' },
        { 0xBD, '9' },
        { 0xBE, '!' },
        { 0xBF, '?' },
        { 0xC0, '/' },
        { 0xC1, ':' },
        { 0xC2, '“' },
        { 0xC3, '\'' },
        { 0xC4, '-' },
        { 0xC5, '.' },
        { 0xC6, '.' },
        { 0xC8, ';' },
        { 0xC9, '#' },
        { 0xCA, '+' },
        { 0xCB, '(' },
        { 0xCC, ')' },
        { 0xCD, '%' },
        { 0xCE, '~' },
        { 0xCF, ' ' },
        { 0xD0, ' ' },
        { 0xD1, ' ' },
        { 0xD2, '=' },
        // ITEM ICONS ARE HERE
        { 0xDF, ' ' }, // Space used for map names
        { 0xFE, ' ' },
        { 0xFF, ' ' }
    };

    public static readonly Dictionary<char, byte> CHAR_TO_BYTE_DICT = new Dictionary<char, byte>();

    /// <summary>
    /// Monster index dictionary.
    /// </summary>
    public static readonly Dictionary<int, string> MONSTER_DICT = new Dictionary<int, string>()
    {
        {0, "Guard"},
        {1, "Soldier"},
        {2, "Templar"},
        {3, "Ninja"},
        {4, "Samurai"},
        {5, "Orog"},
        {6, "Mag Roader"},
        {7, "Retainer"},
        {8, "Hazer"},
        {9, "Dahling"},
        {10, "Rain Man"},
        {11, "Brawler"},
        {12, "Apokryphos"},
        {13, "Dark Force"},
        {14, "Whisper"},
        {15, "Over-Mind"},
        {16, "Osteosaur"},
        {17, "Commander"},
        {18, "Rhodox"},
        {19, "Were-Rat"},
        {20, "Ursus"},
        {21, "Rhinotaur"},
        {22, "Steroidite"},
        {23, "Leafer"},
        {24, "Stray Cat"},
        {25, "Lobo"},
        {26, "Doberman"},
        {27, "Vomammoth"},
        {28, "Fidor"},
        {29, "Baskervor"},
        {30, "Suriander"},
        {31, "Chimera"},
        {32, "Behemoth"},
        {33, "Mesosaur"},
        {34, "Pterodon"},
        {35, "FossilFang"},
        {36, "White Drgn"},
        {37, "Doom Drgn"},
        {38, "Brachosaur"},
        {39, "Tyranosaur"},
        {40, "Dark Wind"},
        {41, "Beakor"},
        {42, "Vulture"},
        {43, "Harpy"},
        {44, "HermitCrab"},
        {45, "Trapper"},
        {46, "Hornet"},
        {47, "CrassHoppr"},
        {48, "Delta Bug"},
        {49, "Gilomantis"},
        {50, "Trilium"},
        {51, "Nightshade"},
        {52, "TumbleWeed"},
        {53, "Bloompire"},
        {54, "Trilobiter"},
        {55, "Siegfried"},
        {56, "Nautiloid"},
        {57, "Exocite"},
        {58, "Anguiform"},
        {59, "Reach Frog"},
        {60, "Lizard"},
        {61, "ChickenLip"},
        {62, "Hoover"},
        {63, "Rider"},
        {64, "Chupon"},
        {65, "Pipsqueak"},
        {66, "M-TekArmor"},
        {67, "Sky Armor"},
        {68, "Telstar"},
        {69, "Lethal Wpn"},
        {70, "Vaporite"},
        {71, "Flan"},
        {72, "Ing"},
        {73, "Humpty"},
        {74, "Brainpan"},
        {75, "Cruller"},
        {76, "Cactrot"},
        {77, "Repo Man"},
        {78, "Harvester"},
        {79, "Bomb"},
        {80, "Still Life"},
        {81, "Boxed Set"},
        {82, "SlamDancer"},
        {83, "HadesGigas"},
        {84, "Pug"},
        {85, "Magic Urn"},
        {86, "Mover"},
        {87, "Figaliz"},
        {88, "Buffalax"},
        {89, "Aspik"},
        {90, "Ghost"},
        {91, "Crawler"},
        {92, "Sand Ray"},
        {93, "Areneid"},
        {94, "Actaneon"},
        {95, "Sand Horse"},
        {96, "Dark Side"},
        {97, "Mad Oscar"},
        {98, "Crawly"},
        {99, "Bleary"},
        {100, "Marshal"},
        {101, "Trooper"},
        {102, "General"},
        {103, "Covert"},
        {104, "Ogor"},
        {105, "Warlock"},
        {106, "Madam"},
        {107, "Joker"},
        {108, "Iron Fist"},
        {109, "Goblin"},
        {110, "Apparite"},
        {111, "PowerDemon"},
        {112, "Displayer"},
        {113, "Vector Pup"},
        {114, "Peepers"},
        {115, "Sewer Rat"},
        {116, "Slatter"},
        {117, "Rhinox"},
        {118, "Rhobite"},
        {119, "Wild Cat"},
        {120, "Red Fang"},
        {121, "Bounty Man"},
        {122, "Tusker"},
        {123, "Ralph"},
        {124, "Chitonid"},
        {125, "Wart Puck"},
        {126, "Rhyos"},
        {127, "SrBehemoth"},
        {128, "Vectaur"},
        {129, "Wyvern"},
        {130, "Zombone"},
        {131, "Dragon"},
        {132, "Brontaur"},
        {133, "Allosaurus"},
        {134, "Cirpius"},
        {135, "Sprinter"},
        {136, "Gobbler"},
        {137, "Harpiai"},
        {138, "GloomShell"},
        {139, "Drop"},
        {140, "Mind Candy"},
        {141, "WeedFeeder"},
        {142, "Luridan"},
        {143, "Toe Cutter"},
        {144, "Over Grunk"},
        {145, "Exoray"},
        {146, "Crusher"},
        {147, "Uroburos"},
        {148, "Primordite"},
        {149, "Sky Cap"},
        {150, "Cephaler"},
        {151, "Maliga"},
        {152, "Gigan Toad"},
        {153, "Geckorex"},
        {154, "Cluck"},
        {155, "Land Worm"},
        {156, "Test Rider"},
        {157, "PlutoArmor"},
        {158, "Tomb Thumb"},
        {159, "HeavyArmor"},
        {160, "Chaser"},
        {161, "Scullion"},
        {162, "Poplium"},
        {163, "Intangir"},
        {164, "Misfit"},
        {165, "Eland"},
        {166, "Enuo"},
        {167, "Deep Eye"},
        {168, "GreaseMonk"},
        {169, "NeckHunter"},
        {170, "Grenade"},
        {171, "Critic"},
        {172, "Pan Dora"},
        {173, "SoulDancer"},
        {174, "Gigantos"},
        {175, "Mag Roader"},
        {176, "Spek Tor"},
        {177, "Parasite"},
        {178, "EarthGuard"},
        {179, "Coelecite"},
        {180, "Anemone"},
        {181, "Hipocampus"},
        {182, "Spectre"},
        {183, "Evil Oscar"},
        {184, "Slurm"},
        {185, "Latimeria"},
        {186, "StillGoing"},
        {187, "Allo Ver"},
        {188, "Phase"},
        {189, "Outsider"},
        {190, "Barb-e"},
        {191, "Parasoul"},
        {192, "Pm Stalker"},
        {193, "Hemophyte"},
        {194, "Sp Forces"},
        {195, "Nohrabbit"},
        {196, "Wizard"},
        {197, "Scrapper"},
        {198, "Ceritops"},
        {199, "Commando"},
        {200, "Opinicus"},
        {201, "Poppers"},
        {202, "Lunaris"},
        {203, "Garm"},
        {204, "Vindr"},
        {205, "Kiwok"},
        {206, "Nastidon"},
        {207, "Rinn"},
        {208, "Insecare"},
        {209, "Vermin"},
        {210, "Mantodea"},
        {211, "Bogy"},
        {212, "Prussian"},
        {213, "Black Drgn"},
        {214, "Adamanchyt"},
        {215, "Dante"},
        {216, "Wirey Drgn"},
        {217, "Dueller"},
        {218, "Psychot"},
        {219, "Muus"},
        {220, "Karkass"},
        {221, "Punisher"},
        {222, "Balloon"},
        {223, "Gabbldegak"},
        {224, "GtBehemoth"},
        {225, "Scorpion"},
        {226, "Chaos Drgn"},
        {227, "Spit Fire"},
        {228, "Vectagoyle"},
        {229, "Lich"},
        {230, "Osprey"},
        {231, "Mag Roader"},
        {232, "Bug"},
        {233, "Sea Flower"},
        {234, "Fortis"},
        {235, "Abolisher"},
        {236, "Aquila"},
        {237, "Junk"},
        {238, "Mandrake"},
        {239, "1st Class"},
        {240, "Tap Dancer"},
        {241, "Necromancr"},
        {242, "Borras"},
        {243, "Mag Roader"},
        {244, "Wild Rat"},
        {245, "Gold Bear"},
        {246, "Innoc"},
        {247, "Trixter"},
        {248, "Red Wolf"},
        {249, "Didalos"},
        {250, "Woolly"},
        {251, "Veteran"},
        {252, "Sky Base"},
        {253, "IronHitman"},
        {254, "Io"},
        {255, "Pugs"},
        {256, "Whelk"},
        {257, "Presenter"},
        {258, "Mega Armor"},
        {259, "Vargas"},
        {260, "TunnelArmr"},
        {261, "Prometheus"},
        {262, "GhostTrain"},
        {263, "Dadaluma"},
        {264, "Shiva"},
        {265, "Ifrit"},
        {266, "Number 024"},
        {267, "Number 128"},
        {268, "Inferno"},
        {269, "Crane"},
        {270, "Crane"},
        {271, "Umaro"},
        {272, "Umaro"},
        {273, "Guardian (Vector)"},
        {274, "Guardian (Kefka's Tower)"},
        {275, "Air Force"},
        {276, "Tritoch (Intro)"},
        {277, "Tritoch (Terra Transforms)"},
        {278, "FlameEater"},
        {279, "AtmaWeapon"},
        {280, "Nerapa"},
        {281, "SrBehemoth"},
        {282, "Final Battle Script (Unused)"},
        {283, "Tentacle"},
        {284, "Dullahan"},
        {285, "Doom Gaze"},
        {286, "Chadarnook"},
        {287, "Curley"},
        {288, "Larry"},
        {289, "Moe"},
        {290, "Wrexsoul"},
        {291, "Hidon"},
        {292, "KatanaSoul"},
        {293, "L.30 Magic"},
        {294, "Hidonite"},
        {295, "Doom"},
        {296, "Goddess"},
        {297, "Poltrgeist"},
        {298, "Kefka"},
        {299, "L.40 Magic"},
        {300, "Ultros (Lete River)"},
        {301, "Ultros (Opera House)"},
        {302, "Ultros (Esper Mountain)"},
        {303, "Chupon"},
        {304, "L.20 Magic"},
        {305, "Siegfried"},
        {306, "L.10 Magic"},
        {307, "L.50 Magic"},
        {308, "Head"},
        {309, "Whelk Head"},
        {310, "Colossus"},
        {311, "CzarDragon"},
        {312, "Master Pug"},
        {313, "L.60 Magic"},
        {314, "Merchant"},
        {315, "B.Day Suit"},
        {316, "Tentacle"},
        {317, "Tentacle"},
        {318, "Tentacle"},
        {319, "RightBlade"},
        {320, "Left Blade"},
        {321, "Rough"},
        {322, "Striker"},
        {323, "L.70 Magic"},
        {324, "Tritoch (WoR)"},
        {325, "Laser Gun"},
        {326, "Speck"},
        {327, "MissileBay"},
        {328, "Chadarnook"},
        {329, "Ice Dragon"},
        {330, "Kefka (Hills Maze)"},
        {331, "Storm Drgn"},
        {332, "Dirt Drgn"},
        {333, "Ipooh"},
        {334, "Leader"},
        {335, "Grunt"},
        {336, "Gold Drgn"},
        {337, "Skull Drgn"},
        {338, "Blue Drgn"},
        {339, "Red Dragon"},
        {340, "Piranha"},
        {341, "Rizopas"},
        {342, "Specter"},
        {343, "Short Arm"},
        {344, "Long Arm"},
        {345, "Face"},
        {346, "Tiger"},
        {347, "Tools"},
        {348, "Magic"},
        {349, "Hit"},
        {350, "Girl"},
        {351, "Sleep"},
        {352, "Hidonite"},
        {353, "Hidonite"},
        {354, "Hidonite"},
        {355, "L.80 Magic"},
        {356, "L.90 Magic"},
        {357, "ProtoArmor"},
        {358, "MagiMaster"},
        {359, "SoulSaver"},
        {360, "Ultros (Airship)"},
        {361, "Naughty"},
        {362, "Phunbaba"},
        {363, "Phunbaba"},
        {364, "Phunbaba"},
        {365, "Phunbaba"},
        {366, "Terra's Flashback"},
        {367, "Kefka (Imperial Camp)"},
        {368, "Cyan (Imperial Camp)"},
        {369, "Zone Eater"},
        {370, "Gau (Veldt)"},
        {371, "Kefka vs. Leo"},
        {372, "Kefka (Esper Gate)"},
        {373, "Officer"},
        {374, "Cadet"},
        {375, "&nbsp;"},
        {376, "&nbsp;"},
        {377, "Soldier"},
        {378, "Kefka vs. Esper"},
        {379, "Battle Event"},
        {380, "&nbsp;"},
        {381, "Atma"},
        {382, "Shadow (Colosseum)"},
        {383, "Colosseum"}
    };

    public enum Item : byte
    {
        Dirk,
        MithrilKnife,
        Guardian,
        AirLancet,
        ThiefKnife,
        Assassin,
        ManEater,
        SwordBreaker,
        Graedus,
        ValiantKnife,
        MithrilBlade,
        RegalCutlass,
        RuneEdge,
        FlameSabre,
        Blizzard,
        ThunderBlade,
        Epee,
        BreakBlade,
        Drainer,
        Enhancer,
        Crystal,
        Falchion,
        SoulSabre,
        OgreNix,
        Excalibur,
        Scimitar,
        Illumina,
        Ragnarok,
        AtmaWeapon,
        MithrilPike,
        Trident,
        StoutSpear,
        Partisan,
        PearlLance,
        GoldLance,
        AuraLance,
        ImpHalberd,
        Imperial,
        Kodachi,
        Blossom,
        Hardened,
        Striker,
        Stunner,
        Ashura,
        Kotetsu,
        Forged,
        Tempest,
        Murasame,
        Aura,
        Strato,
        SkyRender,
        HealRod,
        MithrilRod,
        FireRod,
        IceRod,
        ThunderRod,
        PoisonRod,
        PearlRod,
        GravityRod,
        Punisher,
        MagusRod,
        ChocoboBrsh,
        DaVinciBrsh,
        MagicalBrsh,
        RainbowBrsh,
        Shuriken,
        NinjaStar,
        TackStar,
        Flail,
        FullMoon,
        MorningStar,
        Boomerang,
        RisingSun,
        HawkEye,
        BoneClub,
        Sniper,
        WingEdge,
        Cards,
        Darts,
        DoomDarts,
        Trump,
        Dice,
        FixedDice,
        MetalKnuckle,
        MithrilClaw,
        Kaiser,
        PoisonClaw,
        FireKnuckle,
        DragonClaw,
        TigerFangs,
        Buckler,
        HeavyShld,
        MithrilShld,
        GoldShld,
        AegisShld,
        DiamondShld,
        FlameShld,
        IceShld,
        ThunderShld,
        CrystalShld,
        GenjiShld,
        TortoiseShld,
        CursedShld,
        PaladinShld,
        ForceShld,
        LeatherHat,
        HairBand,
        PlumedHat,
        Beret,
        MagusHat,
        Bandana,
        IronHelmet,
        Coronet,
        BardsHat,
        GreenBeret,
        HeadBand,
        MithrilHelm,
        Tiara,
        GoldHelmet,
        TigerMask,
        RedCap,
        MysteryVeil,
        Circlet,
        RegalCrown,
        DiamondHelm,
        DarkHood,
        CrystalHelm,
        OathVeil,
        CatHood,
        GenjiHelmet,
        Thornlet,
        Titanium,
        LeatherArmor,
        CottonRobe,
        KungFuSuit,
        IronArmor,
        SilkRobe,
        MithrilVest,
        NinjaGear,
        WhiteDress,
        MithrilMail,
        GaiaGear,
        MirageVest,
        GoldArmor,
        PowerSash,
        LightRobe,
        DiamondVest,
        RedJacket,
        ForceArmor,
        DiamondArmor,
        DarkGear,
        TaoRobe,
        CrystalMail,
        CzarinaGown,
        GenjiArmor,
        ImpsArmor,
        Minerva,
        TabbySuit,
        ChocoboSuit,
        MoogleSuit,
        NutkinSuit,
        BehemothSuit,
        SnowMuffler,
        NoiseBlaster,
        BioBlaster,
        Flash,
        ChainSaw,
        Debilitator,
        Drill,
        AirAnchor,
        AutoCrossbow,
        FireSkean,
        WaterEdge,
        BoltEdge,
        InvizEdge,
        ShadowEdge,
        Goggles,
        StarPendant,
        PeaceRing,
        Amulet,
        WhiteCape,
        JewelRing,
        FairyRing,
        BarrierRing,
        MithrilGlove,
        GuardRing,
        RunningShoes,
        WallRing,
        CherubDown,
        CureRing,
        TrueKnight,
        DragoonBoots,
        ZephyrCape,
        CzarinaRing,
        CursedRing,
        Earrings,
        AtlasArmlet,
        BlizzardOrb,
        RageRing,
        SneakRing,
        PodBracelet,
        HeroRing,
        Ribbon,
        MuscleBelt,
        CrystalOrb,
        GoldHairpin,
        Economizer,
        ThiefGlove,
        Gauntlet,
        GenjiGlove,
        HyperWrist,
        Offering,
        Beads,
        BlackBelt,
        CoinToss,
        FakeMustache,
        GemBox,
        DragonHorn,
        MeritAward,
        MementoRing,
        SafetyBit,
        RelicRing,
        MoogleCharm,
        CharmBangle,
        MarvelShoes,
        BackGuard,
        GaleHairpin,
        SniperSight,
        ExpEgg,
        Tintinabar,
        SprintShoes,
        RenameCard,
        Tonic,
        Potion,
        XPotion,
        Tincture,
        Ether,
        XEther,
        Elixir,
        Megalixir,
        FenixDown,
        Revivify,
        Antidote,
        Eyedrop,
        Soft,
        Remedy,
        SleepingBag,
        Tent,
        GreenCherry,
        Magicite,
        SuperBall,
        EchoScreen,
        SmokeBomb,
        WarpStone,
        DriedMeat,
        Empty
    }

    // Item ranges
    public static readonly Range RANGE_WEAPONS = new(0, 89);
    public static readonly Range RANGE_SHIELDS = new(90, 104);
    public static readonly Range RANGE_HELMETS = new(105, 131);
    public static readonly Range RANGE_ARMORS = new(132, 162);
    public static readonly Range RANGE_TOOLS = new(163, 170);
    public static readonly Range RANGE_SKEANS = new(171, 175);
    public static readonly Range RANGE_RELICS = new(176, 230);
    public static readonly Range RANGE_CONSUMABLES = new(231, 254);
    public static readonly Range RANGE_NINJASTARS = new(65, 67);
    /// <summary>
    /// All non-relic equippables.
    /// </summary>
    public static readonly Range RANGE_GEAR = new(0, 162);

    public enum Esper : byte
    {
        Ramuh,
        Ifrit,
        Shiva,
        Siren,
        Terrato,
        Shoat,
        Maduin,
        Bismark,
        Stray,
        Palidor,
        Tritoch,
        Odin,
        Raiden,
        Bahamut,
        Alexandr,
        Crusader,
        Ragnarok,
        Kirin,
        ZoneSeek,
        Carbunkl,
        Phantom,
        Sraphim,
        Golem,
        Unicorn,
        Fenrir,
        Starlet,
        Phoenix
    }

    public enum Spell : byte
    {
        Fire,
        Ice,
        Bolt,
        Poison,
        Drain,
        Fire2,
        Ice2,
        Bolt2,
        Bio,
        Fire3,
        Ice3,
        Bolt3,
        Break,
        Doom,
        Pearl,
        Flare,
        Demi,
        Quartr,
        XZone,
        Meteor,
        Ultima,
        Quake,
        WWind,
        Merton,
        Scan,
        Slow,
        Rasp,
        Mute,
        Safe,
        Sleep,
        Muddle,
        Haste,
        Stop,
        Bserk,
        Float,
        Imp,
        Rflect,
        Shell,
        Vanish,
        Haste2,
        Slow2,
        Osmose,
        Warp,
        Quick,
        Dispel,
        Cure,
        Cure2,
        Cure3,
        Life,
        Life2,
        Antdot,
        Remedy,
        Regen,
        Life3,
        Ramuh,
        Ifrit,
        Shiva,
        Siren,
        Terrato,
        Shoat,
        Maduin,
        Bismark,
        Stray,
        Palidor,
        Tritoch,
        Odin,
        Raiden,
        Bahamut,
        Alexandr,
        Crusader,
        Ragnarok,
        Kirin,
        ZoneSeek,
        Carbunkl,
        Phantom,
        Sraphim,
        Golem,
        Unicorn,
        Fenrir,
        Starlet,
        Phoenix,
        FireSkean,
        WaterEdge,
        BoltEdge,
        Storm,
        Dispatch,
        Retort,
        Slash,
        QuadraSlam,
        Empowerer,
        Stunner,
        QuadraSlice,
        Cleave,
        Pummel,
        AuraBolt,
        Suplex,
        FireDance,
        Mantra,
        AirBlade,
        Spiraler,
        BumRush,
        WindSlash,
        SunBath,
        Rage,
        Harvester,
        SandStorm,
        Antlion,
        ElfFire,
        Specter,
        LandSlide,
        SonicBoom,
        ElNino,
        Plasma,
        Snare,
        CaveIn,
        Snowball,
        Surge,
        Cokatrice,
        Wombat,
        Kitty,
        Tapir,
        Whump,
        WildBear,
        PoisFrog,
        IceRabbit,
        SuperBall,
        Flash,
        Chocobop,
        HBomb,
        SevenFlush,
        Megahit,
        FireBeam,
        BoltBeam,
        IceBeam,
        BioBlast,
        HealForce,
        Confuser,
        XFer,
        TekMissile,
        Condemned,
        Roulette,
        CleanSweep,
        AquaRake,
        Aero,
        BlowFish,
        BigGuard,
        Revenge,
        PearlWind,
        L5Doom,
        L4Flare,
        L3Muddle,
        Reflect,
        LPearl,
        StepMine,
        ForceField,
        Dischord,
        SourMouth,
        PepUp,
        Rippler,
        Stone,
        Quasar,
        GrandTrain,
        Exploder,
        ImpSong,
        Clear,
        Virite,
        ChokeSmoke,
        Schiller,
        Lullaby,
        AcidRain,
        Confusion,
        Megazerk,
        Mute_Duplicate,
        Net,
        Slimer,
        DeltaHit,
        Entwine,
        Blaster,
        Cyclonic,
        FireBall,
        AtomicRay,
        TekLaser,
        Diffuser,
        WaveCannon,
        MegaVolt,
        GigaVolt,
        Blizzard,
        Absolute0,
        Magnitude8,
        Raid,
        FlashRain,
        TekBarrier,
        FallenOne,
        WallChange,
        Escape,
        FiftyGs,
        MindBlast,
        NCross,
        FlareStar,
        LoveToken,
        Seize,
        RPolarity,
        Targetting,
        Sneeze,
        SCross,
        Launcher,
        Charm,
        ColdDust,
        Tentacle,
        HyperDrive,
        Train,
        EvilToot,
        GravBomb,
        Engulf,
        Disaster,
        Shrapnel,
        Bomblet,
        HeartBurn,
        Zinger,
        Discard,
        Overcast,
        Missile,
        Goner,
        Meteo,
        Revenger,
        Phantasm,
        Dread,
        ShockWave,
        Blaze,
        SoulOut,
        GaleCut,
        Shimsham,
        LodeStone,
        ScarBeam,
        BabaBreath,
        LifeShaver,
        FireWall,
        Slide,
        Battle,
        Special,
        RiotBlade,
        Mirager,
        BackBlade,
        ShadowFang,
        RoyalShock,
        TigerBreak,
        SpinEdge,
        SabreSoul,
        StarPrism,
        RedCard,
        MoogleRush,
        XMeteo,
        Takedown,
        WildFang,
        Lagomorph,
        Nothing
    }

    public enum Stat : byte
    {
        vigor,
        speed,
        stamina,
        magic
    }

    public enum EquipmentSlot : byte
    {
        rhand,
        lhand,
        helmet,
        armor,
        relic1,
        relic2
    }

    /// <summary>
    /// Flagset dictionary.
    /// </summary>
    public static readonly Dictionary<string, string> FLAGSET_DICT = new Dictionary<string, string>()
    {
        { "Ultros League - Season 4", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.2.9.9.4.12.12 -oc 30.8.8.1.1.11.8 -od 59.1.1.11.31 -sc1 random -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -scis -com 98989898989898989898989898 -rec1 28 -rec2 27 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2 -hmced 2 -xgced 2 -ase 2 -msl 40 -sed -bbs -drloc shuffle -stloc mix -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -mmprp 75 125 -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -sebr -sesb -ccsr 20 -chrm 0 0 -cms -frw -wmhc -cor 100 -crr 100 -crvr 80 100 -crm -ari -anca -adeh -ame 1 -nmc -noshoes -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -fc -ond -rr -etn" },
        { "Ultros League - Season 5", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.2.9.9.4.12.12.10.21.21 -oc 30.8.8.1.1.11.8 -od 59.1.1.11.31 -sc1 random -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -scis -com 98989898989898989898989898 -rec1 28 -rec2 27 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2 -hmced 2 -xgced 2 -ase 2 -msl 40 -sed -bbs -drloc shuffle -stloc mix -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -elrt -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -mmprp 75 125 -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -ir stronger -csb 6 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -sebr -snsb -snee -snil -ccsr 20 -chrm 0 0 -cms -frw -wmhc -cor 100 -crr 100 -crvr 100 120 -crm -ari -anca -adeh -ame 1 -nmc -noshoes -u254 -nfps -fs -fe -fvd -fr -fj -fbs -fedc -fc -ond -etn" },
        { "Moogles First Tournament", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.2.9.9.4.12.12 -oc 58.1.1.12.4 -od 58.1.1.12.7 -oe 58.1.1.12.5.12.2 -sc1 random -sc2 random -sc3 random -sal -eu -csrp 100 125 -fst -brl -slr 5 8 -lmprp 75 125 -lel -srr 15 30 -rnl -sdr 1 1 -das -dda -dns -sch -com 98989898989898989898989898 -rec1 28 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2 -hmced 2 -xgced 2.5 -ase 2 -msl 40 -sed -bbs -be -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 1 5 -ebr 80 -emprp 75 125 -ems -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sws 3 -sfd 3 -sto 1 -ieor 33 -ieror 33 -csb 1 16 -mca -stra -saw -sisr 35 -sprp 75 125 -sdm 5 -npi -ccsr 35 -cms -cor -crr -crvr 75 128 -crm -ari -anca -adeh -nmc -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Terra", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 31.10.10.1.1.12.9 -oe 33.1.1.12.9 -of 40.2.2.11.26.11.51 -og 45.10.10.0.0 -sc1 terra -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 03989898989898989898989898 -rec1 28 -rec2 27 -xpm 3 -mpm 7 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esrt -ebr 82 -emprp 30 50 -ems -nm1 terra -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Locke", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 8.1.1.12.9 -oe 46.10.10.0.0 -of 48.5.5.0.0 -og 45.-5.-5.0.0 -sc1 locke -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sca -com 98059898989898989898989898 -rec1 28 -rec2 27 -rec3 29 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -smc 3 -sfd 10 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sirt -sprp 150 200 -ssf4 -sdm 5 -npi -snbr -ccsr 100 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Edgar", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 43.1.1.12.6 -oe 48.5.5.0.0 -of 45.5.5.0.0 -og 45.10.10.1.1.12.9 -oh 46.10.10.1.1.12.9 -oi 47.10.10.1.1.12.9 -oj 48.10.10.1.1.12.9 -ok 58.1.1.12.6 -sc1 edgar -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898099898989898989898 -rec1 28 -rec2 27 -rec3 29 -xpm 3 -mpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 3000 -smc 3 -sws 3 -sfd 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 100 -sprv 2000 2000 -ssf4 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -rc -as -etn" },
        { "Coliseum - Sabin", "-open -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 14.1.1.12.9 -oe 47.20.20.1.1.12.9 -of 38.1.1.12.7 -og 48.5.5.0.0 -oh 47.5.5.0.0 -oi 13.3.3.11.34.11.11.11.33 -sc1 sabin -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898981098989898989898 -rec1 28 -rec2 27 -rec3 29 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 0 2 -ebr 100 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -ssf4 -sdm 5 -npi -snbr -snsb -snee -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -cnee -ari -anca -adeh -nmc -nee -nu -nfps -nfce -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Cyan", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.8.8.1.1.11.8 -od 48.20.20.1.1.12.9 -oe 48.10.10.0.0 -of 47.5.5.0.0 -og 45.-5.-5.0.0 -sc1 cyan -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -com 98980798989898989898989898 -rec1 28 -rec2 27 -rec3 29 -rec4 9 -xpm 4 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -msl 50 -sed -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sfd 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -nfce -pd -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Gau", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 29.25.25.1.1.12.9 -oe 46.20.20.1.1.12.9 -of 37.1.1.11.48 -og 48.5.5.0.0 -oh 46.5.5.0.0 -sc1 gau -sc2 random -sc3 random -sal -sn -eu -csrp 50 175 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989898989898981697 -rec1 28 -rec2 27 -rec3 29 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -bmbd -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 1 3 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 4 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn -ycreature" },
        { "Coliseum - Celes", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 16.1.1.12.9 -oe 31.10.10.1.1.12.9 -of 45.5.5.0.0 -og 46.5.5.0.0 -oh 40.1.1.12.4 -oi 58.1.1.12.5 -sc1 celes -sc2 random -sc3 random -sal -eu -csrp 90 140 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989811989898989898 -rec1 28 -rec3 29 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 5 5 -ebr 33 -emprp 125 150 -eebr 7 -nm1 random -rnl1 -rns1 -nm2 celes -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -nfce -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn -yimperial" },
        { "Coliseum - Setzer", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 58.1.1.12.9 -oe 58.1.1.12.9 -of 58.1.1.12.9 -og 48.5.5.0.0 -oh 47.5.5.0.0 -sc1 setzer -sc2 random -sc3 random -sal -eu -csrp 65 140 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 99999999999999999915999999 -rec1 28 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -asr 2 -msl 50 -sed -bbr -be -bnu -rer 0 -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 0 5 -ebr 82 -emprv 1 128 -eer 6 12 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sirt -sprv 0 65535 -sdm 5 -npi -snbr -snsb -ccrs -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Strago", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 35.1.1.12.9 -oe 45.10.10.0.0 -of 28.24.24.2.2.11.14.11.10 -sc1 strago -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 6 12 -lmprp 50 100 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989898129898989898 -rec1 28 -rec3 29 -rec4 23 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 1 2 -ebr 82 -emprp 50 100 -ems -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 40 -sprp 75 125 -sdm 5 -npi -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" },
        { "Coliseum - Relm", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 45.15.15.1.1.12.9 -oe 45.5.5.0.0 -of 46.5.5.0.0 -sc1 relm -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989898981398989898 -rec1 28 -rec2 27 -rec3 29 -xpm 3 -mpm 7 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 4 5 -ebr 82 -emprp 75 125 -ems -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -cpal 100.95.20.92.126.41.6 -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nee -nil -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn -ysketch" },
        { "Coliseum - Shadow", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 9.1.1.12.9 -oe 48.5.5.0.0 -of 46.5.5.0.0 -og 58.1.1.12.2 -sc1 shadow -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989808989898989898989898 -rec1 28 -rec2 27 -rec3 29 -xpm 8 -mpm 5 -gpm 10 -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 50000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -ssf0 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -nfce -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn -yremove" },
        { "Coliseum - Mog", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 36.1.1.12.9 -oe 46.5.5.0.0 -of 48.5.5.0.0 -og 27.8.8.1.1.11.28 -oh 36.1.1.6.2.2 -oi 36.1.1.6.4.4 -sc1 mog -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989898989898199898 -rec1 28 -rec2 27 -rec3 29 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 100 -ieror 100 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -name TERRA.LOCKE.KUPEK.KUPOP.KUMAMA.KUKU.KUTAN.KUPAN.KUSHU.KURIN.MOG.KURU.KAMOG.UMARO -cpor 0.1.10.10.10.10.10.10.10.10.10.10.10.13.14 -cspr 0.1.10.10.10.10.10.10.10.10.10.10.10.13.14.15.18.19.20.21 -cspp 2.1.5.5.5.5.5.5.5.5.5.5.5.5.1.0.6.1.0.3 -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn -ymascot" },
        { "Coliseum - Gogo", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 12.1.1.12.9 -sc1 gogo -sc2 random -sc3 random -sal -eu -csrp 80 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989898989898989898 -xpm 4 -mpm 5 -gpm 5 -nxppd -lsh 1 -hmh 1 -xgh 1 -ase 2.5 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 82 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nfps -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn -yreflect" },
        { "Coliseum - Umaro", "-cg -oa 2.2.2.2.6.6.4.9.9 -ob 3.1.1.8.12.12 -oc 30.3.3.1.1.11.8 -od 5.1.1.12.9 -oe 48.30.30.1.1.12.9 -of 48.10.10.0.0 -sc1 umaro -sc2 random -sc3 random -sal -eu -csrp 100 125 -fst -brl -slr 3 5 -lmprp 75 125 -lel -srr 25 35 -rnl -rnc -sdr 1 2 -das -dda -dns -sch -com 98989898989897979797979897 -scc -rec1 28 -rec2 6 -rec3 11 -rec4 23 -rec5 13 -rec6 14 -xpm 3 -mpm 5 -gpm 5 -nxppd -lsced 2.5 -hmced 2.5 -xgced 2 -ase 2 -msl 50 -sed -bbs -be -bnu -res -fer 0 -escr 100 -dgne -wnz -mmnu -cmd -esr 2 5 -ebr 100 -emprp 75 125 -nm1 random -rnl1 -rns1 -nm2 random -rnl2 -rns2 -nmmi -gp 5000 -smc 3 -sto 1 -ieor 33 -ieror 33 -csb 3 14 -mca -stra -saw -sisr 20 -sprp 75 125 -sdm 5 -npi -snbr -snsb -ccsr 20 -cms -frw -cor -crr -crvr 120 120 -crm -ari -anca -adeh -nmc -nu -nfps -nfce -fs -fe -fvd -fr -fj -fbs -fedc -ond -rr -as -etn" }
    };
}