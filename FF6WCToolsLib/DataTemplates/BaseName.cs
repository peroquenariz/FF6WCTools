namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseName : BaseData
{
    protected BaseName(byte[] data, int dataIndex) : base(data, dataIndex) { }

    public override string ToString()
    {
        return DataHandler.ExtractName(_data);
    }
}