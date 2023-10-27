namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseData : IWritableMemoryBlock
{
    protected readonly byte[] _defaultData;
    protected byte[] _data;
    protected int _dataIndex;

    public abstract uint TargetAddress { get; }

    protected BaseData(byte[] data, int dataIndex)
    {
        _dataIndex = dataIndex;
        _defaultData = data;
        _data = (byte[])_defaultData.Clone();
    }
    
    public new abstract string ToString();

    public byte[] ToByteArray()
    {
        return (byte[])_data.Clone();
    }

    public void ResetData()
    {
        _data = (byte[])_defaultData.Clone();
    }
}
