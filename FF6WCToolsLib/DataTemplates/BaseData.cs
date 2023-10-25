using System;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseData
{
    public abstract uint DataStartAddress { get; }
    public abstract byte DataBlockSize { get; }
    public abstract int DataBlockCount { get; }

    public abstract byte[] ToByteArray();

    public new abstract string ToString();
}
