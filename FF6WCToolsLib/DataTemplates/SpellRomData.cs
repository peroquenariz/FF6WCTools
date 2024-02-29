using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents a spell's ROM block.
/// </summary>
public class SpellRomData : BaseRomData
{
    public static uint StartAddress => SPELL_DATA_START;
    public static byte BlockSize => SPELL_DATA_BLOCK_SIZE;
    public static int BlockCount => SPELL_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellRomData(byte[] defaultSpellData, int spellIndex) : base(defaultSpellData, spellIndex) { }

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

    /// <summary>
    /// Modifies a spell to make it usable in the menu.
    /// </summary>
    public void ToggleUsableOnMenu()
    {
        byte usableInMenu = _data[(int)SpellDataStructure.SpellFlags2];
        usableInMenu = DataHandler.ToggleBit(usableInMenu, (byte)SpellFlags2.CAN_USE_SPELL_ON_MENU);
        _data[(int)SpellDataStructure.SpellFlags2] = usableInMenu;
    }

    /// <summary>
    /// Modifies a spell to change its targeting.
    /// </summary>
    /// <param name="targetingPreset"></param>
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

    /// <summary>
    /// Modifies a spell to change its spell power.
    /// </summary>
    /// <param name="spellPower">The new spell power value.</param>
    public void SetSpellPower(byte spellPower)
    {
        _data[(int)SpellDataStructure.SpellPower] = spellPower;
    }

    /// <summary>
    /// Modifies a spell to toggle their elements.
    /// </summary>
    /// <param name="element">The element to toggle.</param>
    public void ToggleElement(ElementalProperties element)
    {
        byte elementData = _data[(int)SpellDataStructure.ElementalProperties];
        elementData = DataHandler.ToggleBit(elementData, (byte)element);
        _data[(int)SpellDataStructure.ElementalProperties] = elementData;
    }

    /// <summary>
    /// Modifies a spell to enable or disable MP damage.
    /// </summary>
    public void ToggleMPDamage()
    {
        byte mpDamageData = _data[(int)SpellDataStructure.SpellFlags2];
        mpDamageData = DataHandler.ToggleBit(mpDamageData, (byte)SpellFlags2.USE_MP_DAMAGE);
        _data[(int)SpellDataStructure.SpellFlags2] = mpDamageData;
    }

    /// <summary>
    /// Modifies a spell to toggle defense piercing damage.
    /// </summary>
    public void ToggleIgnoreDefense()
    {
        byte ignoreDefenseData = _data[(int)SpellDataStructure.SpellFlags1];
        ignoreDefenseData = DataHandler.ToggleBit(ignoreDefenseData, (byte)SpellFlags1.IGNORE_DEFENSE);
        _data[(int)SpellDataStructure.SpellFlags1] = ignoreDefenseData;
    }

    /// <summary>
    /// Modifies a spell to change its MP cost.
    /// </summary>
    /// <param name="spellMPCost">The new MP cost to set.</param>
    public void SetMPCost(byte spellMPCost)
    {
        _data[(int)SpellDataStructure.MPCost] = spellMPCost;
    }

    /// <summary>
    /// Modifies a spell to apply a status on hit.
    /// </summary>
    /// <param name="statusFlag">The status bit flag.</param>
    /// <param name="statusByteOffset">The status byte offset.</param>
    public void ToggleStatus(byte statusFlag, byte statusByteOffset)
    {
        int statusByteIndex = (int)SpellDataStructure.Status1 + statusByteOffset;
        byte statusData = _data[statusByteIndex];
        statusData = DataHandler.ToggleBit(statusData, statusFlag);
        _data[statusByteIndex] = statusData;
    }

    /// <summary>
    /// Modifies a spell to toggle its status lifting capabilities.
    /// </summary>
    public void ToggleLiftStatus()
    {
        byte liftStatus = _data[(int)SpellDataStructure.SpellFlags3];
        liftStatus = DataHandler.ToggleBit(liftStatus, (byte)SpellFlags3.LIFT_STATUS);
        _data[(int)SpellDataStructure.SpellFlags3] = liftStatus;
    }
}