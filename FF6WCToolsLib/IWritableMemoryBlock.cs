namespace FF6WCToolsLib;

public interface IWritableMemoryBlock
{
    public uint TargetAddress { get; }

    public byte[] ToByteArray();
}