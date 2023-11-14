namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseRamData : IWritableMemoryBlock
{
    protected byte[] _data;
    protected int _dataIndex;

    public int Index => _dataIndex;
    public abstract uint TargetAddress { get; }

    protected BaseRamData(int blockSize, int dataIndex)
    {
        _dataIndex = dataIndex;
        _data = new byte[blockSize];
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

    public void UpdateData(byte[] newData)
    {
        if (newData.Length != _data.Length) return; // Shouldn't happen, but just in case...

        _data = newData;
    }
}
