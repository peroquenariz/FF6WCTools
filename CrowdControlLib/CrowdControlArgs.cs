using FF6WCToolsLib;
using System;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using static FF6WCToolsLib.WCData;

namespace CrowdControlLib;

public class CrowdControlArgs
{
    private const string VALID_NAME_CHARACTERS = // Characters available in the rename screen
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?/:\"\'-.0123456789 ";
    
    private bool _isValid;
    private string _errorMessage;

    private const int GP_AMOUNT_MIN = -10000; // TODO: expose customizable parameters in a config file
    private const int GP_AMOUNT_MAX = 10000;
    private const byte MIN_SPELL_LEARN_RATE = 1;
    private const byte MAX_SPELL_LEARN_RATE = 20;

    private readonly Effect _effectType;
    private WindowEffect _windowEffect;
    private GPEffect _gpEffect;
    private ItemEffect _itemEffect;
    private ElementalProperties _element;
    private Character _character;
    private string _newCharacterName;
    private Item _item;
    private string _newItemName;
    private Spell _spell;
    private string _newSpellName;
    private Item _relicEffectItem;

    public bool IsValid => _isValid;
    public string ErrorMessage => _errorMessage;
    public Effect EffectType => _effectType;
    public WindowEffect WindowEffect => _windowEffect;
    public GPEffect GPEffect => _gpEffect;
    public ItemEffect ItemEffect => _itemEffect;
    public ElementalProperties Element => _element;
    public int GPAmount { get; set; }
    public Character Character => _character;
    public string NewCharactername => _newCharacterName;
    public Item Item => _item;
    public string NewItemName => _newItemName;
    public Spell Spell => _spell;
    public string NewSpellName => _newSpellName;
    public byte LearnRate { get; set; }
    public Item RelicEffectItem => _relicEffectItem;

    public CrowdControlArgs(string message)
    {
        _errorMessage = string.Empty;
        _newCharacterName = string.Empty;
        _newItemName = string.Empty;
        _newSpellName = string.Empty;

        string[] splitMessage = message.Split(" ");

        Enum.TryParse(splitMessage[0], true, out _effectType);

        switch (_effectType)
        {
            case Effect._INVALID:
                _errorMessage = $"Invalid crowd control effect '{splitMessage[0]}'!";
                return;
            case Effect.item:
                SetItemArgs(splitMessage);
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
                SetSpellNameArgs(splitMessage);
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

    private void SetItemArgs(string[] splitMessage)
    {
        if (splitMessage.Length < 4 || splitMessage.Length > 5)
        {
            _errorMessage = "Invalid item command!";
            return;
        }
        bool isValidItem = Enum.TryParse(splitMessage[1], true, out _item);

        if (!isValidItem)
        {
            _errorMessage = $"Item '{splitMessage[1]}' invalid!";
            return;
        }

        bool isValidItemEffect = Enum.TryParse(splitMessage[2], true, out _itemEffect);

        if (!isValidItemEffect)
        {
            _errorMessage = $"Item effect '{splitMessage[2]}' invalid!";
            return;
        }

        // If it's a item spell casting effect, check that the parameters are valid.
        if (_itemEffect is ItemEffect.spellproc or ItemEffect.breakable or ItemEffect.teach)
        {
            // Check for correct item type.
            switch (_itemEffect)
            {
                case ItemEffect.spellproc:
                    // Only allow weapons.
                    bool isWeapon = DataHandler.CheckItemInRange(_item, RANGE_WEAPONS);
                    if (!isWeapon)
                    {
                        _errorMessage = $"Item '{_item}' invalid for spell proccing! Only weapons are allowed.";
                        return;
                    }
                    break;
                case ItemEffect.breakable:
                    // Don't allow skeans or consumables. TODO: check if relics and tools can be broken!
                    bool isSkean = DataHandler.CheckItemInRange(_item, RANGE_SKEANS);
                    bool isConsumable = DataHandler.CheckItemInRange(_item, RANGE_CONSUMABLES);
                    if (isSkean || isConsumable)
                    {
                        _errorMessage = $"Item '{_item}' invalid for spell breaking! Consumables and skeans are not allowed.";
                        return;
                    }
                    break;
                case ItemEffect.teach:
                    // Only allow equippable gear (weapons, helmets, armors, shields, relics).
                    bool isEquippableGearPiece = DataHandler.CheckItemInRange(_item, RANGE_GEAR);
                    bool isRelic = DataHandler.CheckItemInRange(_item, RANGE_RELICS);
                    if (!isEquippableGearPiece && !isRelic)
                    {
                        _errorMessage = $"Item '{_item}' invalid for spell teaching! Only equippable items allowed.";
                        return;
                    }
                    break;
            }
            
            bool isValidSpell = Enum.TryParse(splitMessage[3], true, out _spell);
            Console.WriteLine(_spell);
            // Only allow magical spells for procing.
            if (!isValidSpell || (int)_spell >= SPELLS_MAGICAL_NAMES_BLOCK_COUNT)
            {
                _errorMessage = $"Spell '{splitMessage[3]}' invalid!";
                return;
            }

            if (!(_itemEffect == ItemEffect.teach))
            {
                // If it doesn't need more parameters, set as valid args and return.
                _isValid = isValidSpell;
                return;
            }
            else
            {
                // Parse the spell teach rate.
                bool isValidNumber = Byte.TryParse(splitMessage[4], out byte learnRate);

                if (!isValidNumber)
                {
                    _errorMessage = $"Invalid number '{splitMessage[4]}'!";
                    return;
                }

                LearnRate = learnRate;

                if (LearnRate >= MIN_SPELL_LEARN_RATE &&
                    LearnRate <= MAX_SPELL_LEARN_RATE)
                {
                    _isValid = isValidNumber;
                    return;
                }
                else
                {
                    _errorMessage = $"Invalid teaching rate '{splitMessage[4]}'! Ranges allowed: 1-20.";
                    return;
                }
            }
        }
        
        if (_itemEffect == ItemEffect.reliceffect)
        {
            // Check that the relic effect provided is valid (is a relic?)
            // Build a 4 byte array with the effect data (grab it from ItemData._defaultData?)
            // In CrowdControl.cs, bitwise OR this array against the existing effect data
            // so that it adds to the ones already existing.
            string relicParam = splitMessage[3];

            isValidItem = Enum.TryParse(relicParam, true, out Item _relicEffectItem);

            if (!isValidItem)
            {
                _errorMessage = $"Invalid relic '{_relicEffectItem}'!";
                return;
            }

            // Ignore tintinabar :(
            // Apparently this effect is hardcoded to item #229.
            if (_relicEffectItem == Item.Tintinabar)
            {
                _errorMessage = $"'Tintinabar' effect cannot be copied!";
                return;
            }
            
            bool isRelic = DataHandler.CheckItemInRange(_relicEffectItem, RANGE_RELICS);
            
            if (!isRelic)
            {
                _errorMessage = $"Invalid relic '{splitMessage[3]}'!";
                return;
            }

            _isValid = isRelic;
        }

        if (_itemEffect is ItemEffect.absorb or ItemEffect.nullify or ItemEffect.weak)
        {
            string element = splitMessage[3];

            // Parse the element
            bool isValidElement = Enum.TryParse(element, out ElementalProperties _element);

            if (!isValidElement)
            {
                _errorMessage = $"Invalid element '{splitMessage[3]}'!";
                return;
            }

            _isValid = isValidElement;
        }
    }

    private void SetSpellNameArgs(string[] splitMessage)
    {
        if (splitMessage.Length <= 1)
        {
            _errorMessage = "Spell not specified!";
            return;
        }
        bool isValidSpell = Enum.TryParse(splitMessage[1], true, out _spell);

        if (!isValidSpell)
        {
            _errorMessage = $"Spell '{splitMessage[1]}' invalid!";
            return;
        }

        // Concatenate spell name.
        for (int i = 2; i < splitMessage.Length; i++)
        {
            _newSpellName += splitMessage[i] + " ";
        }

        // Remove trailing whitespaces.
        _newSpellName = _newSpellName.TrimEnd();

        // Check for invalid characters
        bool isValidName = IsNameValid(_newSpellName, SPELLS_MAGICAL_NAMES_BLOCK_SIZE - 1);
        if (!isValidName)
        {
            _errorMessage = $"Spell name {_newSpellName} invalid!";
            return;
        }
        _isValid = isValidSpell;
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
            _errorMessage = "Invalid window command!";
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
            _errorMessage = "Invalid character rename command!";
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
            _errorMessage = "Invalid GP command!";
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
            else
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

public enum ItemEffect
{
    spellproc,
    breakable,
    teach,
    reliceffect,
    statboost,
    absorb,
    nullify,
    weak,
}