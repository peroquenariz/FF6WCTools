namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents an inventory slot, comprised of an item and a quantity.
/// </summary>
public class InventorySlot : BaseInventorySlot
{
    public InventorySlot() : base() { }

    /// <summary>
    /// Gets the item index data from the inventory slot.
    /// </summary>
    /// <returns>A writable memory block with the item index data.</returns>
    public IWritableMemoryBlock GetItemIndexSlotData()
    {
        return new InventorySlotData((WCData.Item)(byte)_item.Index, _indexValue);
    }

    /// <summary>
    /// Gets the item quantity data from the inventory slot.
    /// </summary>
    /// <returns>A writable memory block with the item quantity data.</returns>
    public IWritableMemoryBlock GetItemQuantitySlotData()
    {
        return new InventorySlotData(_quantity, _indexValue);
    }
}
