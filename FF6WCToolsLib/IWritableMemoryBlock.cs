namespace FF6WCToolsLib;

public interface IWritableMemoryBlock
{
    /// <summary>
    /// Returns the target memory address to write to.
    /// </summary>
    public uint TargetAddress { get; }

    /// <summary>
    /// Returns the data to write.
    /// </summary>
    /// <returns></returns>
    public byte[] ToByteArray();
}