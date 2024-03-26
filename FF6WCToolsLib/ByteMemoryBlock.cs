namespace FF6WCToolsLib;

/// <summary>
/// Simple structure for writing data to memory.
/// </summary>
public readonly struct ByteMemoryBlock : IWritableMemoryBlock
{
    private readonly byte[] _data;
    private readonly uint _address;

    public uint TargetAddress => _address;

    public ByteMemoryBlock(byte[] data, uint address)
    {
        _data = data;
        _address = address;
    }

    public ByteMemoryBlock(byte data, uint address)
    {
        _data = new byte[] { data };
        _address = address;
    }

    public byte[] ToByteArray()
    {
        return _data;
    }
}
