namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// The base class for all FF6WC ROM data.
/// ROM data is initialized by reading the ROM before the run starts,
/// and storing the original version in the default data field.
/// Further modification is done on the BaseData._data field.
/// </summary>
public abstract class BaseRomData : BaseData
{
    protected readonly byte[] _defaultData;

    public byte[] DefaultData => _defaultData;

    protected BaseRomData(byte[] data, int dataIndex) : base(data, dataIndex)
    {
        _defaultData = (byte[])data.Clone();
    }

    /// <summary>
    /// Resets the ROM data to its original value.
    /// </summary>
    public void ResetData()
    {
        _data = (byte[])_defaultData.Clone();
    }
}
