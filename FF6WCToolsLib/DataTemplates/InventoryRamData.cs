using System.Collections.Generic;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents the inventory's RAM block.
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

    /// <summary>
    /// Instantiates all inventory slot objects.
    /// </summary>
    /// <returns>A list containing all the inventory slots.</returns>
    private static List<InventorySlot> InitializeInventorySlots()
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

    /// <summary>
    /// Updates all inventory slots with new items.
    /// </summary>
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

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <param name="addQuantity">The quantity of items to add.</param>
    /// <param name="wasEmpty">True if the slot was empty before adding the item, otherwise false.</param>
    /// <returns>The inventory slot on which the item was added, or null if there was no room in the inventory.</returns>
    public InventorySlot? AddItem(Item item, byte addQuantity, out bool wasEmpty)
    {
        InventorySlot? emptySlot = null;
        InventorySlot? targetSlot = null;
        wasEmpty = false;

        foreach (var slot in _inventorySlotList)
        {
            // Save the first empty slot, in case the item doesn't exist in the inventory.
            if (emptySlot == null && slot.IsEmpty)
            {
                emptySlot = slot;
            }

            // If the item exists in the inventory, get the slot and break the loop.
            if (slot.Item == item)
            {
                targetSlot = slot;
                break;
            }
        }

        // If item exists, increase the quantity and return the slot.
        if (targetSlot != null)
        {
            targetSlot.Quantity += addQuantity;
            return targetSlot;
        }
        // If item doesn't exist and there is inventory space available,
        // add the item to the first empty slot available and return it.
        else if (emptySlot != null)
        {
            emptySlot.Item = item;
            emptySlot.Quantity += addQuantity;
            wasEmpty = true;
            return emptySlot;
        }
        // In the extremely rare circumstance the inventory is full and the item wasn't found, return null.
        // This shouldn't happen unless there are multiple slots with the same item.
        else return null;
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <param name="removeQuantity">The quantity of items to remove.</param>
    /// <param name="isEmpty">True if the slot is emptied after item removal, otherwise false.</param>
    /// /// <returns>The inventory slot on which the item was removed, or null if the item wasn't found.</returns>
    public InventorySlot? RemoveItem(Item item, byte removeQuantity, out bool isEmpty)
    {
        isEmpty = false;

        foreach (var slot in _inventorySlotList)
        {
            // If the item exists in the inventory, subtract from the quantity.
            if (slot.Item == item)
            {
                slot.Quantity -= removeQuantity;
                isEmpty = slot.IsEmpty;
                return slot;
            }
        }

        return null;
    }
}