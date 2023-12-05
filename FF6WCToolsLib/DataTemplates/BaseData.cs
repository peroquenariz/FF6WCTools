namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// The base class for all FF6WC data.
/// It consists of a byte array that holds the data, and an index.
/// </summary>
public abstract class BaseData : IWritableMemoryBlock
{
    protected byte[] _data;
    protected int _dataIndex;

    public int Index => _dataIndex;
    public abstract uint TargetAddress { get; }

    protected BaseData(byte[] data, int dataIndex)
    {
        _data = data;
        _dataIndex = dataIndex;
    }

    /// <summary>
    /// Provides a text representation of the stored data.
    /// </summary>
    public new abstract string ToString();

    public byte[] ToByteArray()
    {
        return _data;
    }
}