﻿using System;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class SpellEsperAttackName : BaseData
{
    public static uint StartAddress => SPELLS_ALL_NAMES_START + SPELLS_ESPER_ATTACK_NAMES_OFFSET;
    public static byte BlockSize => SPELLS_ESPER_ATTACK_NAMES_BLOCK_SIZE;
    public static int BlockCount => SPELLS_ESPER_ATTACK_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellEsperAttackName(byte[] nameData, int nameIndex) : base(nameData, nameIndex) { }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}