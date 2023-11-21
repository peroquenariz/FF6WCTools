namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseRomData : BaseData
{
    protected readonly byte[] _defaultData;

    public byte[] DefaultData => _defaultData;

    protected BaseRomData(byte[] data, int dataIndex) : base(data, dataIndex)
    {
        _defaultData = (byte[])data.Clone();
    }

    public void ResetData()
    {
        _data = (byte[])_defaultData.Clone();
    }
}
