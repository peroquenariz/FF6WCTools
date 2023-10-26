namespace FF6WCToolsLib.DataTemplates;

public interface IWritableMemoryBlock
{
    public uint TargetAddress { get; }

    public byte[] ToByteArray();
}