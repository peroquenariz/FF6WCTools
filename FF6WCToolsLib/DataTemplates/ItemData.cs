using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class ItemData : BaseData
{
    private ItemType _itemType;

    public override uint StartAddress => ITEM_DATA_START;
    public override byte BlockSize => ITEM_DATA_BLOCK_SIZE;
    public override int BlockCount => ITEM_DATA_BLOCK_COUNT;

    public ItemData(byte[] itemData, int itemIndex) : base(itemData, itemIndex)
    {
        _itemType = (ItemType)(itemData[0] & 0x07);
    }

    public override string ToString()
    {
        //string description = 
        //    "itemType\n" +
        //    "equipableActorsLow\n" +
        //    "equipableActorsHigh\n" +
        //    "spellLearnRate\n" +
        //    "spellToLearn\n" +
        //    "fieldEffect\n" +
        //    "statusProtection1\n" +
        //    "statusProtection2\n" +
        //    "equipmentStatus\n" +
        //    "itemFlags1\n" +
        //    "itemFlags2\n" +
        //    "itemFlags3\n" +
        //    "itemFlags4\n" +
        //    "itemFlags5\n" +
        //    "targeting\n" +
        //    "weaponElementOrHalveElement\n" +
        //    "vigorAndSpeed\n" +
        //    "staminaAndMagic\n" +
        //    "weaponSpellCasting\n" +
        //    "weaponOrItemFlags\n" +
        //    "weaponPower\n";
        string description = 
            $"Item original name: {ITEM_DICT[(byte)_dataIndex]}\n" +
            $"Item type: {_itemType}";
        return description;
    }
}
