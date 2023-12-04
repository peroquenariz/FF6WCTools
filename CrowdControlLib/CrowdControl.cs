using FF6WCToolsLib;
using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FF6WCToolsLib.DataTemplates;
using TwitchChatbot;
using static CrowdControlLib.CrowdControlEffects;

namespace CrowdControlLib;

public class CrowdControl
{
    private readonly SniClient _sniClient;
    private readonly string? _libVersion;

    private readonly List<CrowdControlMessage> _crowdControlMessageQueue;

    private readonly byte[] _defaultSpellData;
    private readonly byte[] _defaultItemData;
    private readonly byte[] _defaultEsperData;

    private readonly byte[] _defaultItemNamesData;
    private readonly byte[] _defaultSpellMagicalNamesData;
    private readonly byte[] _defaultSpellEsperNamesData;
    private readonly byte[] _defaultSpellAttackNamesData;
    private readonly byte[] _defaultSpellEsperAttackNamesData;
    private readonly byte[] _defaultSpellDanceNamesData;

    private readonly List<SpellRomData> _spellDataList;
    private readonly List<ItemRomData> _itemDataList;
    private readonly List<EsperRomData> _esperDataList;
    private readonly List<CharacterRamData> _characterDataList;
    private readonly List<CharacterSpellRamData> _characterSpellDataList;

    private readonly List<ItemName> _itemNamesList;
    private readonly List<SpellMagicalName> _spellMagicalNamesList;
    private readonly List<SpellEsperName> _spellEsperNamesList;
    private readonly List<SpellAttackName> _spellAttackNamesList;
    private readonly List<SpellEsperAttackName> _spellEsperAttackNamesList;
    private readonly List<SpellDanceName> _spellDanceNamesList;

    private readonly InventoryRamData _inventory;

    private readonly List<string> _communityNames = new()
    {
        "PLEX",
        "SQUACK",
        "POOTS",
        "GAAHR",
        "PERO",
        "DRINKS",
        "DR. DT",
        "BOOGAR",
        "FIKTAH",
        "DBLDWN",
        "MOGRAE",
        "RAV",
        "KATT",
        "THORN",
        "OBI",
        "FCNHIT",
        "HUNNY",
        "ORGAN",
        "RAPHA",
        "SHDWCD",
        "TACOMG",
        "SABRE",
    };

    private readonly Dictionary<Effect, Action<CrowdControlArgs>> _commands;
    private readonly CommandHandler _commandHandler;
    private static readonly Random _rng = new Random();

    public string? LibVersion { get => _libVersion; }
    public static Random Rng => _rng;

    public CrowdControl(SniClient sniClient, Chatbot chatbot)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;

        _crowdControlMessageQueue = new List<CrowdControlMessage>();

        // Get all ROM default data.
        _defaultItemData = _sniClient.ReadMemory(ItemRomData.StartAddress, ItemRomData.DataSize);
        _defaultSpellData = _sniClient.ReadMemory(SpellRomData.StartAddress, SpellRomData.DataSize);
        _defaultEsperData = _sniClient.ReadMemory(EsperRomData.StartAddress, EsperRomData.DataSize);

        _defaultItemNamesData = _sniClient.ReadMemory(ItemName.StartAddress, ItemName.DataSize);
        _defaultSpellMagicalNamesData = _sniClient.ReadMemory(SpellMagicalName.StartAddress, SpellMagicalName.DataSize);
        _defaultSpellEsperNamesData = _sniClient.ReadMemory(SpellEsperName.StartAddress, SpellEsperName.DataSize);
        _defaultSpellAttackNamesData = _sniClient.ReadMemory(SpellAttackName.StartAddress, SpellAttackName.DataSize);
        _defaultSpellEsperAttackNamesData = _sniClient.ReadMemory(SpellEsperAttackName.StartAddress, SpellEsperAttackName.DataSize);
        _defaultSpellDanceNamesData = _sniClient.ReadMemory(SpellDanceName.StartAddress, SpellDanceName.DataSize);

        // Initialize data. Stuff in game RAM initializes empty and gets updated when needed.
        _itemDataList = InitializeData<ItemRomData>(_defaultItemData, ItemRomData.BlockCount);
        _spellDataList = InitializeData<SpellRomData>(_defaultSpellData, SpellRomData.BlockCount);
        _esperDataList = InitializeData<EsperRomData>(_defaultEsperData, EsperRomData.BlockCount);
        _characterDataList = InitializeRamData<CharacterRamData>(CharacterRamData.BlockCount);
        _characterSpellDataList = InitializeRamData<CharacterSpellRamData>(CharacterSpellRamData.BlockCount);
        _inventory = new InventoryRamData();

        _itemNamesList = InitializeData<ItemName>(_defaultItemNamesData, ItemName.BlockCount);
        _spellMagicalNamesList = InitializeData<SpellMagicalName>(_defaultSpellMagicalNamesData, SpellMagicalName.BlockCount);
        _spellEsperNamesList = InitializeData<SpellEsperName>(_defaultSpellEsperNamesData, SpellEsperName.BlockCount);
        _spellAttackNamesList = InitializeData<SpellAttackName>(_defaultSpellAttackNamesData, SpellAttackName.BlockCount);
        _spellEsperAttackNamesList = InitializeData<SpellEsperAttackName>(_defaultSpellEsperAttackNamesData, SpellEsperAttackName.BlockCount);
        _spellDanceNamesList = InitializeData<SpellDanceName>(_defaultSpellDanceNamesData, SpellDanceName.BlockCount);

        _commands = new Dictionary<Effect, Action<CrowdControlArgs>>()
        {
            { Effect.window, ModifyWindow },
            { Effect.item, ModifyItem },
            { Effect.spell, ModifySpell },
            { Effect.character, ModifyCharacter },
            { Effect.itemname, ModifyItemName },
            { Effect.spellname, ModifySpellName },
            { Effect.charactername, ModifyCharacterName },
            { Effect.gp, ModifyGP },
            { Effect.inventory, ModifyInventory },
            { Effect.mirror, MirrorAllItemNames }
        };

        DataHandler.InitializeInverseCharDict();

        // Subscribe to chat messages event.
        chatbot.OnCrowdControlMessageReceived += Chatbot_OnCrowdControlMessageReceived;
        
        // Instantiate the command handler and pass the list of crowd control commands.
        _commandHandler = new CommandHandler(_commands);
        
        // Send command notifications to chat.
        _commandHandler.OnSuccessfulEffectLoaded += chatbot.CrowdControl_OnSuccessfulEffectLoaded;
        _commandHandler.OnFailedEffect += chatbot.CrowdControl_OnFailedEffect;

        InitializeCommunityNames();
    }

    private void Chatbot_OnCrowdControlMessageReceived(object? sender, MessageEventArgs e)
    {
        _crowdControlMessageQueue.Add(new CrowdControlMessage(e.User, e.Message));
    }

    private void InitializeCommunityNames()
    {
        // Rename Fenrir to SabrWolf - arooooo!
        SpellEsperName fenrir = _spellEsperNamesList[(int)Esper.Fenrir];
        fenrir.Rename(DataHandler.EncodeName("SabrWolf", SpellEsperName.BlockSize));
        _sniClient.WriteMemory(fenrir);

        // Rename Fenrir's spell to "Pack Call" - arooooo again!
        // Spaces for esper attack names are 0xFE, go figure.
        SpellEsperAttackName fenrirSpell = _spellEsperAttackNamesList[(int)Esper.Fenrir];
        fenrirSpell.Rename(DataHandler.EncodeName("Pack Call", SpellEsperAttackName.BlockSize, 0xFE));
        _sniClient.WriteMemory(fenrirSpell);

        // Rename Palidor's spell to "Falcon Hit".
        SpellEsperAttackName palidorSpell = _spellEsperAttackNamesList[(int)Esper.Palidor];
        palidorSpell.Rename(DataHandler.EncodeName("Falcon Hit", SpellEsperAttackName.BlockSize, 0xFE));
        _sniClient.WriteMemory(palidorSpell);
    }

    public async Task ExecuteAsync()
    {
        await Console.Out.WriteLineAsync("Running...");

        try
        {
            // TODO: add timer shenanigans.

            while (true)
            {
                // Check that the queue isn't empty.
                if (_crowdControlMessageQueue.Count != 0)
                {
                    // Attempt to parse the oldest message.
                    bool wasEffectLoaded = _commandHandler.TryLoadEffect(_crowdControlMessageQueue[0]);

                    // Remove oldest message from the list. TODO: don't remove if they're time or currency throttled.
                    _crowdControlMessageQueue.RemoveAt(0);
                }
                await Task.Delay(1000);
            }
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.ToString());
        }
    }

    /// <summary>
    /// Generic data instantiator.
    /// </summary>
    /// <typeparam name="T">The type of data to instantiate.</typeparam>
    /// <param name="blockCount">Amount of items to instantiate.</param>
    /// <returns>A list with the data objects.</returns>
    private List<T> InitializeRamData<T>(int blockCount) where T : BaseRamData
    {
        List<T> dataList = new List<T>();

        for (int i = 0; i < blockCount; i++)
        {
            T data = (T)Activator.CreateInstance(typeof(T), new object[] { i })!;

            dataList.Add(data);
        }

        return dataList;
    }

    /// <summary>
    /// Generic data instantiator.
    /// </summary>
    /// <typeparam name="T">The type of data to instantiate.</typeparam>
    /// <param name="defaultData">Byte data section.</param>
    /// <param name="blockCount">Amount of items to instantiate.</param>
    /// <returns>A list with the data objects.</returns>
    private List<T> InitializeData<T>(byte[] defaultData, int blockCount) where T : BaseRomData
    {
        List<T> dataList = new List<T>();

        int blockSize = defaultData.Length / blockCount;

        for (int i = 0; i < blockCount; i++)
        {
            int start = i * blockSize;
            int end = (i + 1) * blockSize;

            // Create an instance of T by calling the constructor with two parameters.
            T data = (T)Activator.CreateInstance(typeof(T), new object[] { defaultData[start..end], i })!;

            dataList.Add(data);
        }

        return dataList;
    }

    private void ModifyInventory(CrowdControlArgs args)
    {
        throw new NotImplementedException();
    }

    private void ModifyGP(CrowdControlArgs args)
    {
        const int MIN_GP = 0;
        const int MAX_GP = 999999;

        if (args.GPEffect == GPEffect.modify)
        {
            int currentGP = DataHandler.ConcatenateByteArray(_sniClient.ReadMemory(CURRENT_GP_START, CURRENT_GP_SIZE));

            currentGP += args.GPAmount;
            if (currentGP < MIN_GP)
            {
                currentGP = MIN_GP;
            }
            else if (currentGP > MAX_GP)
            {
                currentGP = MAX_GP;
            }
            _sniClient.WriteMemory(CURRENT_GP_START, DataHandler.DecatenateInteger(currentGP, 3));
        }
        else if (args.GPEffect == GPEffect.empty)
        {
            _sniClient.WriteMemory(CURRENT_GP_START, DataHandler.DecatenateInteger(MIN_GP, 3));
        }
    }

    private void ModifySpellName(CrowdControlArgs args)
    {
        SpellMagicalName targetSpellName = _spellMagicalNamesList[(int)args.Spell];
        string newSpellName = args.NewSpellName;
        int nameBytesSize = SpellMagicalName.BlockSize - 1; // Subtract 1 for the spell icon
        
        byte[] nameBytes = DataHandler.EncodeName(newSpellName, nameBytesSize);

        targetSpellName.Rename(nameBytes, true);
        _sniClient.WriteMemory(targetSpellName);
    }

    private void ModifyItemName(CrowdControlArgs args)
    {
        ItemName targetItemName = _itemNamesList[(int)args.Item];
        string newItemName = args.NewItemName;
        int nameBytesSize = ItemName.BlockSize - 1; // Subtract 1 for the item icon
        
        byte[] nameBytes = DataHandler.EncodeName(newItemName, nameBytesSize);

        targetItemName.Rename(nameBytes, true);
        _sniClient.WriteMemory(targetItemName);
    }

    private void ModifyCharacter(CrowdControlArgs args)
    {
        // Spell teach/forget effect.
        if (args.CharacterEffect is CharacterEffect.forget or CharacterEffect.teach)
        {
            bool isSpellTeachEffect = args.CharacterEffect == CharacterEffect.teach;
            byte spellLearnedValue = (byte)(isSpellTeachEffect ? 0xFF : 0x00);

            uint targetSpellAddress = CharacterSpellRamData.GetSpellAddress(args.Spell, args.Character);

            _sniClient.WriteMemory(targetSpellAddress, new byte[] { spellLearnedValue });

            return;
        }
        
        CharacterRamData targetCharacter = _characterDataList[(int)args.Character];
        
        // Equipment effect.
        if (Enum.IsDefined(typeof(EquipmentSlot), args.CharacterEffect.ToString())) // TODO: get rid of string checking?
        {
            // TODO: this method is static because right now it's just overwriting the existing item in the slot.
            // We don't need to read the character or access any instance data.
            // In the future, it might be nice to implement a method that puts the currently held item in the inventory,
            // and equips the given one.
            uint equipmentAddress = CharacterRamData.GetEquipmentAddress(args.Character, args.EquipmentSlot);
            byte itemValue = (byte)args.Item;

            _sniClient.WriteMemory(equipmentAddress, new byte[] { itemValue });
        }
        // Stat boost effect.
        else if (Enum.IsDefined(typeof(Stat), args.CharacterEffect.ToString()))
        {
            // Read current character data.
            targetCharacter.UpdateData(_sniClient.ReadMemory(targetCharacter));

            // Update stat.
            targetCharacter.SetStatBoost(args.StatBoostType, args.StatBoostValue);

            // Write new character data to memory.
            _sniClient.WriteMemory(targetCharacter);
        }
        else
        {
            throw new NotImplementedException();
            // TODO: event that logs error in console.
        }
    }

    private void ModifySpell(CrowdControlArgs args)
    {
        SpellRomData targetSpell = _spellDataList[(int)args.Spell];
        
        switch (args.SpellEffect)
        {
            case SpellEffect.reset:
                targetSpell.ResetData();
                break;
            case SpellEffect.targeting:
                targetSpell.ModifyTargeting(args.TargetingPreset);
                break;
            case SpellEffect.spellpower:
                targetSpell.SetSpellPower(args.SpellPower);
                break;
            case SpellEffect.element:
                targetSpell.ToggleElement(args.Element);
                break;
            case SpellEffect.mpdamage:
                targetSpell.ToggleMPDamage();
                break;
            case SpellEffect.ignoredefense:
                targetSpell.ToggleIgnoreDefense();
                break;
            case SpellEffect.mpcost:
                targetSpell.SetMPCost(args.SpellMPCost);
                break;
            case SpellEffect.status:
                targetSpell.ToggleStatus(args.StatusEffectFlag, args.StatusEffectByteOffset);
                break;
            case SpellEffect.liftstatus:
                targetSpell.ToggleLiftStatus();
                break;
            default:
                throw new NotImplementedException(); // TODO: event that logs error in console.
        }

        _sniClient.WriteMemory(targetSpell);
    }

    private void ModifyItem(CrowdControlArgs args)
    {
        ItemRomData targetItem = _itemDataList[(int)args.Item];

        switch (args.ItemEffect)
        {
            case ItemEffect.reset:
                targetItem.ResetData();
                break;
            case ItemEffect.spellproc:
                targetItem.SpellProc(_spellDataList[(int)args.Spell]);
                break;
            case ItemEffect.breakable:
                targetItem.Breakable(_spellDataList[(int)args.Spell]);
                break;
            case ItemEffect.teach:
                targetItem.TeachSpell(_spellDataList[(int)args.Spell], args.LearnRate);
                break;
            case ItemEffect.reliceffect:
                targetItem.AddRelicEffect(_itemDataList[(int)args.RelicEffectItem]);
                break;
            case ItemEffect.statboost:
                targetItem.SetStatBoost(args.StatBoostType, args.StatBoostValue);
                break;
            case ItemEffect.absorb:
                targetItem.ToggleElementalProperty(args.Element, (int)ItemDataStructure.Status2__AbsorbElement);
                break;
            case ItemEffect.nullify:
                targetItem.ToggleElementalProperty(args.Element, (int)ItemDataStructure.Status3__NullifyElement);
                break;
            case ItemEffect.weak:
                targetItem.ToggleElementalProperty(args.Element, (int)ItemDataStructure.Status4__WeakElement);
                break;
            case ItemEffect.weaponelement:
                targetItem.ToggleElementalProperty(args.Element, (int)ItemDataStructure.WeaponElement__HalveElement);
                break;
            case ItemEffect.price:
                targetItem.SetPrice(args.GPAmount);
                break;
            default:
                throw new NotImplementedException(); // Something is really wrong if we're hitting this.
        }

        _sniClient.WriteMemory(targetItem);
    }

    private void MirrorAllItemNames(CrowdControlArgs args) // TODO: refactor to mirror any name data type?
    {
        List<byte> mirroredDataList = new();

        foreach (var item in _itemNamesList)
        {
            item.Mirror(true);
            byte[] mirroredData = item.ToByteArray();
            for (int i = 0; i < mirroredData.Length; i++)
            {
                mirroredDataList.Add(mirroredData[i]);
            }
        }
        
        _sniClient.WriteMemory(ItemName.StartAddress, mirroredDataList.ToArray());
    }

    private void ModifyCharacterName(CrowdControlArgs args) // TODO: move character name to its own class????
    {
        string newCharacterName = args.NewCharactername;
        
        byte[] nameData;

        if (newCharacterName.ToLower() == "community")
        {
            // Take a random name from the community list.
            string communityName = _communityNames[_rng.Next(0, _communityNames.Count)];

            nameData = DataHandler.EncodeName(communityName, CHARACTER_DATA_NAME_SIZE);
        }
        else // Not a community name
        {
            nameData = DataHandler.EncodeName(newCharacterName, CHARACTER_DATA_NAME_SIZE);
        }

        // Write byte array to memory.
        uint characterNameIndex = CharacterRamData.StartAddress + (uint)CharacterDataStructure.Name + (CharacterRamData.BlockSize * (uint)args.Character);
        _sniClient.WriteMemory(characterNameIndex, nameData);
    }

    private void ModifyWindow(CrowdControlArgs args)
    {
        byte[] wallpaper = _sniClient.ReadMemory(WALLPAPER, 1);
        wallpaper[0] &= 0xF0;
        
        if (args.WindowEffect == WindowEffect.random)
        {
            wallpaper[0] |= (byte)_rng.Next(0, 8); // Randomly select a wallpaper

            byte[] colors = new byte[114]; // Font + all window colors
            _rng.NextBytes(colors);

            _sniClient.WriteMemory(WALLPAPER, wallpaper);
            _sniClient.WriteMemory(FONT_COLOR, colors);
        }
        else if (args.WindowEffect == WindowEffect.demonchocobo)
        {
            // Select chocobo window
            byte chocoboWindowIndex = 7;
            wallpaper[0] |= chocoboWindowIndex;
            _sniClient.WriteMemory(WALLPAPER, wallpaper);
            
            // Write demon chocobo colors to chocobo window
            byte[] demonChocoboPalette = new byte[] { 0x1f, 0x7c, 0x00, 0x00, 0x1f, 0x7c, 0xe0, 0x03, 0xe0, 0x03, 0x74, 0x01, 0x84, 0x10 };
            uint chocoboWindowAddress = WINDOW_COLOR_START + (WINDOW_COLOR_BLOCK_SIZE * (uint)chocoboWindowIndex);
            _sniClient.WriteMemory(chocoboWindowAddress, demonChocoboPalette);
        }
        else if (args.WindowEffect == WindowEffect.vanilla)
        {
            // Select default window
            byte chocoboWindowIndex = 0;
            wallpaper[0] |= chocoboWindowIndex;
            _sniClient.WriteMemory(WALLPAPER, wallpaper);

            // Write default colors to the default window.
            byte[] defaultPalette = new byte[] { 0xff, 0x7f, 0x99, 0x73, 0xd4, 0x5a, 0x10, 0x42, 0x4a, 0x29, 0xc5, 0x18, 0xc6, 0x44, 0xa5, 0x40 };
            uint defaultWindowAddress = FONT_COLOR;
            _sniClient.WriteMemory(defaultWindowAddress, defaultPalette);
        }
    }
}