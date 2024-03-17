using System.Collections.Generic;

namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseInventory<T> : BaseRamData where T : BaseInventorySlot, new()
{
    protected readonly T[] _inventorySlots;

    protected BaseInventory(int blockCount, int dataSize) : base(dataSize, 0)
    {
        _inventorySlots = InitializeSlots(blockCount);
    }

    public T this[int index]
    {
        get => _inventorySlots[index];
    }

    /// <summary>
    /// Instantiates all inventory slots.
    /// </summary>
    /// <returns>An array containing all the inventory slots.</returns>
    private static T[] InitializeSlots(int blockCount)
    {
        T[] inventory = new T[blockCount];

        for (int i = 0; i < blockCount; i++)
        {
            T slot = new() { Index = i };
            inventory[i] = slot;
        }

        return inventory;
    }

    protected abstract void UpdateInventorySlots(List<ItemRomData> itemList);

    public void UpdateData(byte[] newData, List<ItemRomData> itemList)
    {
        base.UpdateData(newData);
        UpdateInventorySlots(itemList);
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <param name="amount">The quantity of items to add.</param>
    /// <param name="wasEmpty">True if the slot was empty before adding the item, otherwise false.</param>
    /// <returns>The inventory slot on which the item was added, or null if there was no room in the inventory.</returns>
    public T? AddItem(ItemRomData item, byte amount, out bool wasEmpty)
    {
        T? emptySlot = null;
        T? targetSlot = null;
        wasEmpty = false;

        foreach (var slot in _inventorySlots)
        {
            // Save the first empty slot, in case the item doesn't exist in the inventory.
            if (emptySlot == null && slot.IsEmpty)
            {
                emptySlot = slot;
            }

            // If the item exists in the inventory, get the slot and break the loop.
            if (slot.Item.Index == item.Index)
            {
                targetSlot = slot;
                break;
            }
        }

        // If item exists, increase the quantity and return the slot.
        if (targetSlot != null)
        {
            targetSlot.Quantity += amount;
            return targetSlot;
        }
        // If item doesn't exist and there is inventory space available,
        // add the item to the first empty slot available and return it.
        else if (emptySlot != null)
        {
            emptySlot.Item = item;
            emptySlot.Quantity += amount;
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
    /// <param name="amount">The quantity of items to remove.</param>
    /// <param name="isEmpty">True if the slot is emptied after item removal, otherwise false.</param>
    /// /// <returns>The inventory slot on which the item was removed, or null if the item wasn't found.</returns>
    public T? RemoveItem(ItemRomData item, byte amount, out bool isEmpty)
    {
        isEmpty = false;

        foreach (var slot in _inventorySlots)
        {
            // If the item exists in the inventory, subtract from the quantity.
            if (slot.Item.Index == item.Index)
            {
                slot.Quantity -= amount;
                isEmpty = slot.IsEmpty;
                return slot;
            }
        }

        return null;
    }
}
