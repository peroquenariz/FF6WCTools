using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;

namespace FF6WCToolsLib.DataTemplates;

public class CharacterData : BaseRamData
{
    private const int MIN_STAT_VALUE = 10; // TODO: expose these in a config file.
    private const int MAX_STAT_VALUE = 90; // Keep in mind these stat values are PRE-ITEM BUFFS!

    public static uint StartAddress => CHARACTER_DATA_START;
    public static byte BlockSize => CHARACTER_DATA_BLOCK_SIZE;
    public static int BlockCount => CHARACTER_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;
    
    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public CharacterData(int characterIndex) : base(BlockSize, characterIndex) { }
    
    public byte[] RenameCharacter(byte[] characterName)
    {
        for (int i = 0; i < 6; i++)
        {
            _data[(int)CharacterDataStructure.Name + i] = characterName[i];
        }
        
        return _data[(int)CharacterDataStructure.Name..((int)CharacterDataStructure.Name + 6)];
    }

    public override string ToString()
    {
        string characterDescription =
            $"Actor index: {_data[(int)CharacterDataStructure.ActorIndex]}\n" +
            $"Graphic index: {_data[(int)CharacterDataStructure.GraphicIndex]}\n" +
            $"Name: {DataHandler.ExtractName(_data[(int)CharacterDataStructure.Name..(int)(CharacterDataStructure.Name + 6)])}\n" +
            $"Level: {_data[(int)CharacterDataStructure.Level]}\n" +
            $"Current HP: {DataHandler.ConcatenateByteArray(_data[(int)CharacterDataStructure.CurrentHP..(int)(CharacterDataStructure.CurrentHP + 2)])}\n" +
            $"Current MP: {DataHandler.ConcatenateByteArray(_data[(int)CharacterDataStructure.CurrentMP..(int)(CharacterDataStructure.CurrentMP + 2)])}\n" +
            $"Experience points: {DataHandler.ConcatenateByteArray(_data[(int)CharacterDataStructure.ExperiencePoints..(int)(CharacterDataStructure.ExperiencePoints + 3)])}\n" +
            $"Status 1: {(StatusCondition1)_data[(int)CharacterDataStructure.Status1]}\n" +
            $"Status 4: {(StatusCondition1)_data[(int)CharacterDataStructure.Status4]}\n" +
            $"Vigor: {_data[(int)CharacterDataStructure.Vigor]}\n" +
            $"Speed: {_data[(int)CharacterDataStructure.Speed]}\n" +
            $"Stamina: {_data[(int)CharacterDataStructure.Stamina]}\n" +
            $"Mag. Power: {_data[(int)CharacterDataStructure.MagPower]}\n" +
            $"Esper: {ESPER_DICT[_data[(int)CharacterDataStructure.Esper]]}\n" +
            $"Weapon: {ITEM_DICT[_data[(int)CharacterDataStructure.RHand]]}\n" +
            $"Shield: {ITEM_DICT[_data[(int)CharacterDataStructure.LHand]]}\n" +
            $"Helmet: {ITEM_DICT[_data[(int)CharacterDataStructure.Helmet]]}\n" +
            $"Armor: {ITEM_DICT[_data[(int)CharacterDataStructure.Armor]]}\n" +
            $"Relic1: {ITEM_DICT[_data[(int)CharacterDataStructure.Relic1]]}\n" +
            $"Relic2: {ITEM_DICT[_data[(int)CharacterDataStructure.Relic2]]}\n";
        
        return characterDescription;
    }

    public static uint GetEquipmentAddress(Character character, EquipmentSlot equipmentSlot)
    {
        int slot = (int)CharacterDataStructure.RHand + (int)equipmentSlot;
        return (uint)(StartAddress + ((byte)character * BlockSize) + slot);
    }

    public void SetStatBoost(Stat stat, int statBoostValue)
    {
        int statIndex = (int)CharacterDataStructure.Vigor + (int)stat;
        
        // Apply the stat boost.
        int newStatValue = _data[statIndex] + statBoostValue;

        // Clamp the value to the given range.
        newStatValue = Math.Clamp(newStatValue, MIN_STAT_VALUE, MAX_STAT_VALUE);

        // Save the boosted stat in the character data.
        _data[statIndex] = (byte)newStatValue;
    }
}