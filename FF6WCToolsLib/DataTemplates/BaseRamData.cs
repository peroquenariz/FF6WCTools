namespace FF6WCToolsLib.DataTemplates;

/// <summary>
/// The base class for all FF6WC RAM data.
/// RAM data initializes empty, and gets updated every time data needs to be modified.
/// </summary>
public abstract class BaseRamData : BaseData, IReadableMemoryBlock
{
    protected BaseRamData(int blockSize, int dataIndex) : base(new byte[blockSize], dataIndex) { }

    /// <summary>
    /// Updates the data block.
    /// </summary>
    /// <param name="newData">A byte array with the new data.</param>
    public virtual void UpdateData(byte[] newData)
    {
        if (newData.Length != _data.Length) return; // Shouldn't happen, but just in case...

        _data = newData;
    }

    public virtual uint GetDataSize()
    {
        return (uint)_data.Length;
    }
}
