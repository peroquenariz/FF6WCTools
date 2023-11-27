namespace FF6WCToolsLib.DataTemplates;

public interface IReadableMemoryBlock
{
    public uint TargetAddress { get; }

    public byte GetBlockSize();
}