using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class EsperData : BaseRomData
{
    public static uint StartAddress => ESPER_DATA_START;
    public static byte BlockSize => ESPER_DATA_BLOCK_SIZE;
    public static int BlockCount => ESPER_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;

    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);
    
    public EsperData(byte[] esperData, int esperIndex) : base(esperData, esperIndex) { }

    public override string ToString()
    {
        string esperName = ESPER_DICT.ContainsKey((byte)_dataIndex) ? ESPER_DICT[(byte)_dataIndex] : "NOT_FOUND";
        string esperDescription =
            $"Spell name: {esperName}\n" + // TODO: replace with current name in memory!
            $"Spell 1 learn rate: {_data[(int)EsperDataStructure.Spell1LearnRate]}x\n" +
            $"Spell 1: {SPELL_DICT[_data[(int)EsperDataStructure.Spell1]]}\n" +
            $"Spell 2 learn rate: {_data[(int)EsperDataStructure.Spell2LearnRate]}x\n" +
            $"Spell 2: {SPELL_DICT[_data[(int)EsperDataStructure.Spell2]]}\n" +
            $"Spell 3 learn rate: {_data[(int)EsperDataStructure.Spell3LearnRate]}x\n" +
            $"Spell 3: {SPELL_DICT[_data[(int)EsperDataStructure.Spell3]]}\n" +
            $"Spell 4 learn rate: {_data[(int)EsperDataStructure.Spell4LearnRate]}x\n" +
            $"Spell 4: {SPELL_DICT[_data[(int)EsperDataStructure.Spell4]]}\n" +
            $"Spell 5 learn rate: {_data[(int)EsperDataStructure.Spell5LearnRate]}x\n" +
            $"Spell 5: {SPELL_DICT[_data[(int)EsperDataStructure.Spell5]]}\n" +
            $"Spell bonus: {(EsperLevelUpBonus)_data[(int)EsperDataStructure.LevelUpBonus]}\n";
        return esperDescription;
    }
}
