namespace FF6WCToolsLib;

public interface IReadableMemoryBlock
{
    /// <summary>
    /// Returns the target memory address to read from.
    /// </summary>
    public uint TargetAddress { get; }

    /// <summary>
    /// Returns the size of the data to read.
    /// </summary>
    /// <returns></returns>
    public uint GetDataSize();
}