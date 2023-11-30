namespace FF6WCToolsLib.DataTemplates;

public abstract class BaseNameRomData : BaseRomData
{
    protected BaseNameRomData(byte[] data, int dataIndex) : base(data, dataIndex) { }

    /// <summary>
    /// Mirrors the name.
    /// </summary>
    /// <param name="hasIcon">Specifies if the name has an icon (item, spell). Defaults to false.</param>
    public void Mirror(bool hasIcon = false)
    {
        byte[] mirroredData = new byte[_data.Length];
        int startIndex = 0;
        
        if (hasIcon)
        {
            startIndex++; // Offset 1 to the right.
            mirroredData[0] = _data[0]; // Don't mirror the icon!
        }

        for (int i = startIndex; i < _data.Length; i++)
        {
            mirroredData[i] = _data[_data.Length - i];
        }
        
        _data = mirroredData;
    }

    /// <summary>
    /// Changes the name.
    /// </summary>
    /// <param name="newNameData"></param>
    /// <param name="hasIcon">Specifies if the name has an icon (item, spell). Defaults to false.</param>
    public void Rename(byte[] newNameData, bool hasIcon = false)
    {
        int newNameLength = newNameData.Length;

        if (newNameLength == _data.Length)
        {
            _data = newNameData;
        }
        else if (newNameLength == (_data.Length - 1) && hasIcon)
        {
            for (int i = 0; i < newNameLength; i++)
            {
                _data[i + 1] = newNameData[i];
            }
        }
        else
        {
            int maxCharacterCount = newNameLength;
            
            if (newNameLength > _data.Length)
            {
                maxCharacterCount = _data.Length;
            }

            for (int i = 0; i < maxCharacterCount; i++)
            {
                _data[i] = newNameData[i];
            }
        }
    }

    public override string ToString()
    {
        return DataHandler.ExtractName(_data);
    }
}