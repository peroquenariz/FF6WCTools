using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents an item name's ROM block.
/// </summary>
public class ItemName : BaseNameRomData
{
    public static uint StartAddress => ITEM_NAMES_START;
    public static byte BlockSize => ITEM_NAMES_BLOCK_SIZE;
    public static int BlockCount => ITEM_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public ItemName(byte[] itemNameData, int itemNameIndex) : base(itemNameData, itemNameIndex) { }

    public override string ToString()
    {
        return DataHandler.ExtractName(_data[1..]); // Don't print the item icon
    }
}
