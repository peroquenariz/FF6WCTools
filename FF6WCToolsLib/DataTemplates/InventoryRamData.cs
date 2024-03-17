using System.Collections.Generic;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents the inventory's RAM block.
/// Two 256 bytes sections, one for item indexes, the other one for item quantities.
/// </summary>
public class InventoryRamData : BaseInventory<InventorySlot>
{
    public static uint StartAddress => INVENTORY_START;
    
    /// <summary>
    /// 255 item slots + 1 slot that's always empty.
    /// Slot 256 is inaccessible in-game due to a bugfix in the US version (see FF6J equip anything bug),
    /// but it's still accounted for in the ram to preserve the indexes correctly.
    /// Slot 256 is always an empty item (item index 0xFF and item quantity 0x0).
    /// </summary>
    public static uint BlockCount => INVENTORY_SIZE+1;
    public static byte BlockSize => 2; // Indexes and quantities
    public static uint DataSize => BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress;

    public InventoryRamData() : base((int)BlockCount, (int)DataSize) { }

    /// <summary>
    /// Updates all inventory slots with new items.
    /// </summary>
    protected override void UpdateInventorySlots(List<ItemRomData> itemList)
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            InventorySlot slot = _inventorySlots[i];
            slot.Item = itemList[_data[i]];
            slot.Quantity = _data[i + BlockCount];
        }
    }

    public override string ToString()
    {
        string inventory = string.Empty;

        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            InventorySlot inventorySlot = _inventorySlots[i];

            if (inventorySlot.IsEmpty) continue; // Skip empty items.

            inventory += $"{inventorySlot.Item}: {inventorySlot.Quantity}\n";
        }

        return inventory;
    }
}