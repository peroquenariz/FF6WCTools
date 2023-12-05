using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// Represents a character's spell data RAM block.
/// Only the magic spells are stored here.
/// Lores/blitzes/swdtechs are located after all the magical spells and they're global.
/// </summary>
public class CharacterSpellRamData : BaseRamData
{
    public static uint StartAddress => CHARACTER_SKILL_DATA_START;
    public static byte BlockSize => CHARACTER_SKILL_DATA_SPELL_BLOCK_SIZE;
    public static int BlockCount => CHARACTER_SKILL_DATA_BLOCK_COUNT; // Gogo and Umaro have no spells.
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public byte GetSpellLearnedData(uint spellIndex)
    {
        return _data[spellIndex];
    }

    public uint GetSpellLearnedAddress(uint spellIndex)
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

    /// <summary>
    /// Gets the memory address of a spell.
    /// </summary>
    /// <param name="spell">The spell offset.</param>
    /// <param name="character">The character offset.</param>
    /// <returns>The memory address of the spell.</returns>
    public static uint GetSpellAddress(Spell spell, Character character)
    {
        return (uint)(StartAddress + (byte)spell + ((byte)character * BlockSize));
    }
}
