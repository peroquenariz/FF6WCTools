using System;
using static FF6WCToolsLib.WCData;

namespace CrowdControlLib;

public class CrowdControlArgs
{
    private const string VALID_NAME_CHARACTERS = // Characters available in the rename screen
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?/:\"\'-.0123456789 ";
    
    private bool _isValid;
    private string _errorMessage;

    private const int GP_AMOUNT_MIN = -10000; // TODO: expose these two in a config file
    private const int GP_AMOUNT_MAX = 10000;

    private readonly Effect _effectType;
    private WindowEffect _windowEffect;
    private GPEffect _gpEffect;
    private Character _character;
    private string _newCharacterName;
    private Item _item;
    private string _newItemName;

    public bool IsValid => _isValid;
    public string ErrorMessage => _errorMessage;
    public Effect EffectType => _effectType;
    public WindowEffect WindowEffect => _windowEffect;
    public GPEffect GPEffect => _gpEffect;
    public int GPAmount { get; set; }
    public Character Character => _character;
    public string NewCharactername => _newCharacterName;
    public Item Item => _item;
    public string NewItemName => _newItemName;

    public CrowdControlArgs(string message)
    {
        _errorMessage = string.Empty;
        _newCharacterName = string.Empty;
        _newItemName = string.Empty;

        string[] splitMessage = message.Split(" ");

        Enum.TryParse(splitMessage[0], true, out _effectType);

        switch (_effectType)
        {
            case Effect._INVALID:
                _errorMessage = $"Invalid crowd control effect '{splitMessage[0]}'!";
                return;
            case Effect.item:
                break;
            case Effect.spell:
                break;
            case Effect.character:
                break;
            case Effect.inventory:
                break;
            case Effect.itemname:
                SetItemNameArgs(splitMessage);
                break;
            case Effect.spellname:
                break;
            case Effect.charactername:
                SetCharacterNameArgs(splitMessage);
                break;
            case Effect.gp:
                SetGPArgs(splitMessage);
                break;
            case Effect.window:
                SetWindowArgs(splitMessage);
                break;
            case Effect.mirror:
                _isValid = true;
                break;
            default:
                break;
        }
    }

    private void SetItemNameArgs(string[] splitMessage)
    {
        if (splitMessage.Length <= 1)
        {
            _errorMessage = "Item not specified!";
            return;
        }
        bool isValidItem = Enum.TryParse(splitMessage[1], true, out _item);

        if (!isValidItem)
        {
            _errorMessage = $"Item '{splitMessage[1]}' invalid!";
            return;
        }

        // Concatenate item name.
        for (int i = 2; i < splitMessage.Length; i++)
        {
            _newItemName += splitMessage[i] + " ";
        }

        // Remove trailing whitespaces.
        _newItemName = _newItemName.TrimEnd();

        // Check for invalid characters
        bool isValidName = IsNameValid(_newItemName, ITEM_NAMES_BLOCK_SIZE - 1);
        if (!isValidName)
        {
            _errorMessage = $"Item name {_newItemName} invalid!";
            return;
        }
        _isValid = isValidItem;
    }

    private void SetWindowArgs(string[] splitMessage)
    {
        if (splitMessage.Length == 1)
        {
            _errorMessage = "Window effect not specified!";
            return;
        }
        else if (splitMessage.Length > 2)
        {
            _errorMessage = "Invalid window command format!";
            return;
        }

        bool isValidWindowEffect = Enum.TryParse(splitMessage[1], true, out _windowEffect);
        if (!isValidWindowEffect)
        {
            _errorMessage = $"Invalid window effect '{splitMessage[1]}'!";
            return;
        }
        _isValid = isValidWindowEffect;
    }

    private void SetCharacterNameArgs(string[] splitMessage)
    {
        if (splitMessage.Length != 3)
        {
            _errorMessage = "Invalid character rename command format!";
            return;
        }
        bool isValidCharacter = Enum.TryParse(splitMessage[1], true, out _character);
        if (!isValidCharacter)
        {
            _errorMessage = $"Character '{splitMessage[1]}' invalid!";
            return;
        }
        _newCharacterName = splitMessage[2];

        // Check for invalid characters
        bool isValidName = IsNameValid(_newCharacterName, CHARACTER_DATA_NAME_SIZE);
        if (!isValidName)
        {
            _errorMessage = $"Character name {_newCharacterName} invalid!";
            return;
        }

        _isValid = isValidCharacter;
    }

    private void SetGPArgs(string[] splitMessage)
    {
        if (splitMessage.Length < 2 || splitMessage.Length > 3)
        {
            _errorMessage = "Invalid GP command format!";
            return;
        }
        else
        {
            // Get GP effect.
            bool isValidGPEffect = Enum.TryParse(splitMessage[1], true, out _gpEffect);
            if (!isValidGPEffect)
            {
                // If not valid, return.
                _errorMessage = $"Invalid GP command: '{splitMessage[1]}'!";
                return;
            }
            else if (_gpEffect == GPEffect.empty)
            {
                // If it's empty GP, mark as valid and return.
                _isValid = isValidGPEffect;
                return;
            }

            // If it's modify GP effect:
            if (splitMessage.Length != 3)
            {
                _errorMessage = "GP amount not specified!";
                return;
            }
            bool isValidGPRange = false;

            // Check that GP amount is an actual number.
            bool isValidNumber = Int32.TryParse(splitMessage[2], out int gpAmount);

            if (!isValidNumber)
            {
                // If not a valid number, return.
                _errorMessage = $"GP amount '{splitMessage[2]}' not a valid number!";
                return;
            }

            if (gpAmount >= GP_AMOUNT_MIN &&
                gpAmount <= GP_AMOUNT_MAX)
            {
                isValidGPRange = true;
                GPAmount = gpAmount;
            }

            if (!isValidGPRange)
            {
                // If not a valid GP range, return.
                _errorMessage = $"GP amount '{splitMessage[2]}' out of range - valid range: {GP_AMOUNT_MIN}-{GP_AMOUNT_MAX}";
                return;
            }

            _isValid = isValidGPEffect && isValidGPRange;
        }
    }

    private static bool IsNameValid(string newName, byte nameSize)
    {
        for (int i = 0; i < newName.Length; i++) // ignore the item icon
        {
            if (i > nameSize) break;
            if (!VALID_NAME_CHARACTERS.Contains(newName[i]))
            {
                return false;
            }
        }
        return true;
    }
}

public enum Effect
{
    _INVALID,
    item,
    spell,
    character,
    inventory,
    itemname,
    spellname,
    charactername,
    gp,
    window,
    mirror
}

public enum WindowEffect
{
    _INVALID,
    vanilla,
    demonchocobo,
    random,
}

public enum GPEffect
{
    modify,
    empty
}