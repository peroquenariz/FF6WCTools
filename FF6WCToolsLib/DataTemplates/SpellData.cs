using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;

namespace FF6WCToolsLib.DataTemplates;

public class SpellData : BaseData
{
    private readonly int _spellIndex;
    
    private byte _targeting;
    private byte _elementalProperties;
    private byte _SpellFlags1;
    private byte _SpellFlags2;
    private byte _SpellFlags3;
    private byte _mpCost;
    private byte _spellPower;
    private byte _SpellFlags4;
    private byte _hitRate;
    private byte _specialEffect;
    private byte _status1;
    private byte _status2;
    private byte _status3;
    private byte _status4;

    public override uint DataStartAddress => SPELL_DATA_START;
    public override byte DataBlockSize => SPELL_DATA_BLOCK_SIZE;
    public override int DataBlockCount => SPELL_DATA_BLOCK_COUNT;
    
    public SpellData(byte[] data, int spellIndex)
    {
        _spellIndex = spellIndex;
        
        // Initialize spell data.
        _targeting = data[0];
        _elementalProperties = data[1];
        _SpellFlags1 = data[2];
        _SpellFlags2 = data[3];
        _SpellFlags3 = data[4];
        _mpCost = data[5];
        _spellPower = data[6];
        _SpellFlags4 = data[7];
        _hitRate = data[8];
        _specialEffect = data[9];
        _status1 = data[10];
        _status2 = data[11];
        _status3 = data[12];
        _status4 = data[13];
    }

    public override byte[] ToByteArray()
    {
        return new byte[] {
            _targeting,
            _elementalProperties,
            _SpellFlags1,
            _SpellFlags2,
            _SpellFlags3,
            _mpCost,
            _spellPower,
            _SpellFlags4,
            _hitRate,
            _specialEffect,
            _status1,
            _status2,
            _status3,
            _status4
        };
    }

    public override string ToString()
    {
        string spellDescription = 
            $"Spell name: {SPELL_DICT[(byte)_spellIndex]}\n" +
            $"Targeting: {(Targeting)_targeting}\n" +
            $"Elemental properties: {(ElementalProperties)_elementalProperties}\n" +
            $"Spell flags 1: {(SpellFlags1)_SpellFlags1}\n" +
            $"Spell flags 2: {(SpellFlags2)_SpellFlags2}\n" +
            $"Spell flags 3: {(SpellFlags3)_SpellFlags3}\n" +
            $"MP cost: {_mpCost}\n" +
            $"Spell power: {_spellPower}\n" +
            $"Spell flags 4: {(SpellFlags4)_SpellFlags4}\n" +
            $"Hit rate: {_hitRate}\n" +
            $"Special effect: {_specialEffect}\n" +
            $"Status 1: {(StatusCondition1)_status1}\n" +
            $"Status 2: {(StatusCondition2)_status2}\n" +
            $"Status 3: {(StatusCondition3)_status3}\n" +
            $"Status 4: {(StatusCondition4)_status4}\n";
        return spellDescription;
    }
}
