using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class CharacterData : BaseRamData
{
    public static uint StartAddress => CHARACTER_DATA_START;
    public static byte BlockSize => CHARACTER_DATA_BLOCK_SIZE;
    public static int BlockCount => CHARACTER_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;
    
    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    public CharacterData(int characterIndex) : base(BlockSize, characterIndex) { }

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
            $"Weapon: {ITEM_DICT[_data[(int)CharacterDataStructure.Weapon]]}\n" +
            $"Shield: {ITEM_DICT[_data[(int)CharacterDataStructure.Shield]]}\n" +
            $"Helmet: {ITEM_DICT[_data[(int)CharacterDataStructure.Helmet]]}\n" +
            $"Armor: {ITEM_DICT[_data[(int)CharacterDataStructure.Armor]]}\n" +
            $"Relic1: {ITEM_DICT[_data[(int)CharacterDataStructure.Relic1]]}\n" +
            $"Relic2: {ITEM_DICT[_data[(int)CharacterDataStructure.Relic2]]}\n";
        
        return characterDescription;
    }
}