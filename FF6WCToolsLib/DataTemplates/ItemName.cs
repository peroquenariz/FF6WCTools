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

    /// <summary>
    /// Mirrors the name of the item.
    /// </summary>
    public void Mirror()
    {
        byte[] mirroredData = new byte[13];
        mirroredData[0] = _data[0]; // Keep the item icon in the right place
        for (int i = 1; i < _data.Length; i++)
        {
            mirroredData[i] = _data[_data.Length - i];
        }
        _data = mirroredData;
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
