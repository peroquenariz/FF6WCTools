using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib.DataTemplates;

public class SpellData : BaseData
{
    private byte _targeting;
    private byte _elementalProperties;
    private byte _SpecialFlags1;
    private byte _SpecialFlags2;
    private byte _SpecialFlags3;
    private byte _mpCost;
    private byte _spellPower;
    private byte _SpecialFlags4;
    private byte _hitRate;
    private byte _specialEffect;
    private byte _status1;
    private byte _status2;
    private byte _status3;
    private byte _status4;

    

    public SpellData(byte[] data, int spellIndex)
    {
        
    }

    public override uint DataStartAddress => throw new System.NotImplementedException();

    public override byte DataBlockSize => throw new System.NotImplementedException();

    public override int DataBlockCount => throw new System.NotImplementedException();

    public override byte[] ToByteArray() // TODO: make enum
    {
        // Implementation (maybe useless? probably easy to just return the whole thing)
        // It's just a few bytes, anyways...
        // Default: return the entire array
        // Optionally, use parameter to only return a byte[1] with the 
        
        return new byte[] { }; // Full spell data
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
