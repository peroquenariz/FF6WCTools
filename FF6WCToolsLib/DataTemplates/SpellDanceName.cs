using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class SpellDanceName : BaseNameRomData
{
    public static uint StartAddress => SPELLS_ALL_NAMES_START + SPELLS_DANCE_NAMES_OFFSET;
    public static byte BlockSize => SPELLS_DANCE_NAMES_BLOCK_SIZE;
    public static int BlockCount => SPELLS_DANCE_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellDanceName(byte[] nameData, int nameIndex) : base(nameData, nameIndex) { }
}