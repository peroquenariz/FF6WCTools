using FF6WCToolsLib;
using System;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using static FF6WCToolsLib.WCData;

namespace CrowdControlLib;

/// <summary>
/// Handles the crowd control message parsing.
/// </summary>
public class CrowdControlArgs
{
    private const string VALID_NAME_CHARACTERS = // Characters available in the rename screen, plus whitespace.
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?/:\"\'-.0123456789 ";
    
    private bool _isValid;
    private string _errorMessage;

    private const int GP_AMOUNT_MIN = -10000; // TODO: expose customizable parameters in a config file
    private const int GP_AMOUNT_MAX = 10000;
    private const byte MIN_SPELL_LEARN_RATE = 1;
    private const byte MAX_SPELL_LEARN_RATE = 20;
    private const int STAT_BOOST_MIN_VALUE = -7;
    private const int STAT_BOOST_MAX_VALUE = 7;
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
    private Stat _statBoostType;
    private SpellEffect _spellEffect;
    private TargetingPreset _targetingPreset;

    public bool IsValid => _isValid;
    public string ErrorMessage => _errorMessage;
    public Effect EffectType => _effectType;
    public WindowEffect WindowEffect => _windowEffect;
    public GPEffect GPEffect => _gpEffect;
    public ItemEffect ItemEffect => _itemEffect;
    public ElementalProperties Element => _element;
    public int GPAmount { get; private set; }
    public Character Character => _character;
    public string NewCharactername => _newCharacterName;
    public Item Item => _item;
    public string NewItemName => _newItemName;
    public Spell Spell => _spell;
    public string NewSpellName => _newSpellName;
    public byte LearnRate { get; private set; }
    public Item RelicEffectItem => _relicEffectItem;
    public Stat StatBoostType => _statBoostType;
    public int StatBoostValue { get; private set; }
    public SpellEffect SpellEffect => _spellEffect;
    public TargetingPreset TargetingPreset => _targetingPreset;
    public byte SpellPower { get; private set; }

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
                SetSpellArgs(splitMessage);
                break;
            case Effect.character:
                _errorMessage = $"'{_effectType}' not implemented yet!";
                break;
            case Effect.inventory:
                _errorMessage = $"'{_effectType}' not implemented yet!";
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
        }
    }

    private void SetSpellArgs(string[] splitMessage)
    {
        // Check if the message length is valid.
        if (splitMessage.Length < 3 || splitMessage.Length > 4)
        {
            _errorMessage = "Invalid spell command!";
            return;
        }

        // Check if the item is valid.
        if (!Enum.TryParse(splitMessage[1], true, out _spell))
        {
            _errorMessage = $"Spell '{splitMessage[1]}' invalid!";
            return;
        }

        // Check if the item effect is valid.
        if (!Enum.TryParse(splitMessage[2], true, out _spellEffect))
        {
            _errorMessage = $"Spell effect '{splitMessage[2]}' invalid!";
            return;
        }

        switch (_spellEffect)
        {
            case SpellEffect.targeting:
                _isValid = CheckTargetingPreset(splitMessage);
                break;
            case SpellEffect.spellpower:
                _isValid = CheckSpellPowerValue(splitMessage);
                break;
            case SpellEffect.element:
                _isValid = CheckSpellElement(splitMessage);
                break;
            case SpellEffect.mpdamage:
                _isValid = true;
                break;
            case SpellEffect.ignoredefense:
                _isValid = true;
                break;
            case SpellEffect.mpcost:
                _isValid = CheckSpellMPCost(splitMessage);
                break;
            case SpellEffect.status:
                break;
            case SpellEffect.liftstatus:
                break;
        }
    }

    private bool CheckSpellMPCost(string[] splitMessage)
    {
        throw new NotImplementedException();
    }

    private bool CheckSpellElement(string[] splitMessage)
    {
        if (splitMessage.Length != 4)
        {
            _errorMessage = "Element not specified!";
            return false;
        }

        if (!Enum.TryParse(splitMessage[3], true, out _element))
        {
            _errorMessage = $"Element '{splitMessage[3]}' invalid!";
            return false;
        }
        
        return true;
    }

    private bool CheckSpellPowerValue(string[] splitMessage)
    {
        if (splitMessage.Length != 4)
        {
            _errorMessage = "Spell power value not specified!";
            return false;
        }
        
        if (!int.TryParse(splitMessage[3], out int spellPower))
        {
            _errorMessage = $"Spell power value '{splitMessage[3]}' invalid!";
            return false;
        }
        else if (spellPower < byte.MinValue || spellPower > byte.MaxValue)
        {
            _errorMessage = $"Spell power value '{spellPower}' invalid! Valid range: {byte.MinValue}-{byte.MaxValue}";
            return false;
        }

        SpellPower = (byte)spellPower;
        return true;
    }

    private bool CheckTargetingPreset(string[] splitMessage)
    {
        if (splitMessage.Length != 4)
        {
            _errorMessage = $"Spell targeting not specified!";
            return false;
        }
        else if (!Enum.TryParse(splitMessage[3], true, out _targetingPreset))
        {
            _errorMessage = $"Spell targeting invalid!";
            return false;
        }

        return true;
    }

    private void SetItemArgs(string[] splitMessage)
    {
        // Check if the message length is valid.
        if (splitMessage.Length < 4 || splitMessage.Length > 5)
        {
            _errorMessage = "Invalid item command!";
            return;
        }

        // Check if the item is valid.
        if (!Enum.TryParse(splitMessage[1], true, out _item))
        {
            _errorMessage = $"Item '{splitMessage[1]}' invalid!";
            return;
        }

        // Check if the item effect is valid.
        if (!Enum.TryParse(splitMessage[2], true, out _itemEffect))
        {
            _errorMessage = $"Item effect '{splitMessage[2]}' invalid!";
            return;
        }

        // If it's a item spell casting effect, check that the parameters are valid.
        if (_itemEffect is ItemEffect.spellproc or ItemEffect.breakable or ItemEffect.teach)
        {
            // Check if the item is valid for spell casting effects.
            if (!CheckIfItemIsValidForSpellCastingEffect()) return; // Please make a longer method name, kkthxbye.
            
            // Check if it's a valid spell.
            bool isValidSpell = Enum.TryParse(splitMessage[3], true, out _spell);
            
            // Only allow magical spells for procing.
            if (!isValidSpell || (int)_spell >= SPELLS_MAGICAL_NAMES_BLOCK_COUNT)
            {
                _errorMessage = $"Spell '{splitMessage[3]}' invalid!";
                return;
            }

            // If it's not a teaching effect, it doesn't need more parameters.
            // Set as valid and return.
            if (!(_itemEffect == ItemEffect.teach))
            {
                _isValid = isValidSpell;
                return;
            }
            else
            {
                // Check if the learn rate is valid.
                if (!TryParseSpellLearnRate(splitMessage)) return;
            }
        }
        
        // Relic effect.
        if (_itemEffect == ItemEffect.reliceffect)
        {
            // Check that the relic effect provided is valid (is a relic?)
            if (!Enum.TryParse(splitMessage[3], true, out _relicEffectItem))
            {
                _errorMessage = $"Invalid relic '{_relicEffectItem}'!";
                return;
            }

            _isValid = CheckIfItemIsRelic(splitMessage);
            return;
        }

        // Weapon element effect.
        if (_itemEffect == ItemEffect.weaponelement)
        {
            // Return if it's not a valid weapon.
            if (!DataHandler.CheckItemInRange((byte)_item, RANGE_WEAPONS))
            {
                _errorMessage = $"Item {_item} is not a weapon!";
                return;
            }
        }

        // Parse element for all elemental effect types.
        if (_itemEffect is ItemEffect.absorb or ItemEffect.nullify or ItemEffect.weak or ItemEffect.weaponelement)
        {
            string element = splitMessage[3];

            // Return if it's not a valid element.
            if (!Enum.TryParse(element, true, out _element))
            {
                _errorMessage = $"Invalid element '{splitMessage[3]}'!";
                return;
            }

            _isValid = true;
            return;
        }

        // Stat boost effect.
        if (_itemEffect == ItemEffect.statboost)
        {
            string statBoostType = splitMessage[3];

            // Check if it's a valid stat.
            if (!Enum.TryParse(statBoostType, true, out _statBoostType))
            {
                _errorMessage = $"Invalid stat '{statBoostType}'!";
                return;
            }

            // Check if the stat value was specified.
            if (splitMessage.Length != 5)
            {
                _errorMessage = $"Stat boost value not specified!";
                return;
            }
            
            _isValid = CheckIfStatBoostValueIsValid(splitMessage);
            return;
        }

        // Price effect.
        if (_itemEffect == ItemEffect.price)
        {
            _isValid = TryParseGPAmount(splitMessage);
            return;
        }
    }

    private bool TryParseGPAmount(string[] splitMessage)
    {
        // Try parse the gp amount.
        if (!int.TryParse(splitMessage[3], out int gpAmount))
        {
            // Not a number.
            _errorMessage = $"{splitMessage[3]} is not a number!";
            return false;
        }
        else if (gpAmount < ushort.MinValue ||
                 gpAmount > ushort.MaxValue)
        {
            // Price out of range
            _errorMessage = $"Price {gpAmount} GP is out of range! Valid range: {ushort.MinValue}-{ushort.MaxValue}";
            return false;
        }

        // Save the GP amount and return.
        GPAmount = gpAmount;
        return true;
    }

    private bool CheckIfStatBoostValueIsValid(string[] splitMessage)
    {
        string statBoostValueString = splitMessage[4];

        // Check if the value provided is valid and within range.
        if (!int.TryParse(statBoostValueString, out int statBoostValue))
        {
            // It's not a number.
            _errorMessage = $"'{splitMessage[4]}' is not a number!";
            return false;
        }
        else if (statBoostValue < STAT_BOOST_MIN_VALUE ||
                 statBoostValue > STAT_BOOST_MAX_VALUE)
        {
            // Value is out of valid range
            _errorMessage = $"Stat boost value '{statBoostValue}' out of range! Valid range: {STAT_BOOST_MIN_VALUE}-{STAT_BOOST_MAX_VALUE} ";
            return false;
        }

        StatBoostValue = statBoostValue;
        return true;
    }

    private bool CheckIfItemIsRelic(string[] splitMessage)
    {
        // Ignore tintinabar :(
        // Apparently this effect is hardcoded to item #229.
        if (_relicEffectItem == Item.Tintinabar)
        {
            _errorMessage = $"'Tintinabar' effect cannot be copied!";
            return false;
        }

        bool isRelic = DataHandler.CheckItemInRange(_relicEffectItem, RANGE_RELICS);

        if (!isRelic)
        {
            _errorMessage = $"Invalid relic '{splitMessage[3]}'!";
            return false;
        }
        
        return isRelic;
    }

    private bool TryParseSpellLearnRate(string[] splitMessage)
    {
        // Parse the spell learn rate.
        bool isValidNumber = Byte.TryParse(splitMessage[4], out byte learnRate);

        if (!isValidNumber)
        {
            _errorMessage = $"Invalid number '{splitMessage[4]}'!";
            return false;
        }

        LearnRate = learnRate;

        if (LearnRate >= MIN_SPELL_LEARN_RATE &&
            LearnRate <= MAX_SPELL_LEARN_RATE)
        {
            _isValid = isValidNumber;
            return true;
        }
        else
        {
            _errorMessage = $"Invalid learning rate '{splitMessage[4]}'! Valid range: {MIN_SPELL_LEARN_RATE}-{MAX_SPELL_LEARN_RATE}.";
            return false;
        }
    }

    private bool CheckIfItemIsValidForSpellCastingEffect()
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
                    return false;
                }
                break;
            case ItemEffect.breakable:
                // Don't allow skeans or consumables. TODO: check if relics and tools can be broken!
                bool isSkean = DataHandler.CheckItemInRange(_item, RANGE_SKEANS);
                bool isConsumable = DataHandler.CheckItemInRange(_item, RANGE_CONSUMABLES);
                if (isSkean || isConsumable)
                {
                    _errorMessage = $"Item '{_item}' invalid for spell breaking! Consumables and skeans are not allowed.";
                    return false;
                }
                break;
            case ItemEffect.teach:
                // Only allow equippable gear (weapons, helmets, armors, shields, relics).
                bool isEquippableGearPiece = DataHandler.CheckItemInRange(_item, RANGE_GEAR);
                bool isRelic = DataHandler.CheckItemInRange(_item, RANGE_RELICS);
                if (!isEquippableGearPiece && !isRelic)
                {
                    _errorMessage = $"Item '{_item}' invalid for spell teaching! Only equippable items allowed.";
                    return false;
                }
                break;
        }

        return true;
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
        if (splitMessage.Length < 3)
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
        
        // Concatenate character name.
        for (int i = 2; i < splitMessage.Length; i++)
        {
            _newCharacterName += splitMessage[i] + " ";
        }

        // Remove trailing whitespaces.
        _newCharacterName = _newCharacterName.TrimEnd();

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
    weaponelement,
    price,
}

public enum SpellEffect
{
    targeting,
    spellpower,
    element,
    mpdamage,
    ignoredefense,
    status,
    liftstatus,
    mpcost,
}