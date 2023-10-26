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

    private readonly byte[] _defaultSpellData;
    private readonly byte[] _defaultItemData;

    private readonly List<SpellData> _spellDataList;
    private readonly List<ItemData> _itemDataList;
    

    public string? LibVersion { get => _libVersion; }

    public CrowdControl(SniClient sniClient, List<Message> crowdControlMessageQueue)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;
        _crowdControlMessageQueue = crowdControlMessageQueue;

        _spellDataList = new List<SpellData>();
        _itemDataList = new List<ItemData>();

        // Get all ROM default data.
        _defaultSpellData = _sniClient.ReadMemory(SPELL_DATA_START, SPELL_DATA_BLOCK_COUNT * SPELL_DATA_BLOCK_SIZE);
        _defaultItemData = _sniClient.ReadMemory(ITEM_DATA_START, ITEM_DATA_BLOCK_COUNT * ITEM_DATA_BLOCK_SIZE);
        
        // Create data structures.
        InitializeData();
    }

    /// <summary>
    /// Populates data lists with current loaded ROM data.
    /// </summary>
    private void InitializeData()
    {
        for (int i = 0; i < SPELL_DATA_BLOCK_COUNT; i++)
        {
            int spellStart = i * SPELL_DATA_BLOCK_SIZE;
            int spellEnd = (i+1) * SPELL_DATA_BLOCK_SIZE;
            _spellDataList.Add(new SpellData(_defaultSpellData[spellStart..spellEnd], i));
        }

        for (int i = 0; i < ITEM_DATA_BLOCK_COUNT; i++)
        {
            int itemStart = i * ITEM_DATA_BLOCK_SIZE;
            int itemEnd = (i + 1) * ITEM_DATA_BLOCK_SIZE;
            _itemDataList.Add(new ItemData(_defaultItemData[itemStart..itemEnd], i));
        }
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

        //await Console.Out.WriteLineAsync(zeroCount.ToString());

        while (true)
        {
            await Task.Delay(5000);
        }
    }
}