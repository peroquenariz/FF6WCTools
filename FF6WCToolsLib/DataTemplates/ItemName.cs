using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class ItemName : BaseData
{
    public static uint StartAddress => ITEM_NAMES_START;
    public static byte BlockSize => ITEM_NAMES_BLOCK_SIZE;
    public static int BlockCount => ITEM_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public ItemName(byte[] itemNameData, int itemNameIndex) : base(itemNameData, itemNameIndex) { }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
