using System;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents an inventory slot, comprised of an item and a quantity.
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

    private bool _isEmpty;

    /// <summary>
    /// Current item in the slot.
    /// </summary>
    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item == Item.Empty)
            {
                _quantity = 0;
                _isEmpty = true;
            }
            else
            {
                _isEmpty = false;
            }
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
            if (_quantity == 0)
            {
                _item = Item.Empty;
                _isEmpty = true;
            }
            else
            {
                _isEmpty = false;
            }
        }
    }

    /// <summary>
    /// Returns true if the item slot is empty.
    /// </summary>
    public bool IsEmpty => _isEmpty;

    public InventorySlot(int slotIndex)
    {
        _slotIndex = (byte)slotIndex;
        _item = Item.Empty;
        _quantity = 0;
    }

    /// <summary>
    /// Gets the item index data from the inventory slot.
    /// </summary>
    /// <returns>A writable memory block with the item index data.</returns>
    public IWritableMemoryBlock GetItemIndexSlotData()
    {
        return new InventorySlotData(_item, _slotIndex);
    }

    /// <summary>
    /// Gets the item quantity data from the inventory slot.
    /// </summary>
    /// <returns>A writable memory block with the item quantity data.</returns>
    public IWritableMemoryBlock GetItemQuantitySlotData()
    {
        return new InventorySlotData(_quantity, _slotIndex);
    }
}
