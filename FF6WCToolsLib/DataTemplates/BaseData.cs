namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseData : IWritableMemoryBlock
{
    protected byte[] _data;
    protected int _dataIndex;

    public int Index => _dataIndex;
    public abstract uint TargetAddress { get; }

    public BaseData(byte[] data, int dataIndex)
    {
        _data = data;
        _dataIndex = dataIndex;
    }

    /// <summary>
    /// Code spaghetti for debugging game data.
    /// </summary>
    /// <returns></returns>
    public new abstract string ToString();

    public byte[] ToByteArray()
    {
        return _data;
    }
}