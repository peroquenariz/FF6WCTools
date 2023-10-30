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

        // Get all ROM default data.
        _defaultItemData = _sniClient.ReadMemory(ItemData.StartAddress, ItemData.DataSize);
        _defaultSpellData = _sniClient.ReadMemory(SpellData.StartAddress, SpellData.DataSize);
        _defaultEsperData = _sniClient.ReadMemory(EsperData.StartAddress, EsperData.DataSize);
        
        _defaultItemNamesData = _sniClient.ReadMemory(ItemName.StartAddress, ItemName.DataSize);
        _defaultSpellMagicalNamesData = sniClient.ReadMemory(SpellMagicalName.StartAddress, SpellMagicalName.DataSize);
        _defaultSpellEsperNamesData = sniClient.ReadMemory(SpellEsperName.StartAddress, SpellEsperName.DataSize);
        _defaultSpellAttackNamesData = sniClient.ReadMemory(SpellAttackName.StartAddress, SpellAttackName.DataSize);
        _defaultSpellEsperAttackNamesData = sniClient.ReadMemory(SpellEsperAttackName.StartAddress, SpellEsperAttackName.DataSize);
        _defaultSpellDanceNamesData = sniClient.ReadMemory(SpellDanceName.StartAddress, SpellDanceName.DataSize);

        _itemDataList = InitializeData<ItemData>(_defaultItemData, ItemData.BlockCount);
        _spellDataList = InitializeData<SpellData>(_defaultSpellData, SpellData.BlockCount);
        _esperDataList = InitializeData<EsperData>(_defaultEsperData, EsperData.BlockCount);

        _itemNamesList = InitializeData<ItemName>(_defaultItemNamesData, ItemName.BlockCount);
        _spellMagicalNamesList = InitializeData<SpellMagicalName>(_defaultSpellMagicalNamesData, SpellMagicalName.BlockCount);
        _spellEsperNamesList = InitializeData<SpellEsperName>(_defaultSpellEsperNamesData, SpellEsperName.BlockCount);
        _spellAttackNamesList = InitializeData<SpellAttackName>(_defaultSpellAttackNamesData, SpellAttackName.BlockCount);
        _spellEsperAttackNamesList = InitializeData<SpellEsperAttackName>(_defaultSpellEsperAttackNamesData, SpellEsperAttackName.BlockCount);
        _spellDanceNamesList = InitializeData<SpellDanceName>(_defaultSpellDanceNamesData, SpellDanceName.BlockCount);
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
    /// Generic data instantiator.
    /// </summary>
    /// <typeparam name="T">The type of data to instantiate.</typeparam>
    /// <param name="defaultData">Byte data section.</param>
    /// <param name="blockCount">Amount of items to instantiate.</param>
    /// <returns>A list with the data objects.</returns>
    public List<T> InitializeData<T>(byte[] defaultData, int blockCount) where T : BaseData
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

    private void MirrorAllItemNames()
    {
        List<byte> mirroredDataList = new();

        foreach (var item in _itemNamesList)
        {
            item.Mirror();
            byte[] mirroredData = item.ToByteArray();
            for (int i = 0; i < mirroredData.Length; i++)
            {
                mirroredDataList.Add(mirroredData[i]);
            }
        }

        _sniClient.WriteMemory(ItemName.StartAddress, mirroredDataList.ToArray());
    }
}