using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents a byte of the inventory data.
/// </summary>
public readonly struct InventorySlotData : IWritableMemoryBlock
{
    private readonly int _slotIndex;
    private readonly uint _offset;
    private readonly byte _data;
    public uint TargetAddress => InventoryRamData.StartAddress + (uint)_slotIndex + _offset;

    /// <summary>
    /// Quantity data offsets the address by the size of the inventory item indexes (256 bytes).
    /// </summary>
    public InventorySlotData(byte itemQuantity, int slotIndex)
    {
        _slotIndex = slotIndex;
        _offset = InventoryRamData.BlockCount;
        _data = itemQuantity;
    }

    public InventorySlotData(Item item, int slotIndex)
    {
        _slotIndex = slotIndex;
        _offset = 0;
        _data = (byte)item;
    }

    public byte[] ToByteArray()
    {
        return new byte[] { _data };
    }
}