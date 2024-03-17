using static FF6WCToolsLib.WCData;
using System;

namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseInventorySlot
{
    protected int _indexValue;
    protected ItemRomData _item;
    protected byte _quantity;
    protected bool _isEmpty;

    /// <summary>
    /// Current item in the slot.
    /// </summary>
    public ItemRomData Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item.Index == (byte)WCData.Item.Empty)
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
                _item = ItemRomData.Empty;
                _isEmpty = true;
            }
            else
            {
                _isEmpty = false;
            }
        }
    }

    public bool IsEmpty => _isEmpty;
    public int Index { get => _indexValue; set { _indexValue = value; } }
    protected BaseInventorySlot()
    {
        _item = ItemRomData.Empty;
        _quantity = 0;
    }
}