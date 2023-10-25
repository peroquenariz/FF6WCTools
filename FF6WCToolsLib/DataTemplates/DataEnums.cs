using System;

namespace FF6WCToolsLib.DataTemplates;

public static class DataEnums
{
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
        HP_DRAIN = 0x40,
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
}
