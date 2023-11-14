namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseRomData : IWritableMemoryBlock
{
    protected readonly byte[] _defaultData;
    protected byte[] _data;
    protected int _dataIndex;

    public int Index => _dataIndex;
    public abstract uint TargetAddress { get; }

    protected BaseRomData(byte[] data, int dataIndex)
    {
        _dataIndex = dataIndex;
        _defaultData = data;
        _data = (byte[])_defaultData.Clone();
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

    public void ResetData()
    {
        _data = (byte[])_defaultData.Clone();
    }
}
