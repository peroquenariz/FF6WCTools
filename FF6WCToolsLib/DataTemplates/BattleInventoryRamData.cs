using System.Collections.Generic;
using static FF6WCToolsLib.WCData;

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
        throw new System.NotImplementedException();
    }
}
