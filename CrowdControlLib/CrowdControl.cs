using FF6WCToolsLib;
using static FF6WCToolsLib.WCData;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Message = TwitchChatbot.Message;
using FF6WCToolsLib.DataTemplates;

namespace CrowdControlLib;

public class CrowdControl
{
    private readonly SniClient _sniClient;
    private readonly string? _libVersion;

    private List<Message> _crowdControlMessageQueue;

    private readonly byte[] _defaultItemData;
    //private readonly List<EsperData> _esperDataList;
    private readonly byte[] _defaultSpellData;
    
    private readonly byte[] _defaultItemNamesData;
    private readonly byte[] _defaultSpellMagicalNamesData;
    private readonly byte[] _defaultSpellEsperNamesData;
    private readonly byte[] _defaultSpellAttackNamesData;
    private readonly byte[] _defaultSpellEsperAttackNamesData;
    private readonly byte[] _defaultSpellDanceNamesData;

    private readonly List<SpellData> _spellDataList;
    private readonly List<ItemData> _itemDataList;
    
    private readonly List<ItemName> _itemNamesList;
    private readonly List<SpellMagicalName> _spellMagicalNamesList;
    private readonly List<SpellEsperName> _spellEsperNamesList;
    private readonly List<SpellAttackName> _spellAttackNamesList;
    private readonly List<SpellEsperAttackName> _spellEsperAttackNamesList;
    private readonly List<SpellDanceName> _spellDanceNamesList;

    public string? LibVersion { get => _libVersion; }

    public CrowdControl(SniClient sniClient, List<Message> crowdControlMessageQueue)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;
        _crowdControlMessageQueue = crowdControlMessageQueue;

        _itemDataList = new List<ItemData>();
        _spellDataList = new List<SpellData>();

        _itemNamesList = new List<ItemName>();
        _spellMagicalNamesList = new List<SpellMagicalName>();
        _spellEsperNamesList = new List<SpellEsperName>();
        _spellAttackNamesList = new List<SpellAttackName>();
        _spellEsperAttackNamesList = new List<SpellEsperAttackName>();
        _spellDanceNamesList = new List<SpellDanceName>();

        // Get all ROM default data.
        _defaultItemData = _sniClient.ReadMemory(ItemData.StartAddress, ItemData.DataSize);
        _defaultSpellData = _sniClient.ReadMemory(SpellData.StartAddress, SpellData.DataSize);
        
        _defaultItemNamesData = _sniClient.ReadMemory(ItemName.StartAddress, ItemName.DataSize);
        _defaultSpellMagicalNamesData = sniClient.ReadMemory(SpellMagicalName.StartAddress, SpellMagicalName.DataSize);
        _defaultSpellEsperNamesData = sniClient.ReadMemory(SpellEsperName.StartAddress, SpellEsperName.DataSize);
        _defaultSpellAttackNamesData = sniClient.ReadMemory(SpellAttackName.StartAddress, SpellAttackName.DataSize);
        _defaultSpellEsperAttackNamesData = sniClient.ReadMemory(SpellEsperAttackName.StartAddress, SpellEsperAttackName.DataSize);
        _defaultSpellDanceNamesData = sniClient.ReadMemory(SpellDanceName.StartAddress, SpellDanceName.DataSize);
        
        // Create data structures.
        InitializeData();
        InitializeNames();
    }

    public async Task ExecuteAsync()
    {
        try
        {
            
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.ToString());
        }

        while (true)
        {
            await Task.Delay(5000);
        }
    }

    /// <summary>
    /// Populates name lists with current loaded ROM data.
    /// </summary>
    private void InitializeNames()
    {
        for (int i = 0; i < ItemName.BlockCount; i++)
        {
            int itemNameStart = i * ItemName.BlockSize;
            int itemNameEnd = (i + 1) * ItemName.BlockSize;
            _itemNamesList.Add(new ItemName(_defaultItemNamesData[itemNameStart..itemNameEnd], i));
        }

        for (int i = 0; i < SpellMagicalName.BlockCount; i++)
        {
            int magicalSpellNameStart = i * SpellMagicalName.BlockSize;
            int magicalSpellNameEnd = (i + 1) * SpellMagicalName.BlockSize;
            _spellMagicalNamesList.Add(new SpellMagicalName(_defaultSpellMagicalNamesData[magicalSpellNameStart..magicalSpellNameEnd], i));
        }

        for (int i = 0; i < SpellEsperName.BlockCount; i++)
        {
            int esperNameStart = i * SpellEsperName.BlockSize;
            int esperNameEnd = (i + 1) * SpellEsperName.BlockSize;
            _spellEsperNamesList.Add(new SpellEsperName(_defaultSpellEsperNamesData[esperNameStart..esperNameEnd], i));
        }

        for (int i = 0; i < SpellAttackName.BlockCount; i++)
        {
            int attackNameStart = i * SpellAttackName.BlockSize;
            int attackNameEnd = (i + 1) * SpellAttackName.BlockSize;
            _spellAttackNamesList.Add(new SpellAttackName(_defaultSpellAttackNamesData[attackNameStart..attackNameEnd], i));
        }
        
        for (int i = 0; i < SpellEsperAttackName.BlockCount; i++)
        {
            int esperAttackNameStart = i * SpellEsperAttackName.BlockSize;
            int esperAttackNameEnd = (i + 1) * SpellEsperAttackName.BlockSize;
            _spellEsperAttackNamesList.Add(new SpellEsperAttackName(_defaultSpellEsperAttackNamesData[esperAttackNameStart..esperAttackNameEnd], i));
        }

        for (int i = 0; i < SpellDanceName.BlockCount; i++)
        {
            int danceNameStart = i * SpellDanceName.BlockSize;
            int danceNameEnd = (i + 1) * SpellDanceName.BlockSize;
            _spellDanceNamesList.Add(new SpellDanceName(_defaultSpellDanceNamesData[danceNameStart..danceNameEnd], i));
        }
    }

    private void ResetNames()
    {
        _itemNamesList.Clear();
        _spellMagicalNamesList.Clear();
        _spellEsperNamesList.Clear();
        _spellAttackNamesList.Clear();
        _spellEsperAttackNamesList.Clear();
        _spellDanceNamesList.Clear();

        InitializeNames();
    }

    /// <summary>
    /// Populates data lists with current loaded ROM data.
    /// </summary>
    private void InitializeData()
    {
        for (int i = 0; i < SpellData.BlockCount; i++)
        {
            int spellStart = i * SpellData.BlockSize;
            int spellEnd = (i + 1) * SpellData.BlockSize;
            _spellDataList.Add(new SpellData(_defaultSpellData[spellStart..spellEnd], i));
        }

        for (int i = 0; i < ItemData.BlockCount; i++)
        {
            int itemStart = i * ItemData.BlockSize;
            int itemEnd = (i + 1) * ItemData.BlockSize;
            _itemDataList.Add(new ItemData(_defaultItemData[itemStart..itemEnd], i));
        }
    }
}