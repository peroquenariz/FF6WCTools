using System;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class BattleActorData : BaseRamData
{
    private static byte[]? _currentLineupDataValues;
    private readonly BattleActor[] _battleActors;

    /// <summary>
    /// Characters in battle.
    /// Will return null if not in battle.
    /// Update this data every time you start a battle and set to null on battle end.
    /// </summary>
    public static byte[]? CurrentLineupData
    {
        get => _currentLineupDataValues;
        set
        {
            _currentLineupDataValues = value;
        }
    }
    public static uint StartAddress => BATTLE_CHARACTER_MONSTER_DATA_START;
    public static byte BlockSize => BATTLE_CHARACTER_MONSTER_DATA_BLOCK_SIZE;
    public static int BlockCount => BATTLE_CHARACTER_MONSTER_DATA_BLOCK_COUNT;
    public static uint DataSize => (uint)BlockCount * BlockSize;
    
    public override uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);
    
    public BattleActorData() : base()
    {
        _battleActors = InitializeActors();
    }
    
    public BattleActor this[int index]
    {
        get => _battleActors[index];
    }

    private BattleActor[] InitializeActors()
    {
        var battleActors = new BattleActor[BlockCount];

        for (int i = 0; i < BlockCount; i++)
        {
            battleActors[i] = new BattleActor(i);
        }
        
        return battleActors;
    }

    public override void UpdateData(byte[] newData)
    {
        for (int i = 0; i < _battleActors.Length; i++)
        {
            _battleActors[i].UpdateData(newData);
        }
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public override uint GetDataSize()
    {
        return DataSize;
    }
}
