﻿using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents a spell attack name's ROM block.
/// </summary>
public class SpellAttackName : BaseNameRomData
{
    public static uint StartAddress => SPELLS_ALL_NAMES_START + SPELLS_ATTACK_NAMES_OFFSET;
    public static byte BlockSize => SPELLS_ATTACK_NAMES_BLOCK_SIZE;
    public static int BlockCount => SPELLS_ATTACK_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellAttackName(byte[] nameData, int nameIndex) : base(nameData, nameIndex) { }
}