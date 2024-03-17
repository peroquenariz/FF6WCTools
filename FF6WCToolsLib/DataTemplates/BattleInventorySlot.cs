using System;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class BattleInventorySlot : BaseInventorySlot, IWritableMemoryBlock
{
    [Flags]
    private enum BattleEquipability : byte
    {
        NONE = 0x0,
        CHAR1 = 0x1,
        CHAR2 = 0x2,
        CHAR3 = 0x4,
        CHAR4 = 0x8,
    }

    private byte _type;
    private byte _targeting;
    private byte _equipability;

    public uint TargetAddress => (uint)(BattleInventoryRamData.StartAddress + _indexValue * BattleInventoryRamData.BlockSize);

    public override ItemRomData Item
    {
        get => _item;
        set
        {
            base.Item = value;
            UpdateItemData();
        }
    }

    public override byte Quantity
    {
        get => _quantity;
        set
        {
            base.Quantity = value;
            if (_isEmpty) UpdateItemData();
        }
    }

    private void UpdateItemData()
    {
        byte[] itemData = _item.ToByteArray();
        _type = GetItemTypeBattleData(itemData);
        _targeting = itemData[(int)ItemDataStructure.Targeting];

        // Equipable actors are 2 bytes in item ROM data.
        // Extract the equipability to match the battle ram format (only considers characters currently in battle).
        int fullEquipability = DataHandler.ConcatenateByteArray
            (itemData[(int)ItemDataStructure.EquipableActorsLow..((int)ItemDataStructure.EquipableActorsHigh +1)]);
        _equipability = DataHandler.GetEquipability(fullEquipability);
    }

    private byte GetItemTypeBattleData(byte[] itemData)
    {
        BattleItemType flags = BattleItemType.NONE;
        
        if(_item.ItemType.HasFlag(ItemType.Weapon))
        {
            flags |= BattleItemType.IS_A_WEAPON | BattleItemType.CAN_BE_USED_WITH_JUMP;
        }
        else if(_item.ItemType.HasFlag(ItemType.Shield))
        {
            flags |= BattleItemType.IS_A_SHIELD;
        }
        else if (_item.ItemType.HasFlag(ItemType.Tool))
        {
            flags |= BattleItemType.USEABLE_WITH_TOOLS;
        }

        if (_item.ItemTypeFlags.HasFlag(ItemTypeFlags.CAN_BE_THROWN))
        {
            flags |= BattleItemType.CAN_BE_THROWN;
        }
        if (!_item.ItemTypeFlags.HasFlag(ItemTypeFlags.USABLE_IN_BATTLE))
        {
            // Bit is reversed in battle (NOT usable in battle)
            // Thanks Square for being consistent with your code :)
            flags |= BattleItemType.NOT_USABLE_IN_BATTLE;
        }

        return (byte)flags;
    }

    public byte[] ToByteArray()
    {
        return new byte[]
        {
            (byte)_item.Index,
            _type,
            _targeting,
            _quantity,
            _equipability
        };
    }
}