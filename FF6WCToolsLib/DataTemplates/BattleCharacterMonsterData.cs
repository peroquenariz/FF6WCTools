using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class BattleCharacterMonsterData : BaseRamData
{
    private static byte[]? _currentLineupDataValues;
    
    public static uint StartAddress => BATTLE_CHARACTER_MONSTER_DATA_START;
    public static byte BlockSize => BATTLE_CHARACTER_MONSTER_DATA_BLOCK_SIZE;
    public static int BlockCount => BATTLE_CHARACTER_MONSTER_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;
    
    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);
    public static byte[]? CurrentLineupData
    {
        get => _currentLineupDataValues;
        set
        {
            _currentLineupDataValues = value;
        }
    }
    
    public BattleCharacterMonsterData(int blockSize, int dataIndex) : base(blockSize, dataIndex) { }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
