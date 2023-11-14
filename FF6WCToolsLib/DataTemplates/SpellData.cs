﻿using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class SpellData : BaseRomData
{
    public static uint StartAddress => SPELL_DATA_START;
    public static byte BlockSize => SPELL_DATA_BLOCK_SIZE;
    public static int BlockCount => SPELL_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellData(byte[] defaultSpellData, int spellIndex) : base(defaultSpellData, spellIndex) { }

    public void SetSpecialEffect(byte specialEffect)
    {
        _data[(int)SpellDataStructure.SpecialEffect] = specialEffect;
    }
    
    public void ChangeTargeting(Targeting targetingFlags)
    {
        byte targetingData = (byte)targetingFlags;
        _data[(int)SpellDataStructure.Targeting] = targetingData;
    }

    public void SetSpellPower(byte spellPower)
    {
        _data[(int)SpellDataStructure.SpellPower] = spellPower;
    }

    public void ToggleMPDamage()
    {
        bool isMPDamageEnabled = DataHandler.CheckBitSet(_data[(int)SpellDataStructure.SpellFlags2], (byte)SpellFlags2.USE_MP_DAMAGE);
        if (!isMPDamageEnabled)
        {
            _data[(int)SpellDataStructure.SpellFlags2] |= (byte)SpellFlags2.USE_MP_DAMAGE;
        }
        else
        {
            _data[(int)SpellDataStructure.SpellFlags2] &= (byte)~SpellFlags2.USE_MP_DAMAGE;
        }
        isMPDamageEnabled = DataHandler.CheckBitSet(_data[(int)SpellDataStructure.SpellFlags2], (byte)SpellFlags2.USE_MP_DAMAGE);
    }

    public void SetElemementalProperties(ElementalProperties elementalProperties)
    {
        _data[(int)SpellDataStructure.ElementalProperties] = (byte)elementalProperties;
    }

    public override string ToString()
    {
        string spellDescription =
            $"Spell name: {SPELL_DICT[(byte)_dataIndex]}\n" + // TODO: replace with current name in memory!
            $"Targeting: {(Targeting)_data[(int)SpellDataStructure.Targeting]}\n" +
            $"Elemental properties: {(ElementalProperties)_data[(int)SpellDataStructure.ElementalProperties]}\n" +
            $"Spell flags 1: {(SpellFlags1)_data[(int)SpellDataStructure.SpellFlags1]}\n" +
            $"Spell flags 2: {(SpellFlags2)_data[(int)SpellDataStructure.SpellFlags2]}\n" +
            $"Spell flags 3: {(SpellFlags3)_data[(int)SpellDataStructure.SpellFlags3]}\n" +
            $"MP cost: {_data[(int)SpellDataStructure.MPCost]}\n" +
            $"Spell power: {_data[(int)SpellDataStructure.SpellPower]}\n" +
            $"Spell flags 4: {(SpellFlags4)_data[(int)SpellDataStructure.SpellFlags4]}\n" +
            $"Hit rate: {_data[(int)SpellDataStructure.HitRate]}\n" +
            $"Special effect: {_data[(int)SpellDataStructure.SpecialEffect]}\n" +
            $"Status 1: {(StatusCondition1)_data[(int)SpellDataStructure.Status1]}\n" +
            $"Status 2: {(StatusCondition2)_data[(int)SpellDataStructure.Status2]}\n" +
            $"Status 3: {(StatusCondition3)_data[(int)SpellDataStructure.Status3]}\n" +
            $"Status 4: {(StatusCondition4)_data[(int)SpellDataStructure.Status4]}\n";
        return spellDescription;
    }
}