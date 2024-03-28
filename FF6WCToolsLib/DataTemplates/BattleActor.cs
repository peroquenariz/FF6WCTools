using static FF6WCToolsLib.DataTemplates.DataEnums;

namespace FF6WCToolsLib.DataTemplates;

public class BattleActor
{
    private readonly int _index;
    private readonly byte[] _data;

    public BattleActor(int index)
    {
        _data = new byte[BattleActorData.BlockSize];
        _index = index;
    }

    public uint GetActorDataAddress(int actorIndex, int dataOffset)
    {
        return BattleActorData.StartAddress + (uint)GetActorDataIndex(actorIndex, dataOffset);
    }

    private int GetActorDataIndex(int actorIndex, int dataOffset)
    {
        return (actorIndex * 2) + (20 * (dataOffset / 2)) + (dataOffset % 2);
    }

    public void UpdateData(byte[] battleActorData)
    {
        //var actorDataTypes = Enum.GetValues<BattleActorDataStructure>();
        for (int i = 0; i < _data.Length; i++)
        {
            //int dataTypeOffset = (int)actorDataTypes[i];
            int dataIndex = GetActorDataIndex(_index, i);
            _data[i] = battleActorData[dataIndex];
        }
    }

    public override string ToString()
    {
        string description = string.Empty;

        for (int i = 0; i < _data.Length; i++)
        {
            description += $"{(BattleActorDataStructure)i}: {_data[i]} | ".PadRight(35);
            if (i % 3 == 0)
            {
                description += "\n";
            }
        }

        return description;
    }
}
