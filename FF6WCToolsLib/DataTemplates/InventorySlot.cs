using System;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents an inventory slot.
/// </summary>
public class InventorySlot
{
    /// <summary>
    /// Quantity zero should only be used for Empty items.
    /// </summary>
    private const byte ITEM_QUANTITY_MIN_VALUE = 0;
    private const byte ITEM_QUANTITY_MAX_VALUE = 99;

    private readonly byte _slotIndex;
    private Item _item;
    private byte _quantity;

    /// <summary>
    /// Current item in the slot.
    /// </summary>
    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item == Item.Empty) _quantity = 0;
        }
    }

    /// <summary>
    /// Current quantity of items in the slot.
    /// </summary>
    public byte Quantity
    {
        get => _quantity;
        set
        {
            _quantity = Math.Clamp(value, ITEM_QUANTITY_MIN_VALUE, ITEM_QUANTITY_MAX_VALUE);
            if (_quantity == 0) _item = Item.Empty;
        }
    }

    public InventorySlot(int slotIndex)
    {
        _slotIndex = (byte)slotIndex;
        _item = Item.Empty;
        _quantity = 0;
    }

    public IWritableMemoryBlock GetItemIndexSlotData()
    {
        return new InventorySlotData(_item, _slotIndex);
    }

    public IWritableMemoryBlock GetItemQuantitySlotData()
    {
        return new InventorySlotData(_quantity, _slotIndex);
    }
}
