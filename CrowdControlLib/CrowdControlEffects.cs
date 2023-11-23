using System.Collections.Generic;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace CrowdControlLib;

public static class CrowdControlEffects
{
    /// <summary>
    /// Maps the status crowd control effect to its corresponding bit flag and byte offset.
    /// </summary>
    public static readonly Dictionary<StatusEffect, (byte BitFlag, byte Offset)> StatusEffectByteData = new()
    {
        { StatusEffect.dark, ((byte)StatusCondition1.DARK, 0) },
        { StatusEffect.poison, ((byte)StatusCondition1.POISON, 0) },
        { StatusEffect.imp, ((byte)StatusCondition1.IMP, 0) },
        { StatusEffect.petrify, ((byte)StatusCondition1.PETRIFY, 0) },
        { StatusEffect.death, ((byte)StatusCondition1.DEATH, 0) },
        { StatusEffect.condemned, ((byte)StatusCondition2.CONDEMNED, 1) },
        { StatusEffect.image, ((byte)StatusCondition2.IMAGE, 1) },
        { StatusEffect.mute, ((byte)StatusCondition2.MUTE, 1) },
        { StatusEffect.berserk, ((byte)StatusCondition2.BERSERK, 1) },
        { StatusEffect.muddle, ((byte)StatusCondition2.MUDDLE, 1) },
        { StatusEffect.seizure, ((byte)StatusCondition2.SEIZURE, 1) },
        { StatusEffect.sleep, ((byte)StatusCondition2.SLEEP, 1) },
        { StatusEffect.regen, ((byte)StatusCondition3.REGEN, 2) },
        { StatusEffect.slow, ((byte)StatusCondition3.SLOW, 2) },
        { StatusEffect.haste, ((byte)StatusCondition3.HASTE, 2) },
        { StatusEffect.stop, ((byte)StatusCondition3.STOP, 2) },
        { StatusEffect.shell, ((byte)StatusCondition3.SHELL, 2) },
        { StatusEffect.safe, ((byte)StatusCondition3.SAFE, 2) },
        { StatusEffect.reflect, ((byte)StatusCondition3.REFLECT, 2) },
        { StatusEffect.frozen, ((byte)StatusCondition4.FROZEN, 3) },
        { StatusEffect.Float, ((byte)StatusCondition4.FLOAT, 3) },
        { StatusEffect.interceptor, ((byte)StatusCondition4.INTERCEPTOR, 3) },
    };
    
    public enum Effect
    {
        _INVALID,
        item,
        spell,
        character,
        inventory,
        itemname,
        spellname,
        charactername,
        gp,
        window,
        mirror,
    }

    public enum WindowEffect
    {
        _INVALID,
        vanilla,
        demonchocobo,
        random,
    }

    public enum GPEffect
    {
        modify,
        empty,
    }

    public enum ItemEffect
    {
        spellproc,
        breakable,
        teach,
        reliceffect,
        statboost,
        absorb,
        nullify,
        weak,
        weaponelement,
        price,
        reset,
    }

    public enum SpellEffect
    {
        targeting,
        spellpower,
        element,
        mpdamage,
        ignoredefense,
        status,
        liftstatus,
        mpcost,
        reset,
    }

    public enum StatusEffect
    {
        dark,
        poison,
        imp,
        petrify,
        death,
        condemned,
        image,
        mute,
        berserk,
        muddle,
        seizure,
        sleep,
        regen,
        slow,
        haste,
        stop,
        shell,
        safe,
        reflect,
        frozen,
        Float,
        interceptor,
    }
}