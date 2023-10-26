namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseData
{
    protected readonly byte[] _defaultData;
    protected byte[] _data;
    protected int _dataIndex;
    
    public abstract uint StartAddress { get; }
    public abstract byte BlockSize { get; }
    public abstract int BlockCount { get; }
    public uint TargetAddress => StartAddress + (uint)(BlockSize * _dataIndex);

    protected BaseData(byte[] data, int dataIndex)
    {
        _dataIndex = dataIndex;
        _defaultData = data;
        _data = (byte[])_defaultData.Clone();
    }
    
    public new abstract string ToString();

    public byte[] ToByteArray()
    {
        return _data;
    }

    public void ResetData()
    {
        _data = (byte[])_defaultData.Clone();
    }
}
