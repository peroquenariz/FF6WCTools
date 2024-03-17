using System.Collections.Generic;
using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class BattleInventoryRamData : BaseInventory<BattleInventorySlot>
{
    public static uint StartAddress => BATTLE_INVENTORY_START;

    public static uint BlockCount => BATTLE_INVENTORY_BLOCK_COUNT;
    public static byte BlockSize => BATTLE_INVENTORY_BLOCK_SIZE;
    public static uint DataSize => BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress;

    public BattleInventoryRamData() : base((int)BlockCount, (int)DataSize) { }

    protected override void UpdateInventorySlots(List<ItemRomData> itemList)
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            if (i == 255) continue; // Skip inaccessible item slot.
            
            BattleInventorySlot slot = _inventorySlots[i];
            slot.Item = itemList[_data[i * BlockSize]];
            slot.Quantity = _data[i * BlockSize + (int)BattleItemDataStructure.Quantity];
        }
    }
}
