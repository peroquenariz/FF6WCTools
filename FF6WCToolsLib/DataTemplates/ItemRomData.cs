using System;
using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents an item's ROM data.
/// </summary>
public class ItemRomData : BaseRomData
{
    private readonly ItemType _itemType;

    public static uint StartAddress => ITEM_DATA_START;
    public static byte BlockSize => ITEM_DATA_BLOCK_SIZE;
    public static int BlockCount => ITEM_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public ItemRomData(byte[] itemData, int itemIndex) : base(itemData, itemIndex)
    {
        // Extract the item type.
        _itemType = (ItemType)(itemData[0] & 0x07);
    }

    public override string ToString()
    {
        string itemDescription =
            $"Item original name: {ITEM_DICT[(byte)_dataIndex]}\n" + // TODO: check actual name too!
            $"Item type flags: {(ItemTypeFlags)_data[(int)ItemDataStructure.ItemType]}\n" +
            $"Item type: {_itemType}\n" +
            $"Equipable Actors (low): {(EquipableActorsLow)_data[(int)ItemDataStructure.EquipableActorsLow]}\n" +
            $"Equipable Actors (high): {(EquipableActorsHigh)_data[(int)ItemDataStructure.EquipableActorsHigh]}\n" +
            $"Spell learn rate: {_data[(int)ItemDataStructure.SpellLearnRate]}\n" +
            $"Spell to learn: {_data[(int)ItemDataStructure.SpellToLearn]}\n" +
            $"Field effect: {_data[(int)ItemDataStructure.FieldEffect]}\n" + // TODO: What even is this byte????
            $"Status protection 1: {(StatusCondition1)_data[(int)ItemDataStructure.StatusProtection1]}\n" +
            $"Status protection 2: {(StatusCondition2)_data[(int)ItemDataStructure.StatusProtection2]}\n" +
            $"Equipment status: {(StatusCondition3)_data[(int)ItemDataStructure.EquipmentStatus]}\n" +
            $"Item flags 1: {(ItemFlags1)_data[(int)ItemDataStructure.ItemFlags1]}\n" +
            $"Item flags 2: {(ItemFlags2)_data[(int)ItemDataStructure.ItemFlags2]}\n" +
            $"Item flags 3: {(ItemFlags3)_data[(int)ItemDataStructure.ItemFlags3]}\n" +
            $"Item flags 4: {(ItemFlags4)_data[(int)ItemDataStructure.ItemFlags4]}\n" +
            $"Item flags 5: {(ItemFlags5)_data[(int)ItemDataStructure.ItemFlags5]}\n" +
            $"Targeting: {(Targeting)_data[(int)ItemDataStructure.Targeting]}\n";
        itemDescription += _itemType == ItemType.Weapon ? "Weapon element type: " : "Halves elemental damage: ";
        itemDescription += $"{(ElementalProperties)_data[(int)ItemDataStructure.WeaponElement__HalveElement]}\n";
        
        byte[] vigorAndSpeed = DataHandler.GetItemStatBoostInfo(_data[(int)ItemDataStructure.VigorAndSpeed]);
        byte[] staminaAndMagic = DataHandler.GetItemStatBoostInfo(_data[(int)ItemDataStructure.StaminaAndMagic]);
        string vigorSign = Convert.ToBoolean(vigorAndSpeed[1]) ? "-" : "+";
        string speedSign = Convert.ToBoolean(vigorAndSpeed[3]) ? "-" : "+";
        string staminaSign = Convert.ToBoolean(vigorAndSpeed[1]) ? "-" : "+";
        string magicSign = Convert.ToBoolean(vigorAndSpeed[3]) ? "-" : "+";
        itemDescription +=
            $"Vigor: {vigorSign}{vigorAndSpeed[0]}\n" +
            $"Speed: {speedSign}{vigorAndSpeed[2]}\n" +
            $"Stamina: {staminaSign}{staminaAndMagic[0]}\n" +
            $"Magic: {magicSign}{staminaAndMagic[2]}\n" +
            $"Spell ID: {SPELL_DICT[(byte)(_data[(int)ItemDataStructure.WeaponSpellCasting] & 0x3F)]}\n" +
            $"WeaponSpellCasting: {(WeaponSpellCasting)_data[(int)ItemDataStructure.WeaponSpellCasting]}\n";
        itemDescription += _itemType == ItemType.Weapon ? "Weapon flags: " : "Item flags: ";
        itemDescription += _itemType == ItemType.Weapon ?
            (WeaponFlags)_data[(int)ItemDataStructure.WeaponFlags__ItemFlags] :
            (ItemFlags)_data[(int)ItemDataStructure.WeaponFlags__ItemFlags];
        itemDescription += "\n";
        switch (_itemType)
        {
            case ItemType.Weapon:
                itemDescription += "Weapon power: ";
                break;
            case ItemType.Item:
                itemDescription += "Item heal power: ";
                break;
            default:
                itemDescription += "Physical defense: ";
                break;
        }
        itemDescription += $"{_data[(int)ItemDataStructure.WeaponPower__ItemHealPower__PhysDefense]}\n";
        switch (_itemType)
        {
            case ItemType.Weapon:
                itemDescription += $"Weapon hit rate: {_data[(int)ItemDataStructure.WeaponHitRate__Status1__MagDefense]}";
                break;
            case ItemType.Item:
                itemDescription += $"Actor status 1: {(StatusCondition1)_data[(int)ItemDataStructure.WeaponHitRate__Status1__MagDefense]}";
                break;
            default:
                itemDescription += $"Magic defense: {_data[(int)ItemDataStructure.WeaponHitRate__Status1__MagDefense]}";
                break;
        }
        itemDescription += "\n";
        itemDescription += _itemType == ItemType.Item ?
            $"Actor status 2 {(StatusCondition2)_data[(int)ItemDataStructure.Status2__AbsorbElement]}\n" :
            $"Absorb element: {(ElementalProperties)_data[(int)ItemDataStructure.Status2__AbsorbElement]}\n";
        itemDescription += _itemType == ItemType.Item ?
            $"Actor status 2 {(StatusCondition3)_data[(int)ItemDataStructure.Status3__NullifyElement]}\n" :
            $"Nullify element: {(ElementalProperties)_data[(int)ItemDataStructure.Status3__NullifyElement]}\n";
        itemDescription += _itemType == ItemType.Item ?
            $"Actor status 2 {(StatusCondition4)_data[(int)ItemDataStructure.Status4__WeakElement]}\n" :
            $"Weak element: {(ElementalProperties)_data[(int)ItemDataStructure.Status4__WeakElement]}\n";
        itemDescription += 
            $"?Equipment status: {(StatusCondition2)_data[(int)ItemDataStructure._EquipmentStatus]}\n" +
            $"Physical evasion: {_data[(int)ItemDataStructure.PhysicalAndMagicEvasion] & 0x0F}\n" +
            $"Magic evasion: {(_data[(int)ItemDataStructure.PhysicalAndMagicEvasion] & 0xF0) >> 4}\n" +
            $"Evade animation: {(SpecialItemEffectLowByte)(_data[(int)ItemDataStructure.SpecialEffect] & 0x0F)}\n" +
            $"Special effect: {(SpecialItemEffectHighByte)((_data[(int)ItemDataStructure.SpecialEffect] & 0xF0) >> 4)}\n" +
            $"Price: {DataHandler.ConcatenateByteArray(_data[(int)ItemDataStructure.PriceLowByte..((int)ItemDataStructure.PriceHighByte + 1)])}\n";

        return itemDescription;
    }

    /// <summary>
    /// Modifies an item to be able to proc a spell on hit.
    /// </summary>
    /// <param name="spell">The spell to proc.</param>
    public void SpellProc(SpellRomData spell)
    {
        // Enable spell proccing. TODO: research if proc targeting can be modified per spell.
        _data[(int)ItemDataStructure.WeaponSpellCasting] |= (byte)WeaponSpellCasting.ALLOW_RANDOM_CASTING;

        // Clear any spell data, but keep other parameters intact.
        byte weaponProcParams = 0b11000000;
        _data[(int)ItemDataStructure.WeaponSpellCasting] &= weaponProcParams;
        
        // Add the spell.
        _data[(int)ItemDataStructure.WeaponSpellCasting] |= (byte)spell.Index;
    }

    /// <summary>
    /// Modifies an item to be able to be broken to cast a spell.
    /// </summary>
    /// <param name="spell">The spell to cast on item break.</param>
    public void Breakable(SpellRomData spell)
    {
        // Enable breakability.
        _data[(int)ItemDataStructure.WeaponSpellCasting] |= (byte)WeaponSpellCasting.REMOVE_FROM_INVENTORY;

        // Mark is as usable in the inventory.
        _data[(int)ItemDataStructure.ItemType] |= (byte)ItemTypeFlags.USABLE_IN_BATTLE;

        // Clear any spell data, but keep other parameters intact.
        byte weaponProcParams = 0b11000000;
        _data[(int)ItemDataStructure.WeaponSpellCasting] &= weaponProcParams;

        // Add the spell.
        _data[(int)ItemDataStructure.WeaponSpellCasting] |= (byte)spell.Index;

        // Copy spell targeting.
        _data[(int)ItemDataStructure.Targeting] |= spell.ToByteArray()[(int)SpellDataStructure.Targeting];

        // Disable item throwability.
        _data[(int)ItemDataStructure.ItemType] &= (byte)~ItemTypeFlags.CAN_BE_THROWN;
    }

    /// <summary>
    /// Modifies an item to make it teach a spell.
    /// </summary>
    /// <param name="spell">The spell to teach.</param>
    /// <param name="learnRate">The learn rate of the spell.</param>
    public void TeachSpell(SpellRomData spell, byte learnRate)
    {
        // Set the spell to be taught.
        _data[(int)ItemDataStructure.SpellToLearn] = (byte)spell.Index;

        // Set the learning rate.
        _data[(int)ItemDataStructure.SpellLearnRate] = learnRate;
    }

    /// <summary>
    /// Modifies an item to add a relic property to it.
    /// </summary>
    /// <param name="relicEffectItem">The relic to copy its effects from.</param>
    public void AddRelicEffect(ItemRomData relicEffectItem)
    {
        // Get the original relic data. Use default data in case the current data is modified!
        byte[] dataToCopy = relicEffectItem.DefaultData;

        // Copy all relevant relic data into the new array.
        byte[] indexesToCopy = new byte[]
        {
            (int)ItemDataStructure.StatusProtection1,
            (int)ItemDataStructure.StatusProtection2,
            (int)ItemDataStructure.ItemFlags1,
            (int)ItemDataStructure.ItemFlags2,
            (int)ItemDataStructure.ItemFlags3,
            (int)ItemDataStructure.ItemFlags4,
            (int)ItemDataStructure.ItemFlags5,
            (int)ItemDataStructure.WeaponElement__HalveElement,
            (int)ItemDataStructure.Status2__AbsorbElement,
            (int)ItemDataStructure.Status3__NullifyElement,
            (int)ItemDataStructure.Status4__WeakElement,
            (int)ItemDataStructure.EquipmentStatus,
            (int)ItemDataStructure._EquipmentStatus,
            (int)ItemDataStructure.FieldEffect,
        };

        for (int i = 0; i < indexesToCopy.Length; i++)
        {
            int index = indexesToCopy[i];
            
            // Bitwise OR this array against the existing item data
            _data[index] |= dataToCopy[index];
        }
    }

    /// <summary>
    /// Toggles an item's elemental properties.
    /// Usable for weapon damage and absorb/nullify/weak element.
    /// </summary>
    /// <param name="element">The element to toggle.</param>
    /// <param name="dataIndex">The elemental property to modify.</param>
    public void ToggleElementalProperty(ElementalProperties element, int dataIndex)
    {
        byte data = _data[dataIndex];
        data = DataHandler.ToggleBit(data, (byte)element);
        _data[dataIndex] = data;
    }

    /// <summary>
    /// Modifies an item to provide a stat boost/debuff.
    /// </summary>
    /// <param name="statBoostType">The stat to modify.</param>
    /// <param name="statBoostValue">The boost value.</param>
    public void SetStatBoost(Stat statBoostType, int statBoostValue)
    {
        // Take the stat boost type and value and generate the corresponding byte data.
        bool isHighBits = DataHandler.SetStatBoost(statBoostType, statBoostValue, out byte statBoostData);

        // Select the correct byte index to modify.
        ItemDataStructure statIndex =
            statBoostType is Stat.vigor or Stat.speed ? ItemDataStructure.VigorAndSpeed : ItemDataStructure.StaminaAndMagic;

        // Clear the current stat boost.
        if (isHighBits)
        {
            // Speed and MagPow are in the high 4 bits.
            _data[(int)statIndex] &= 0x0F;
        }
        else
        {
            // Vigor and stamina are in the low 4 bits.
            _data[(int)statIndex] &= 0xF0;
        }

        // Add the stat boost to the item.
        _data[(int)statIndex] |= statBoostData;
    }

    /// <summary>
    /// Modifies an item to change its vendor price.
    /// </summary>
    /// <param name="gpAmount">The GP value to set.</param>
    public void SetPrice(int gpAmount)
    {
        byte[] priceArray = DataHandler.DecatenateInteger(gpAmount, 2); // Item price is 2 bytes.
        _data[(int)ItemDataStructure.PriceLowByte] = priceArray[0];
        _data[(int)ItemDataStructure.PriceHighByte] = priceArray[1];
    }
}