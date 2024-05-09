using System;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using static FF6WCToolsLib.WCData;
using static CrowdControlLib.CrowdControlEffects;
using FF6WCToolsLib;
using FF6WCToolsLib.DataTemplates;

namespace CrowdControlLib;

/// <summary>
/// Handles the crowd control message parsing.
/// </summary>
internal class CrowdControlArgs
{
    /// <summary>
    /// Characters available in the rename screen, plus whitespace.
    /// </summary>
    private const string VALID_NAME_CHARACTERS = 
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?/:\"\'-.0123456789 ";
    
    // Global effect triggers.
    private const string INVENTORY_COMMAND_TRIGGER = "inventory";
    private const string GP_COMMAND_TRIGGER = "gp";
    private const string WINDOW_COMMAND_TRIGGER = "window";
    private const string MIRROR_COMMAND_TRIGGER = "mirror";

    private const int MIN_COMMAND_LENGTH = 2;

    private bool _isValid;
    private string _errorMessage;

    // Command ranges.
    private const int GP_AMOUNT_MIN = -10000; // TODO: expose customizable parameters in a config file
    private const int GP_AMOUNT_MAX = 10000;
    private const byte MIN_SPELL_LEARN_RATE = 1;
    private const byte MAX_SPELL_LEARN_RATE = 20; // More than 20 is possible, I just capped it to FF6's max vanilla learn rate.
    private const int CHARACTER_STAT_BOOST_MIN = -10;
    private const int CHARACTER_STAT_BOOST_MAX = 10;
    private const int ITEM_STAT_BOOST_MIN_VALUE = -7;
    private const int ITEM_STAT_BOOST_MAX_VALUE = 7;
    private const int SPELL_COST_MAX_VALUE = 254;

    private int _cost;

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
    private StatusEffect _statusEffect;
    private CharacterEffect _characterEffect;
    private EquipmentSlot _equipmentSlot;
    private InventoryEffect _inventoryEffect;
    private Esper _esper;

    public bool IsValid => _isValid;
    public string ErrorMessage => _errorMessage;
    public int Cost => _cost;
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
    public byte SpellMPCost { get; private set; }
    public StatusEffect StatusEffect => _statusEffect;
    public byte StatusEffectFlag { get; private set; }
    public byte StatusEffectByteOffset { get; private set; }
    public CharacterEffect CharacterEffect => _characterEffect;
    public EquipmentSlot EquipmentSlot => _equipmentSlot;
    public InventoryEffect InventoryEffect => _inventoryEffect;
    public Esper Esper => _esper;

    public CrowdControlArgs(CrowdControlMessage message)
    {
        _errorMessage = string.Empty;
        _newCharacterName = string.Empty;
        _newItemName = string.Empty;
        _newSpellName = string.Empty;
        
        // Split chat message.
        string[] splitMessage = message.Content.Split(" ");

        if (splitMessage.Length < MIN_COMMAND_LENGTH)
        {
            _errorMessage = $"Invalid command '{message.Content}'";
            return;
        }
        else
        {
            _effectType = GetEffectTypeAndTarget(splitMessage); 
        }

        // Get effect and command parameters.
        string effect = splitMessage[1];
        string[] parameters = Array.Empty<string>();
        if (splitMessage.Length > MIN_COMMAND_LENGTH)
        {
            parameters = splitMessage[2..];
        }

        switch (_effectType)
        {
            case Effect._INVALID:
                _errorMessage = $"Invalid crowd control effect '{splitMessage[0]}'!";
                return;
            case Effect.item:
                SetItemArgs(effect, parameters);
                break;
            case Effect.spell:
                SetSpellArgs(effect, parameters);
                break;
            case Effect.character:
                SetCharacterArgs(effect, parameters);
                break;
            case Effect.inventory:
                SetInventoryArgs(effect, parameters);
                break;
            case Effect.gp:
                SetGPArgs(effect, parameters);
                break;
            case Effect.window:
                SetWindowArgs(effect);
                break;
            case Effect.mirror:
                _isValid = true;
                break;
        }
    }

    /// <summary>
    /// Checks that the effect target is valid, saves the target and set the effect type.
    /// </summary>
    /// <param name="splitMessage">The message to parse.</param>
    /// <returns>A crowd control effect type, or _INVALID if the target is invalid.</returns>
    private Effect GetEffectTypeAndTarget(string[] splitMessage)
    {
        string target = splitMessage[0].ToLower();
        string effect = splitMessage[1].ToLower();

        if (target == "ragnarok") // Stupid Square naming both sword and esper the same
        {
            // Check if it's the item or the esper.
            if (Enum.IsDefined(typeof(ItemEffect), effect))
            {
                return Effect.item;
            }
            else if (Enum.IsDefined(typeof(EsperEffect), effect))
            {
                // TODO: implement esper effects.
                return Effect._INVALID;
            }
            else
            {
                return Effect._INVALID;
            }
        }
        else if (target == "remedy") // lmao
        {
            // Check if it's the item or the spell.
            if (Enum.IsDefined(typeof(ItemEffect), effect))
            {
                return Effect.item;
            }
            else if (Enum.IsDefined(typeof(SpellEffect), effect))
            {
                // TODO: implement esper effects.
                return Effect.spell;
            }
            else
            {
                return Effect._INVALID;
            }
        }
        else if (Enum.TryParse(target, true, out Item item))
        {
            _item = item;
            return Effect.item;
        }
        else if (Enum.TryParse(target, true, out Spell spell))
        {
            _spell = spell;
            return Effect.spell;
        }
        else if (Enum.TryParse(target, true, out Esper esper))
        {
            // TODO: add esper effects
            _esper = esper;
            return Effect._INVALID;
        }
        else if (Enum.TryParse(target, true, out Character character))
        {
            _character = character;
            return Effect.character;
        }
        else if (target == INVENTORY_COMMAND_TRIGGER)
        {
            return Effect.inventory;
        }
        else if (target == GP_COMMAND_TRIGGER)
        {
            return Effect.gp;
        }
        else if (target == WINDOW_COMMAND_TRIGGER)
        {
            return Effect.window;
        }
        else if (target == MIRROR_COMMAND_TRIGGER)
        {
            // TODO: mirror command triggers even if you put other parameters
            // This is a placeholder for future text mirroring effects.
            return Effect.mirror;
        }
        else
        {
            return Effect._INVALID;
        }
    }

    private void SetInventoryArgs(string effect, string[] parameters)
    {
        // Check if the inventory effect is valid.
        if (!Enum.TryParse(effect, true, out _inventoryEffect))
        {
            _errorMessage = $"Inventory effect {effect} invalid!";
            return;
        }

        string itemParameter = parameters[0];

        // Parse the item to add or remove from the inventory.
        // Don't allow Empty item to be used.
        if (!Enum.TryParse(itemParameter, true, out _item) && _item != Item.Empty)
        {
            _errorMessage = $"Item {itemParameter} invalid!";
            return;
        }

        _isValid = true;
    }

    private void SetCharacterArgs(string effect, string[] parameters)
    {
        // Check if the character effect is valid.
        if (!Enum.TryParse(effect, true, out _characterEffect))
        {
            _errorMessage = $"Character effect '{effect}' invalid!";
            return;
        }

        if (_characterEffect == CharacterEffect.rename)
        {
            SetCharacterNameArgs(parameters);
            return;
        }

        string parameter = parameters[0];

        // If it's a spell teaching effect, save the spell.
        if (_characterEffect is CharacterEffect.teach or CharacterEffect.forget)
        {
            // Ignore Gogo and Umaro.
            if (_character is Character.Gogo or Character.Umaro)
            {
                _errorMessage = $"{_character} can't learn or forget spells!";
                return;
            }
            
            // Get spell.
            if (!Enum.TryParse(parameter, true, out _spell))
            {
                _errorMessage = $"Spell '{parameter}' invalid!";
                return;
            }
            // Only allow magical spells. TODO: teach lores/swdtechs/blitzes/etc.
            else if ((int)_spell >= SpellMagicalName.BlockCount)
            {
                _errorMessage = $"Spell '{parameter}' can't be taught! (teaching lores/swdtechs/blitzes/etc not implemented yet)";
                return;
            }

            _isValid = true;
            return;
        }
        // If it's a stat boost effect, save the value.
        else if (Enum.IsDefined(typeof(Stat), effect))
        {
            // Get the stat type.
            Enum.TryParse(effect, true, out _statBoostType);

            _isValid = IsStatBoostValid(parameter, CHARACTER_STAT_BOOST_MIN, CHARACTER_STAT_BOOST_MAX);
            return;
        }
        // If it's an equipment effect, save the item to equip.
        else if (Enum.IsDefined(typeof(EquipmentSlot), effect))
        {
            // Check if it's a correct item.
            if (!Enum.TryParse(parameter, true, out _item))
            {
                _errorMessage = $"Character equipment item '{parameter}' invalid!";
                return;
            }
            
            // Get the equipment slot.
            Enum.TryParse(effect, true, out _equipmentSlot);
            
            // Check if the item is valid for the given slot
            // TODO: expose this in a config file to allow equipping glitched items.
            _isValid = IsEquipmentValidForSlot(_item, _equipmentSlot);
            return;
        }
    }

    /// <summary>
    /// Checks if the given item is valid for that slot.
    /// </summary>
    /// <param name="item">Message parameter.</param>
    /// <param name="slot">The character equipment slot.</param>
    /// <returns>True if it's a valid item for the slot, otherwise false.</returns>
    private bool IsEquipmentValidForSlot(Item item, EquipmentSlot slot)
    {
        // Empty item is compatible with any slot.
        if (item == Item.Empty) return true;

        // Ignore consumables.
        if (DataHandler.IsItemConsumable(item))
        {
            _errorMessage = $"Character equipment item '{item}' invalid - can't equip consumables!";
            return false;
        }

        bool isValidEquipment = false;

        // Check item ranges.
        if (slot is EquipmentSlot.relic1 or EquipmentSlot.relic2)
        {
            isValidEquipment = DataHandler.CheckItemInRange((byte)item, RANGE_RELICS);
        }
        else
        {
            switch (slot)
            {
                case EquipmentSlot.rhand:
                    isValidEquipment = DataHandler.CheckItemInRange((byte)item, RANGE_WEAPONS);
                    break;
                case EquipmentSlot.lhand:
                    isValidEquipment = DataHandler.CheckItemInRange((byte)item, RANGE_WEAPONS) ||
                                       DataHandler.CheckItemInRange((byte)item, RANGE_SHIELDS);
                    break;
                case EquipmentSlot.helmet:
                    isValidEquipment = DataHandler.CheckItemInRange((byte)item, RANGE_HELMETS);
                    break;
                case EquipmentSlot.armor:
                    isValidEquipment = DataHandler.CheckItemInRange((byte)item, RANGE_ARMORS);
                    break;
            }
        }

        // Set error message if the item doesn't correspond to the slot.
        if (!isValidEquipment)
        {
            _errorMessage = $"Character equipment item '{item}' invalid for {slot} slot!";
        }

        return isValidEquipment;
    }

    private void SetSpellArgs(string effect, string[] parameters)
    {
        // Check if the spell effect is valid.
        if (!Enum.TryParse(effect, true, out _spellEffect))
        {
            _errorMessage = $"Spell effect '{effect}' invalid!";
            return;
        }

        switch (_spellEffect)
        {
            case SpellEffect.rename:
                SetSpellNameArgs(parameters);
                break;
            case SpellEffect.reset:
                _isValid = true;
                break;
            case SpellEffect.targeting:
                _isValid = IsTargetingPresetValid(parameters);
                break;
            case SpellEffect.spellpower:
                _isValid = IsSpellPowerValid(parameters);
                break;
            case SpellEffect.element:
                _isValid = IsSpellElementValid(parameters);
                break;
            case SpellEffect.mpdamage:
                _isValid = true;
                break;
            case SpellEffect.ignoredefense:
                _isValid = true;
                break;
            case SpellEffect.mpcost:
                _isValid = IsSpellMPCostValid(parameters);
                break;
            case SpellEffect.status:
                _isValid = IsStatusEffectValid(parameters);
                break;
            case SpellEffect.liftstatus:
                _isValid = true;
                break;
        }
    }

    private bool IsStatusEffectValid(string[] parameters)
    {
        if (parameters.Length == 0)
        {
            _errorMessage = "Status effect not specified!";
            return false;
        }

        string statusEffectParameter = parameters[0];
        
        // Check if the status effect is valid.
        if (!Enum.TryParse(statusEffectParameter, true, out _statusEffect))
        {
            _errorMessage = $"Invalid or not supported status '{statusEffectParameter}'!";
            return false;
        }

        // Get bit flag and byte offset for the corresponding status.
        (byte bitFlag, byte offset) = StatusEffectByteData[_statusEffect];
        
        StatusEffectFlag = bitFlag;
        StatusEffectByteOffset = offset;
        
        return true;
    }

    private bool IsSpellMPCostValid(string[] parameters)
    {
        // Check if spell cost has been specified.
        if (parameters.Length == 0)
        {
            _errorMessage = "Spell cost value not specified!";
            return false;
        }

        string mpCostParameter = parameters[0];

        // Check if spell cost is a valid number.
        if (!int.TryParse(mpCostParameter, out int spellMPCost))
        {
            _errorMessage = $"Spell cost value '{mpCostParameter}' invalid!";
            return false;
        }
        // Check if spell cost is within a valid range.
        else if (spellMPCost < byte.MinValue || spellMPCost > SPELL_COST_MAX_VALUE)
        {
            _errorMessage = $"Spell cost value '{spellMPCost}' invalid! Valid range: {byte.MinValue}-{SPELL_COST_MAX_VALUE}";
            return false;
        }

        SpellMPCost = (byte)spellMPCost;
        return true;
    }

    private bool IsSpellElementValid(string[] parameters)
    {
        // Check if an element has been specified.
        if (parameters.Length == 0)
        {
            _errorMessage = "Element not specified!";
            return false;
        }

        string elementParameter = parameters[0];

        // Check if the element is valid.
        if (!Enum.TryParse(elementParameter, true, out _element))
        {
            _errorMessage = $"Element '{elementParameter}' invalid!";
            return false;
        }
        
        return true;
    }

    private bool IsSpellPowerValid(string[] parameters)
    {
        // Check if the spell power has been specified.
        if (parameters.Length == 0)
        {
            _errorMessage = "Spell power value not specified!";
            return false;
        }

        string spellPowerParameter = parameters[0];

        // Check if the spell power is a valid number.
        if (!int.TryParse(spellPowerParameter, out int spellPower))
        {
            _errorMessage = $"Spell power value '{spellPowerParameter}' invalid!";
            return false;
        }
        // Check if the spell power specified is within a valid range.
        else if (spellPower < byte.MinValue || spellPower > byte.MaxValue)
        {
            _errorMessage = $"Spell power value '{spellPower}' invalid! Valid range: {byte.MinValue}-{byte.MaxValue}";
            return false;
        }

        SpellPower = (byte)spellPower;
        return true;
    }

    private bool IsTargetingPresetValid(string[] parameters)
    {
        // Check if a targeting preset has been specified.
        if (parameters.Length == 0)
        {
            _errorMessage = $"Spell targeting not specified!";
            return false;
        }
        // Check if the specified targeting preset is valid.
        else if (!Enum.TryParse(parameters[0], true, out _targetingPreset))
        {
            _errorMessage = $"Spell targeting invalid!";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the string array is of a valid length.
    /// </summary>
    /// <param name="splitMessage">The string array with the message.</param>
    /// <param name="minLength">The inclusive minimum length.</param>
    /// <param name="maxLength">The inclusive maximum length.</param>
    /// <returns>True if the array length is between the given ranges, otherwise false.</returns>
    private static bool IsValidLength(string[] splitMessage, int minLength, int maxLength)
    {
        return splitMessage.Length >= minLength && splitMessage.Length <= maxLength;
    }

    private void SetItemArgs(string itemEffect, string[] parameters)
    {
        // Disable editing the empty item.
        if (_item == Item.Empty)
        {
            _errorMessage = $"Item '{_item}' can't be modified!";
            return;
        }
        
        if (Enum.TryParse(itemEffect, true, out _statBoostType)) // Is a stat boost.
        {
            _itemEffect = ItemEffect.statboost;
        }
        else if (!Enum.TryParse(itemEffect, true, out _itemEffect)) // Is a valid item effect.
        {
            _errorMessage = $"Item effect '{itemEffect}' invalid!";
            return;
        }
        
        if (_itemEffect == ItemEffect.reset) // Is a reset.
        {
            _isValid = true;
            return;
        }
        // If it's not a reset and there are no parameters specified, return.
        else if (parameters.Length == 0)
        {
            _errorMessage = "Invalid item command or no parameters specified!";
            return;
        }

        if (_itemEffect == ItemEffect.rename)
        {
            SetItemNameArgs(parameters);
            return;
        }
        // If it's a item spell casting effect, check that the parameters are valid.
        else if (_itemEffect is ItemEffect.proc or ItemEffect.breakable or ItemEffect.teach)
        {
            // Check if the item is valid for spell casting effects.
            if (!IsItemValidForSpellCastingEffect()) return; // Please make a longer method name, kkthxbye.
            
            // Check if it's a valid spell.
            bool isValidSpell = Enum.TryParse(parameters[0], true, out _spell);
            
            // Only allow magical spells for procing.
            if (!isValidSpell || (int)_spell >= SPELLS_MAGICAL_NAMES_BLOCK_COUNT)
            {
                _errorMessage = $"Spell '{parameters[0]}' invalid!";
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
                if (!TryParseSpellLearnRate(parameters[1])) return;
            }
        }
        
        // Relic effect.
        if (_itemEffect == ItemEffect.reliceffect)
        {
            // Check that the target item is equippable
            if (DataHandler.IsItemConsumable(_item))
            {
                _errorMessage = $"Relic effect can't be copied to '{_item}'!";
                return;
            }
            
            // Check that the relic effect provided is valid (is a relic?)
            if (!Enum.TryParse(parameters[0], true, out _relicEffectItem))
            {
                _errorMessage = $"Invalid relic '{parameters[0]}'!";
                return;
            }

            _isValid = IsItemARelic(parameters[0]);
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
            string element = parameters[0];

            // Return if it's not a valid element.
            if (!Enum.TryParse(element, true, out _element))
            {
                _errorMessage = $"Invalid element '{element}'!";
                return;
            }

            _isValid = true;
            return;
        }

        // Stat boost effect.
        if (_itemEffect == ItemEffect.statboost)
        {
            _isValid = IsStatBoostValid(parameters[0], ITEM_STAT_BOOST_MIN_VALUE, ITEM_STAT_BOOST_MAX_VALUE);
            return;
        }

        // Price effect.
        if (_itemEffect == ItemEffect.price)
        {
            _isValid = TryParseGPAmount(parameters[0]);
            return;
        }
    }

    private bool TryParseGPAmount(string parameter)
    {
        // Try parse the gp amount.
        if (!int.TryParse(parameter, out int gpAmount))
        {
            // Not a number.
            _errorMessage = $"{parameter} is not a number!";
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

    private bool IsStatBoostValid(string statBoostValueString, int minValue, int maxValue)
    {
        // Check if the value provided is valid and within range.
        if (!int.TryParse(statBoostValueString, out int statBoostValue))
        {
            // It's not a number.
            _errorMessage = $"'{statBoostValueString}' is not a number!";
            return false;
        }
        else if (statBoostValue < minValue ||
                 statBoostValue > maxValue)
        {
            // Value is out of valid range
            _errorMessage = $"Stat boost value '{statBoostValue}' out of range! Valid range: {minValue}-{maxValue} ";
            return false;
        }

        StatBoostValue = statBoostValue;
        return true;
    }

    private bool IsItemARelic(string parameter)
    {
        // Ignore tintinabar :(
        // Apparently this effect is hardcoded to item #229.
        if (_relicEffectItem == Item.Tintinabar)
        {
            _errorMessage = $"'Tintinabar' effect cannot be copied!";
            return false;
        }

        // Check if the item is a relic.
        bool isRelic = DataHandler.CheckItemInRange(_relicEffectItem, RANGE_RELICS);

        if (!isRelic)
        {
            _errorMessage = $"Invalid relic '{parameter}'!";
            return false;
        }
        
        return isRelic;
    }

    private bool TryParseSpellLearnRate(string parameter)
    {
        // Parse the spell learn rate.
        bool isValidNumber = byte.TryParse(parameter, out byte learnRate);

        if (!isValidNumber)
        {
            _errorMessage = $"Invalid number '{parameter}'!";
            return false;
        }

        LearnRate = learnRate;

        // Check if the spell learn rate is within a valid range.
        if (LearnRate >= MIN_SPELL_LEARN_RATE &&
            LearnRate <= MAX_SPELL_LEARN_RATE)
        {
            _isValid = isValidNumber;
            return true;
        }
        else
        {
            _errorMessage = $"Invalid learning rate '{parameter}'! Valid range: {MIN_SPELL_LEARN_RATE}-{MAX_SPELL_LEARN_RATE}.";
            return false;
        }
    }

    private bool IsItemValidForSpellCastingEffect()
    {
        // Check for correct item type.
        switch (_itemEffect)
        {
            case ItemEffect.proc:
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

    private void SetSpellNameArgs(string[] parameters)
    {
        byte nameSize = (byte)(SpellRomData.BlockSize - 1); // Ignore spell icon.
        _isValid = TryGetName(parameters, nameSize, out _newSpellName);
    }

    private void SetItemNameArgs(string[] parameters)
    {
        byte nameSize = (byte)(ItemRomData.BlockSize - 1); // Ignore item icon.
        _isValid = TryGetName(parameters, nameSize, out _newItemName);
    }

    private bool TryGetName(string[] parameters, byte maxValidNameLength, out string name)
    {
        name = string.Empty;

        // Concatenate item name.
        for (int i = 0; i < parameters.Length; i++)
        {
            name += parameters[i] + " ";
        }

        // Remove trailing whitespaces.
        name = name.TrimEnd();

        // Check for invalid characters
        bool isNameValid = IsNameValid(name, maxValidNameLength);
        if (!isNameValid)
        {
            _errorMessage = $"Item name {name} invalid!";
            return false;
        }

        return true;
    }

    private void SetWindowArgs(string effect)
    {
        bool isValidWindowEffect = Enum.TryParse(effect, true, out _windowEffect);
        
        if (!isValidWindowEffect)
        {
            _errorMessage = $"Invalid window effect '{effect}'!";
            return;
        }
        
        _isValid = isValidWindowEffect;
    }

    private void SetCharacterNameArgs(string[] parameters)
    {
        byte nameSize = CHARACTER_DATA_NAME_SIZE;
        _isValid = TryGetName(parameters, nameSize, out _newCharacterName);
    }

    private void SetGPArgs(string effect, string[] parameters)
    {
        // Get GP effect.
        bool isValidGPEffect = Enum.TryParse(effect, true, out _gpEffect);
        if (!isValidGPEffect)
        {
            // If not valid, return.
            _errorMessage = $"Invalid GP command: '{effect}'!";
            return;
        }
        else if (_gpEffect == GPEffect.empty)
        {
            // If it's empty GP, mark as valid and return.
            _isValid = isValidGPEffect;
            return;
        }

        // If it's modify GP effect:
        if (parameters.Length == 0)
        {
            _errorMessage = "GP amount not specified!";
            return;
        }
        
        bool isValidGPRange = false;
        
        string gpParameter = parameters[0];
        
        // Check that GP amount is an actual number.
        bool isValidNumber = Int32.TryParse(gpParameter, out int gpAmount);

        if (!isValidNumber)
        {
            // If not a valid number, return.
            _errorMessage = $"GP amount '{gpParameter}' not a valid number!";
            return;
        }

        // Check if the GP amount is within a valid range.
        if (gpAmount >= GP_AMOUNT_MIN &&
            gpAmount <= GP_AMOUNT_MAX)
        {
            isValidGPRange = true;
            GPAmount = gpAmount;
        }
        else
        {
            // If not a valid GP range, return.
            _errorMessage = $"GP amount '{gpParameter}' out of range - valid range: {GP_AMOUNT_MIN}-{GP_AMOUNT_MAX}";
            return;
        }

        _isValid = isValidGPEffect && isValidGPRange;
    }

    /// <summary>
    /// Check if a name is valid.
    /// </summary>
    /// <param name="newName">The name to check.</param>
    /// <param name="nameSize">The size to compare against.</param>
    /// <returns>True if the name is valid, otherwise false.</returns>
    private static bool IsNameValid(string newName, byte nameSize)
    {
        for (int i = 0; i < newName.Length; i++)
        {
            // Only check characters that are going to be written to memory.
            // The remaining ones will be trimmed.
            if (i > nameSize) break;
            
            // Check against valid character names.
            if (!VALID_NAME_CHARACTERS.Contains(newName[i]))
            {
                return false;
            }
        }
        return true;
    }
}