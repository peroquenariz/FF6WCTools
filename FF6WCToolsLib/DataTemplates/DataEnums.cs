using System;

namespace FF6WCToolsLib.DataTemplates;

public static class DataEnums
{
    public enum SpellDataStructure
    {
        Targeting,
        ElementalProperties,
        SpellFlags1,
        SpellFlags2,
        SpellFlags3,
        MPCost,
        SpellPower,
        SpellFlags4,
        HitRate,
        SpecialEffect,
        Status1,
        Status2,
        Status3,
        Status4
    }
    
    [Flags]
    public enum Targeting : byte
    {
        NONE = 0x00,
        ALLOWS_MOVABLE_CURSOR_FOR_SINGLE_TARGET = 0x01,
        DISABLE_SWITCH_OF_TARGETS_BETWEEN_GROUPS = 0x02,
        SELECT_ALL_TARGETS_BOTH_GROUPS = 0x04,
        SELECT_ONE_GROUP = 0x08,
        AUTO_ACCEPT_DEFAULT_SELECTION = 0x10,
        MULTIPLE_SELECTION_POSSIBLE = 0x20,
        ENEMY_SELECTED_BY_DEFAULT = 0x40,
        RANDOM_SELECTION = 0x80
    }

    [Flags]
    public enum ElementalProperties : byte
    {
        NONE = 0x00,
        FIRE = 0x01,
        ICE = 0x02,
        LIGHTNING = 0x04,
        POISON = 0x08,
        WIND = 0x10,
        PEARL = 0x20,
        EARTH = 0x40,
        WATER = 0x80
    }

    [Flags]
    public enum SpellFlags1 : byte
    {
        NONE = 0x00,
        PHYSICAL_DAMAGE = 0x01,
        MISS_IF_PROTECTED_AGAINST_DEATH = 0x02,
        TARGET_ONLY_DEAD_ALLIES = 0x04,
        INVERSE_DAMAGE_FOR_UNDEAD = 0x08,
        RANDOMIZE_TARGET = 0x10,
        IGNORE_DEFENSE = 0x20,
        DONT_SPLIT_DAMAGE_ON_MULTIPLE_TARGETS = 0x40,
        ABORT_IF_USED_AGAINST_ALLIES = 0x80
    }

    [Flags]
    public enum SpellFlags2 : byte
    {
        NONE = 0x00,
        CAN_USE_SPELL_ON_MENU = 0x01,
        IGNORE_REFLECT = 0x02,
        LEARN_AS_LORE_IF_CAST = 0x04,
        ALLOW_RUNIC = 0x08,
        /// <summary>
        /// Set for Warp and Quick, no idea what this flag does.
        /// </summary>
        UNKNOWN_0x10 = 0x10,
        CHANGE_TARGET_IF_ACTUAL_TARGET_IS_DEAD = 0x20,
        KILL_USER_AFTER_SPELL_IS_CAST = 0x40,
        USE_MP_DAMAGE = 0x80
    }

    [Flags]
    public enum SpellFlags3 : byte
    {
        NONE = 0x00,
        HEAL_TARGET = 0x01,
        DRAIN_FROM_TARGET_TO_CASTER = 0x02,
        LIFT_STATUS = 0x04,
        TOGGLE_STATUS = 0x08,
        USE_STAMINA_IN_EVASION_FORMULA = 0x10,
        CANT_DODGE = 0x20,
        HITS_IF_TARGET_LEVEL_IS_MULTIPLE_OF_SPELL_HIT_RATE = 0x40,
        /// <summary>
        /// Spell power should be between 1 and 16.
        /// </summary>
        USE_FRACTAL_DAMAGE = 0x80
    }

    [Flags]
    public enum SpellFlags4 : byte
    {
        NONE = 0x00,
        MISS_IF_TARGET_IS_PROTECTED_AGAINST_STATUS = 0x01,
        /// <summary>
        /// Monster only.
        /// </summary>
        SHOW_TEXT_IF_SPELL_HITS = 0x02,
        UNKNOWN_0x04 = 0x04,
        UNKNOWN_0x08 = 0x08,
        UNKNOWN_0x10 = 0x10,
        UNKNOWN_0x20 = 0x20,
        UNKNOWN_0x40 = 0x40,
        UNKNOWN_0x80 = 0x80
    }

    [Flags]
    public enum StatusCondition1 : byte
    {
        NONE = 0x00,
        DARK = 0x01,
        ZOMBIE = 0x02,
        POISON = 0x04,
        ENABLE_MAGITEK = 0x08,
        VANISH = 0x10,
        IMP = 0x20,
        PETRIFY = 0x40,
        DEATH = 0x80
    }

    [Flags]
    public enum StatusCondition2 : byte
    {
        NONE = 0x00,
        CONDEMNED = 0x01,
        KNEELING = 0x02,
        BLINK = 0x04,
        SILENCE = 0x08,
        BERSERK = 0x10,
        CONFUSION = 0x20,
        HP_DRAIN = 0x40, // Seizure
        SLEEP = 0x80
    }

    [Flags]
    public enum StatusCondition3 : byte
    {
        NONE = 0x00,
        /// <summary>
        /// Documented as "Dance, but Float for Equip.
        /// </summary>
        DANCE = 0x01,
        REGEN = 0x02,
        SLOW = 0x04,
        HASTE = 0x08,
        STOP = 0x10,
        SHELL = 0x20,
        SAFE = 0x40,
        REFLECT = 0x80
    }

    /// <summary>
    /// Only Interceptor and Float persist after battle.
    /// The rest are cleared.
    /// </summary>
    [Flags]
    public enum StatusCondition4 : byte
    {
        NONE = 0x00,
        RAGE = 0x01,
        FROZEN = 0x02,
        PROTECTION_FROM_DEATH = 0x04,
        MORPH_INTO_ESPER = 0x08,
        CASTING_SPELL = 0x10,
        REMOVED_FROM_BATTLE = 0x20,
        RANDOMLY_DEFENDED_BY_INTERCEPTOR = 0x40,
        FLOAT = 0x80
    }

    public enum ItemDataStructure
    {
        ItemType,
        EquipableActorsLow,
        EquipableActorsHigh,
        SpellLearnRate,
        SpellToLearn,
        FieldEffect,
        StatusProtection1,
        StatusProtection2,
        EquipmentStatus,
        ItemFlags1,
        ItemFlags2,
        ItemFlags3,
        ItemFlags4,
        ItemFlags5,
        Targeting,
        WeaponElement__HalveElement,
        VigorAndSpeed,
        StaminaAndMagic,
        WeaponSpellCasting,
        WeaponFlags__ItemFlags, // Two different enums for these
        WeaponPower__ItemHealPower__PhysDefense,
        WeaponHitRate__Status1__MagDefense,
        Status2__AbsorbElement,
        Status3__NullifyElement,
        Status4__WeakElement,
        _EquipmentStatus,
        PhysicalAndMagicEvasion,
        /// <summary>
        /// Contains 2 4-bit values that store special traits of the item.
        /// Low 4 bits are for evade animation.
        /// High 4 bits are for weapon special effects.
        /// </summary>
        SpecialEffect,
        PriceLowByte,
        PriceHighByte
    }

    public enum ItemType
    {
        Tool,
        Weapon,
        Armor,
        Shield,
        Helm, 
        Relic, 
        Item
    }

    [Flags]
    public enum ItemTypeFlags : byte
    {
        NONE = 0x00,
        _ = 0x01,
        __ = 0x02,
        ___ = 0x04,
        ____ = 0x08,
        CAN_BE_THROWN = 0x10,
        USABLE_IN_BATTLE = 0x20,
        USABLE_IN_MENU = 0x40,
        ________ = 0x80
    }

    [Flags]
    public enum EquipableActorsLow : byte
    {
        NONE = 0x00,
        Terra = 0x01,
        Locke = 0x02,
        Cyan = 0x04,
        Shadow = 0x08,
        Edgar = 0x10,
        Sabin = 0x20,
        Celes = 0x40,
        Strago = 0x80
    }

    [Flags]
    public enum EquipableActorsHigh : byte
    {
        NONE = 0x00,
        Relm = 0x01,
        Setzer = 0x02,
        Mog = 0x04,
        Gau = 0x08,
        Gogo = 0x10,
        Umaro = 0x20,
        UNKNOWN_0x40 = 0x40, // Possibly Leo and Banon???
        UNKNOWN_0x80 = 0x80
    }

    [Flags]
    public enum ItemFlags1 : byte
    {
        NONE = 0x00,
        RAISE_FIGHT_DAMAGE = 0x01,
        RAISE_MAGIC_DAMAGE = 0x02,
        HP_PLUS_25 = 0x04,
        HP_PLUS_50 = 0x08,
        HP_PLUS_12p5 = 0x10,
        MP_PLUS_25 = 0x20,
        MP_PLUS_50 = 0x40,
        MP_PLUS_12p5 = 0x80,
    }

    [Flags]
    public enum ItemFlags2 : byte
    {
        NONE = 0x00,
        PREEMPTIVE_STRIKE = 0x01,
        PREVENT_BACK_AND_PINCER = 0x02,
        UPGRADE_FIGHT_TO_JUMP = 0x04,
        UPGRADE_MAGIC_TO_XMAGIC = 0x08,
        UPGRADE_SKETCH_TO_CONTROL = 0x10,
        UPGRADE_SLOT_TO_GPRAIN = 0x20,
        UPGRADE_STEAL_TO_CAPTURE = 0x40,
        ENHANCES_JUMP = 0x80,
    }

    [Flags]
    public enum ItemFlags3 : byte
    {
        NONE = 0x00,
        ENHANCES_STEAL = 0x01,
        UNKNOWN_0x02 = 0x02,
        ENHANCES_SKETCH = 0x04,
        ENHANCES_CONTROL = 0x08,
        ALWAYS_HITS = 0x10,
        HALVE_MP_CONSUMPTION = 0x20,
        SET_MP_CONSUMPTION_TO_ONE = 0x40,
        RAISES_VIGOR = 0x80,
    }

    [Flags]
    public enum ItemFlags4 : byte
    {
        NONE = 0x00,
        UPGRADE_FIGHT_TO_XFIGHT = 0x01,
        RANDOMLY_COUNTERS = 0x02,
        RANDOMLY_EVADES = 0x04,
        GAUNTLET_EFFECT = 0x08,
        GENJI_GLOVE_EFFECT = 0x10,
        MERIT_AWARD_EFFECT = 0x20,
        TRUE_KNIGHT_EFFECT = 0x40,
        UNKNONW_0x80 = 0x80,
    }

    [Flags]
    public enum ItemFlags5 : byte
    {
        NONE = 0x00,
        SHELL_WHEN_HP_IS_LOW = 0x01,
        SAFE_WHEN_HP_IS_LOW = 0x02,
        REFLECT_WHEN_HP_IS_LOW = 0x04,
        DOUBLES_GAINED_XP = 0x08,
        DOUBLES_GAINED_GOLD = 0x10,
        UNKNOWN_0x20 = 0x20,
        UNKNOWN_0x40 = 0x40,
        MAKES_BODY_COLD = 0x80,
    }

    [Flags]
    public enum WeaponSpellCasting : byte
    {
        NONE = 0x00,
        _ = 0x01,
        __ = 0x02,
        ___ = 0x04,
        ____ = 0x08,
        _____ = 0x10,
        ______ = 0x20,
        ALLOW_RANDOM_CASTING = 0x40,
        REMOVE_FROM_INVENTORY = 0x80
    }

    [Flags]
    public enum WeaponFlags : byte
    {
        NONE = 0x00,
        UNKNOWN_0x01 = 0x01,
        ENABLE_SWDTECH = 0x02,
        UNKNOWN_0x04 = 0x04,
        UNKNOWN_0x08 = 0x08,
        UNKNOWN_0x10 = 0x10,
        UNALTERED_BACK_DAMAGE = 0x20,
        ALLOWS_TWO_HANDS = 0x40,
        RUNIC = 0x80
    }

    [Flags]
    public enum ItemFlags : byte
    {
        NONE = 0x00,
        UNKNOWN_0x01 = 0x01,
        DAMAGE_ON_UNDEAD = 0x02,
        UNKNOWN_0x04 = 0x04,
        AFFECTS_HP = 0x08,
        AFFECTS_MP = 0x10,
        REMOVE_STATUS = 0x20,
        UNKNOWN_0x40 = 0x40,
        MAX_OUT = 0x80
    }

    /// <summary>
    /// Item evade animations.
    /// </summary>
    public enum SpecialItemEffectLowByte
    {
        NOTHING,
        NOTHING_,
        NOTHING__,
        NOTHING___,
        KNIFE_EVADE,
        SWORD_EVADE,
        BUCKLER_EVADE,
        REDCAPESWIRL_EVADE,
        NOTHING____,
        NOTHING_____,
        BUCKLER_MBLOCK,
        NOTHING______,
        KNIFE_EVADE2,
        SWORD_EVADE2,
        BUCKLER_EVADE_MBLOCK,
        REDCAPESWIRL_EVADE2
    }

    /// <summary>
    /// Item extra effects.
    /// </summary>
    public enum SpecialItemEffectHighByte
    {
        NORMAL,
        RANDOMLY_STEAL,
        ATMA_WEAPON_GFX_AT_HIGH_LVL,
        RANDOMLY_KILL,
        TWOX_DAMAGE_FOR_HUMANS,
        DRAIN_HP,
        DRAIN_MP,
        USES_MP_FOR_MORTAL_BLOW,
        RANDOMLY_THROW_WEAPON,
        REPLACE_GFX_WITH_DICES_THROWN,
        STRONGER_ON_LOW_HP,
        RANDOMLY_CAST_WIND_SLASH,
        CURATIVE_ATTRIBUTES,
        RANDOMLY_SLICE,
        OGRE_NIX_BREAKS,
        UNKNOWN
    }

    public enum EsperDataStructure
    {
        Spell1LearnRate,
        Spell1,
        Spell2LearnRate,
        Spell2,
        Spell3LearnRate,
        Spell3,
        Spell4LearnRate,
        Spell4,
        Spell5LearnRate,
        Spell5,
        LevelUpBonus
    }

    public enum EsperLevelUpBonus : byte
    {
        HP_PLUS_10,
        HP_PLUS_30,
        HP_PLUS_50,
        MP_PLUS_10,
        MP_PLUS_30,
        MP_PLUS_50,
        HP_PLUS_100,
        LV_PLUS_30,
        LV_PLUS_50,
        STRENGTH_PLUS_1,
        STRENGTH_PLUS_2,
        SPEED_PLUS_1,
        SPEED_PLUS_2,
        STAMINA_PLUS_1,
        STAMINA_PLUS_2,
        MAGPWR_PLUS_1,
        MAGPWR_PLUS_2,
        NO_BONUS = 0xFF
    }

    public enum CharacterDataStructure
    {
        ActorIndex,
        GraphicIndex,
        Name,
        Level = 0x08,
        CurrentHP,
        Max_HP_and_boost = 0x0B,
        CurrentMP = 0x0D,
        Max_MP_and_boost = 0x0F,
        ExperiencePoints = 0x11,
        Status1 = 0x14,
        Status4,
        BattleCommand1,
        BattleCommand2,
        BattleCommand3,
        BattleCommand4,
        Vigor,
        Speed,
        Stamina,
        MagPower,
        Esper,
        Weapon,
        Shield,
        Helmet,
        Armor,
        Relic1,
        Relic2
    }

    
}