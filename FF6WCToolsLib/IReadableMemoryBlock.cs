namespace FF6WCToolsLib;

public interface IReadableMemoryBlock
{
    public uint TargetAddress { get; }

    public byte GetBlockSize();
}