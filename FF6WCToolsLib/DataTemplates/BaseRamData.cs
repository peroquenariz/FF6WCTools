namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseRamData : BaseData
{
    protected BaseRamData(int blockSize, int dataIndex) : base(new byte[blockSize], dataIndex) { }

    public void UpdateData(byte[] newData)
    {
        if (newData.Length != _data.Length) return; // Shouldn't happen, but just in case...

        _data = newData;
    }
}
