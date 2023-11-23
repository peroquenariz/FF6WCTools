using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;

namespace FF6WCToolsLib.DataTemplates;

public class SpellData : BaseRomData
{
    public static uint StartAddress => SPELL_DATA_START;
    public static byte BlockSize => SPELL_DATA_BLOCK_SIZE;
    public static int BlockCount => SPELL_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellData(byte[] defaultSpellData, int spellIndex) : base(defaultSpellData, spellIndex) { }

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

    public void ModifyTargeting(TargetingPreset targetingPreset)
    {
        Targeting currentTargeting = (Targeting)_data[(int)SpellDataStructure.Targeting];
        
        // Toggle auto-accept target.
        // Ignore toggle if roulette is on.
        // TODO: show message if auto toggle fails.
        if (targetingPreset == TargetingPreset.auto && !DataHandler.CheckBitSet((byte)currentTargeting, (byte)Targeting.RANDOM_SELECTION))
        {
            byte newTargeting = DataHandler.ToggleBit((byte)currentTargeting, (byte)targetingPreset);
            newTargeting = DataHandler.ToggleBit(newTargeting, (byte)Targeting.DISABLE_SWITCH_OF_TARGETS_BETWEEN_GROUPS);
            _data[(int)SpellDataStructure.Targeting] = newTargeting;
        }
        // Any other preset replaces the old targeting.
        else if (targetingPreset != TargetingPreset.auto)
        {
            _data[(int)SpellDataStructure.Targeting] = (byte)targetingPreset;
        }
    }

    public void SetSpellPower(byte spellPower)
    {
        _data[(int)SpellDataStructure.SpellPower] = spellPower;
    }

    public void ToggleElement(ElementalProperties element)
    {
        byte elementData = _data[(int)SpellDataStructure.ElementalProperties];
        elementData = DataHandler.ToggleBit(elementData, (byte)element);
        _data[(int)SpellDataStructure.ElementalProperties] = elementData;
    }

    public void ToggleMPDamage()
    {
        byte mpDamageData = _data[(int)SpellDataStructure.SpellFlags2];
        mpDamageData = DataHandler.ToggleBit(mpDamageData, (byte)SpellFlags2.USE_MP_DAMAGE);
        _data[(int)SpellDataStructure.SpellFlags2] = mpDamageData;
    }

    public void ToggleIgnoreDefense()
    {
        byte ignoreDefenseData = _data[(int)SpellDataStructure.SpellFlags1];
        ignoreDefenseData = DataHandler.ToggleBit(ignoreDefenseData, (byte)SpellFlags1.IGNORE_DEFENSE);
        _data[(int)SpellDataStructure.SpellFlags1] = ignoreDefenseData;
    }

    public void SetMPCost(byte spellMPCost)
    {
        _data[(int)SpellDataStructure.MPCost] = spellMPCost;
    }

    public void ToggleStatus(byte statusFlag, byte statusByteOffset)
    {
        int statusByteIndex = (int)SpellDataStructure.Status1 + statusByteOffset;
        byte statusData = _data[statusByteIndex];
        statusData = DataHandler.ToggleBit(statusData, statusFlag);
        _data[statusByteIndex] = statusData;
    }

    public void ToggleLiftStatus()
    {
        byte liftStatus = _data[(int)SpellDataStructure.SpellFlags3];
        liftStatus = DataHandler.ToggleBit(liftStatus, (byte)SpellFlags3.LIFT_STATUS);
        _data[(int)SpellDataStructure.SpellFlags3] = liftStatus;
    }
}