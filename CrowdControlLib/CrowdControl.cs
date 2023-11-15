using FF6WCToolsLib;
using static FF6WCToolsLib.WCData;
using static FF6WCToolsLib.DataTemplates.DataEnums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FF6WCToolsLib.DataTemplates;
using TwitchChatbot;

namespace CrowdControlLib;

public class CrowdControl
{
    private readonly SniClient _sniClient;
    private readonly string? _libVersion;

    private readonly byte[] _defaultSpellData;
    private readonly byte[] _defaultItemData;
    private readonly byte[] _defaultEsperData;

    private readonly byte[] _defaultItemNamesData;
    private readonly byte[] _defaultSpellMagicalNamesData;
    private readonly byte[] _defaultSpellEsperNamesData;
    private readonly byte[] _defaultSpellAttackNamesData;
    private readonly byte[] _defaultSpellEsperAttackNamesData;
    private readonly byte[] _defaultSpellDanceNamesData;

    private readonly List<SpellData> _spellDataList;
    private readonly List<ItemData> _itemDataList;
    private readonly List<EsperData> _esperDataList;
    private readonly List<CharacterData> _characterDataList;

    private readonly List<ItemName> _itemNamesList;
    private readonly List<SpellMagicalName> _spellMagicalNamesList;
    private readonly List<SpellEsperName> _spellEsperNamesList;
    private readonly List<SpellAttackName> _spellAttackNamesList;
    private readonly List<SpellEsperAttackName> _spellEsperAttackNamesList;
    private readonly List<SpellDanceName> _spellDanceNamesList;

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
    };

    public static readonly Dictionary<char, byte> INVERSE_CHAR_DICT = new();

    private readonly Dictionary<Effect, Action<CrowdControlArgs>> _commands;
    private readonly CommandHandler _commandHandler;
    private static Random _rng = new Random();

    public string? LibVersion { get => _libVersion; }
    public static Random Rng => _rng;

    public CrowdControl(SniClient sniClient, Chatbot chatbot)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;

        // Get all ROM default data.
        _defaultItemData = _sniClient.ReadMemory(ItemData.StartAddress, ItemData.DataSize);
        _defaultSpellData = _sniClient.ReadMemory(SpellData.StartAddress, SpellData.DataSize);
        _defaultEsperData = _sniClient.ReadMemory(EsperData.StartAddress, EsperData.DataSize);
        
        _defaultItemNamesData = _sniClient.ReadMemory(ItemName.StartAddress, ItemName.DataSize);
        _defaultSpellMagicalNamesData = _sniClient.ReadMemory(SpellMagicalName.StartAddress, SpellMagicalName.DataSize);
        _defaultSpellEsperNamesData = _sniClient.ReadMemory(SpellEsperName.StartAddress, SpellEsperName.DataSize);
        _defaultSpellAttackNamesData = _sniClient.ReadMemory(SpellAttackName.StartAddress, SpellAttackName.DataSize);
        _defaultSpellEsperAttackNamesData = _sniClient.ReadMemory(SpellEsperAttackName.StartAddress, SpellEsperAttackName.DataSize);
        _defaultSpellDanceNamesData = _sniClient.ReadMemory(SpellDanceName.StartAddress, SpellDanceName.DataSize);

        // Initialize data. Stuff in game RAM initializes empty and gets updated when needed.
        _itemDataList = InitializeData<ItemData>(_defaultItemData, ItemData.BlockCount);
        _spellDataList = InitializeData<SpellData>(_defaultSpellData, SpellData.BlockCount);
        _esperDataList = InitializeData<EsperData>(_defaultEsperData, EsperData.BlockCount);
        _characterDataList = InitializeRamData<CharacterData>(CharacterData.BlockCount);

        _itemNamesList = InitializeData<ItemName>(_defaultItemNamesData, ItemName.BlockCount);
        _spellMagicalNamesList = InitializeData<SpellMagicalName>(_defaultSpellMagicalNamesData, SpellMagicalName.BlockCount);
        _spellEsperNamesList = InitializeData<SpellEsperName>(_defaultSpellEsperNamesData, SpellEsperName.BlockCount);
        _spellAttackNamesList = InitializeData<SpellAttackName>(_defaultSpellAttackNamesData, SpellAttackName.BlockCount);
        _spellEsperAttackNamesList = InitializeData<SpellEsperAttackName>(_defaultSpellEsperAttackNamesData, SpellEsperAttackName.BlockCount);
        _spellDanceNamesList = InitializeData<SpellDanceName>(_defaultSpellDanceNamesData, SpellDanceName.BlockCount);

        _commands = new Dictionary<Effect, Action<CrowdControlArgs>>()
        {
            { Effect.window, ModifyFontAndWindow },
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

        InitializeDicts();

        _commandHandler = new CommandHandler(_commands, chatbot.CrowdControlMessageQueue);

        _commandHandler.OnSuccessfulEffectLoaded += chatbot.CrowdControl_OnSuccessfulEffectLoaded;
        _commandHandler.OnFailedEffect += chatbot.CrowdControl_OnFailedEffect;
    }

    private void InitializeDicts()
    {
        // Build inverse character dictionary
        for (byte i = 0x80; i < 0xC6; i++)
        {
            CHAR_TO_BYTE_DICT.Add(CHAR_DICT[i], i);
        }
        CHAR_TO_BYTE_DICT.Add(' ', 0xFF);
    }

    public async Task ExecuteAsync()
    {
        try
        {
            while (true)
            {
                bool wasEffectLoaded = _commandHandler.TryLoadEffect();
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

    private void UpdateCharacterData() // TODO: refactor to use BaseRamData.UpdateData()
    {
        byte[] characterData = _sniClient.ReadMemory(CharacterData.StartAddress, CharacterData.DataSize);

        for (int i = 0; i < CharacterData.BlockCount; i++)
        {
            _characterDataList[i].UpdateData(characterData[(i * CharacterData.BlockSize)..((i + 1) * CharacterData.BlockSize)]);
        }
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
        // TODO: filter the spell to rename
        // Right now, only magical spells are allowed.
        if ((int)args.Spell > 53)
        {
            return;
        }
        
        SpellMagicalName targetSpellName = _spellMagicalNamesList[(int)args.Spell];

        string newSpellName = args.NewSpellName;
        int nameBytesSize = SPELLS_MAGICAL_NAMES_BLOCK_SIZE - 1; // Subtract 1 for the spell icon
        byte[] nameBytes = InitializeArrayWithData(nameBytesSize, CHAR_TO_BYTE_DICT[' ']);

        if (newSpellName.Length > nameBytesSize)
        {
            newSpellName = newSpellName[..nameBytesSize];
        }

        for (int i = 0; i < newSpellName.Length; i++) // TODO: extract to a method
        {
            nameBytes[i] = CHAR_TO_BYTE_DICT[newSpellName[i]];
        }

        targetSpellName.Rename(nameBytes, true);
        _sniClient.WriteMemory(targetSpellName);
    }

    private void ModifyItemName(CrowdControlArgs args)
    {
        ItemName targetItemName = _itemNamesList[(int)args.Item];
        string newItemName = args.NewItemName;
        int nameBytesSize = ITEM_NAMES_BLOCK_SIZE - 1; // Subtract 1 for the item icon
        byte[] nameBytes = InitializeArrayWithData(nameBytesSize, CHAR_TO_BYTE_DICT[' ']);

        if (newItemName.Length > nameBytesSize)
        {
            newItemName = newItemName[..nameBytesSize];
        }

        for (int i = 0; i < newItemName.Length; i++) // TODO: extract to a method
        {
            nameBytes[i] = CHAR_TO_BYTE_DICT[newItemName[i]];
        }

        targetItemName.Rename(nameBytes, true);
        _sniClient.WriteMemory(targetItemName);
    }

    private void ModifyCharacter(CrowdControlArgs args)
    {
        throw new NotImplementedException();
    }

    private void ModifySpell(CrowdControlArgs args)
    {
        throw new NotImplementedException();
    }

    private void ModifyItem(CrowdControlArgs args)
    {
        ItemData targetItem = _itemDataList[(int)args.Item];

        switch (args.ItemEffect)
        {
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
                targetItem.AddRelicEffect(args.RelicEffectItem);
                break;
            case ItemEffect.statboost:
                break;
            case ItemEffect.absorb:
                targetItem.SetElementAbsorb(args.Element);
                break;
            case ItemEffect.nullify:
                targetItem.SetElementNullify(args.Element);
                break;
            case ItemEffect.weak:
                targetItem.SetElementWeakness(args.Element);
                break;
            default:
                break;
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
        
        // Initialize byte array with whitespaces.
        byte[] nameData = InitializeArrayWithData(CHARACTER_DATA_NAME_SIZE, CHAR_TO_BYTE_DICT[' ']);

        // Build byte array with the given name.
        if (newCharacterName.ToLower() == "community")
        {
            // Take a random name from the community list.
            // This list is sanitized to only have correct length names, no need to check here.
            string communityName = _communityNames[_rng.Next(0, _communityNames.Count)];

            for (int i = 0; i < communityName.Length; i++)
            {
                nameData[i] = CHAR_TO_BYTE_DICT[communityName[i]];
            }
        }
        else // Not a community name
        {
            // If string is longer than 6 characters, truncate it.
            if (newCharacterName.Length > CHARACTER_DATA_NAME_SIZE)
            {
                newCharacterName = newCharacterName[..CHARACTER_DATA_NAME_SIZE];
            }

            for (int i = 0; i < newCharacterName.Length; i++)
            {
                nameData[i] = CHAR_TO_BYTE_DICT[newCharacterName[i]];
            }
        }

        // Write byte array to memory.
        uint characterNameIndex = CharacterData.StartAddress + (uint)CharacterDataStructure.Name + (CharacterData.BlockSize * (uint)args.Character);
        _sniClient.WriteMemory(characterNameIndex, nameData);
    }

    /// <summary>
    /// Creates an array filled with a given type of data.
    /// </summary>
    /// <param name="arraySize">The size of the array to initialize.</param>
    /// <param name="defaultData">The byte to fill the array with.</param>
    /// <returns>An array initialized with a given byte.</returns>
    private static byte[] InitializeArrayWithData(int arraySize, byte defaultData)
    {
        byte[] byteArray = new byte[arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            byteArray[i] = defaultData;
        }

        return byteArray;
    }

    private void ModifyFontAndWindow(CrowdControlArgs args)
    {
        byte[] wallpaper = _sniClient.ReadMemory(0x7E1D4E, 1);
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