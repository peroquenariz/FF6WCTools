using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class SpellEsperName : BaseName
{
    public static uint StartAddress => SPELLS_ALL_NAMES_START + SPELLS_ESPER_NAMES_OFFSET;
    public static byte BlockSize => SPELLS_ESPER_NAMES_BLOCK_SIZE;
    public static int BlockCount => SPELLS_ESPER_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellEsperName (byte[] nameData, int nameIndex) : base(nameData, nameIndex) { }
}