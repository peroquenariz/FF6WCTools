using System.Collections.Generic;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents the FF6 inventory memory.
/// Two 256 bytes sections, one for item indexes, the other one for item quantities.
/// </summary>
public class InventoryRamData : BaseRamData
{
    public static uint StartAddress => INVENTORY_START;
    
    /// <summary>
    /// 255 item slots + 1 byte that's always empty. Maybe a separator? Only Square knows.
    /// Slot 256 is item index 0xFF and item quantity 0x00.
    /// </summary>
    public static uint BlockSize => INVENTORY_SIZE+1;
    public static uint DataSize => BlockSize * 2;

    public override uint TargetAddress => StartAddress;

    private readonly List<InventorySlot> _inventorySlotList;

    public InventorySlot this[int index]
    {
        get => _inventorySlotList[index];
    }

    public InventoryRamData() : base((int)DataSize, 0)
    {
        _inventorySlotList = InitializeInventorySlots();
    }

    private List<InventorySlot> InitializeInventorySlots()
    {
        List<InventorySlot> inventory = new();

        for (int i = 0; i < BlockSize; i++)
        {
            inventory.Add(new InventorySlot(i));
        }

        return inventory;
    }

    public override void UpdateData(byte[] newData)
    {
        base.UpdateData(newData);
        UpdateInventorySlots();
    }

    private void UpdateInventorySlots()
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            InventorySlot slot = _inventorySlotList[i];
            slot.Item = (Item)_data[i];
            slot.Quantity = _data[i + BlockSize];
        }
    }

    public override string ToString()
    {
        string inventory = string.Empty;

        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            InventorySlot inventorySlot = _inventorySlotList[i];

            if (inventorySlot.Item == Item.Empty) continue; // Skip empty items.

            inventory += $"{inventorySlot.Item}: {inventorySlot.Quantity}\n";
        }

        return inventory;
    }
}