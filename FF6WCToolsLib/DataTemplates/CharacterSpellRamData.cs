using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class CharacterSpellRamData : BaseRamData
{
    public static uint StartAddress => CHARACTER_SKILL_DATA_START;
    public static byte BlockSize => CHARACTER_SKILL_DATA_SPELL_BLOCK_SIZE;
    public static int BlockCount => CHARACTER_SKILL_DATA_BLOCK_COUNT; // Gogo and Umaro have no spells.
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public byte GetSpellLearnedData(int spellIndex)
    {
        return _data[spellIndex];
    }

    public uint GetSpellLearnedAddress(int spellIndex)
    {
        return (uint)(StartAddress + (BlockSize * _dataIndex) + spellIndex);
    }

    public CharacterSpellRamData(int index) : base(BlockSize, index) { }

    public override string ToString()
    {
        string description = string.Empty;
        for (byte i = 0; i < _data.Length; i++)
        {
            if (_data[i] == 0xFF)
            {
                description += SPELL_DICT[i];
            }
        }

        return description;
    }

    public static uint GetSpellAddress(Spell spell, Character character)
    {
        return (uint)(StartAddress + (byte)spell + ((byte)character * BlockSize));
    }
}
