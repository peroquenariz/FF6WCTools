using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;

namespace FF6WCToolsLib.DataTemplates;

public class ItemData : BaseRomData
{
    private ItemType _itemType;

    public static uint StartAddress => ITEM_DATA_START;
    public static byte BlockSize => ITEM_DATA_BLOCK_SIZE;
    public static int BlockCount => ITEM_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public ItemData(byte[] itemData, int itemIndex) : base(itemData, itemIndex)
    {
        _itemType = (ItemType)(itemData[0] & 0x07);
    }

    public void ChangeArmorElementAbsorb(ElementalProperties elementalProperties)
    {
        if (_itemType == ItemType.Armor)
        {
            _data[(int)ItemDataStructure.Status2__AbsorbElement] |= (byte)elementalProperties;
        }
    }

    public void ToggleRelicEffect(ItemFlags4 itemFlags4)
    {
        bool isFlagSet = DataHandler.CheckBitSet(_data[(int)ItemDataStructure.ItemFlags4], (byte)itemFlags4);
        if (!isFlagSet)
        {
            _data[(int)ItemDataStructure.ItemFlags4] |= (byte)itemFlags4;
        }
        else
        {
            _data[(int)ItemDataStructure.ItemFlags4] &= (byte)~itemFlags4;
        }
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
            $"Field effect: {_data[(int)ItemDataStructure.SpellLearnRate]}\n" + // TODO: What even is this byte????
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

    public void SpellProc(SpellData spell)
    {
        // Enable spell proccing. TODO: research if proc targeting can be modified per spell.
        _data[(int)ItemDataStructure.WeaponSpellCasting] |= (byte)WeaponSpellCasting.ALLOW_RANDOM_CASTING;

        // Clear any spell data, but keep other parameters intact.
        byte weaponProcParams = 0b11000000;
        _data[(int)ItemDataStructure.WeaponSpellCasting] &= weaponProcParams;
        
        // Add the spell.
        _data[(int)ItemDataStructure.WeaponSpellCasting] |= (byte)spell.Index;
    }

    public void Breakable(SpellData spell)
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

    public void TeachSpell(SpellData spell, byte learnRate)
    {
        // Set the spell to be taught.
        _data[(int)ItemDataStructure.SpellToLearn] = (byte)spell.Index;

        // Set the learning rate.
        _data[(int)ItemDataStructure.SpellLearnRate] = learnRate;
    }

    public void AddRelicEffect(Item relicEffectItem)
    {
        throw new NotImplementedException();
    }

    public void SetElementAbsorb(ElementalProperties elementalProperties)
    {
        throw new NotImplementedException();
    }

    public void SetElementNullify(ElementalProperties element)
    {
        throw new NotImplementedException();
    }

    public void SetElementWeakness(ElementalProperties element)
    {
        throw new NotImplementedException();
    }
}
