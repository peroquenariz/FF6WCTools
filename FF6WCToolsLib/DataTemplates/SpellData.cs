using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;

namespace FF6WCToolsLib.DataTemplates;

public class SpellData : BaseData
{
    private readonly int _spellIndex;
    private byte[] _spellData;

    public override uint DataStartAddress => SPELL_DATA_START;
    public override byte DataBlockSize => SPELL_DATA_BLOCK_SIZE;
    public override int DataBlockCount => SPELL_DATA_BLOCK_COUNT;
    
    public SpellData(byte[] data, int spellIndex)
    {
        _spellIndex = spellIndex;
        _spellData = data;
    }

    public override byte[] ToByteArray()
    {
        return _spellData;
    }

    public override string ToString()
    {
        string spellDescription =
            $"Spell name: {SPELL_DICT[(byte)_spellIndex]}\n" + // TODO: replace with current name in memory!
            $"Targeting: {(Targeting)_spellData[(int)SpellDataStructure.Targeting]}\n" +
            $"Elemental properties: {(ElementalProperties)_spellData[(int)SpellDataStructure.ElementalProperties]}\n" +
            $"Spell flags 1: {(SpellFlags1)_spellData[(int)SpellDataStructure.SpellFlags1]}\n" +
            $"Spell flags 2: {(SpellFlags2)_spellData[(int)SpellDataStructure.SpellFlags2]}\n" +
            $"Spell flags 3: {(SpellFlags3)_spellData[(int)SpellDataStructure.SpellFlags3]}\n" +
            $"MP cost: {_spellData[(int)SpellDataStructure.MPCost]}\n" +
            $"Spell power: {_spellData[(int)SpellDataStructure.SpellPower]}\n" +
            $"Spell flags 4: {(SpellFlags4)_spellData[(int)SpellDataStructure.SpellFlags4]}\n" +
            $"Hit rate: {_spellData[(int)SpellDataStructure.HitRate]}\n" +
            $"Special effect: {_spellData[(int)SpellDataStructure.SpecialEffect]}\n" +
            $"Status 1: {(StatusCondition1)_spellData[(int)SpellDataStructure.Status1]}\n" +
            $"Status 2: {(StatusCondition2)_spellData[(int)SpellDataStructure.Status2]}\n" +
            $"Status 3: {(StatusCondition3)_spellData[(int)SpellDataStructure.Status3]}\n" +
            $"Status 4: {(StatusCondition4)_spellData[(int)SpellDataStructure.Status4]}\n";
        return spellDescription;
    }
}
