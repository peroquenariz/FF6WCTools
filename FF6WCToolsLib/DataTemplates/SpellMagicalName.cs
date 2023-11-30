using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class SpellMagicalName : BaseNameRomData
{
    // No need to add an offset to this one, it's just for naming consistency and OCD :)
    public static uint StartAddress => SPELLS_ALL_NAMES_START + SPELLS_MAGICAL_NAMES_OFFSET;
    public static byte BlockSize => SPELLS_MAGICAL_NAMES_BLOCK_SIZE;
    public static int BlockCount => SPELLS_MAGICAL_NAMES_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public SpellMagicalName(byte[] nameData, int nameIndex) : base(nameData, nameIndex) { }

    public override string ToString()
    {
        return DataHandler.ExtractName(_data[1..]); // Don't print the spell icon
    }
}
