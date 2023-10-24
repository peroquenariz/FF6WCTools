using FF6WCToolsLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Message = TwitchChatbot.Message;

namespace CrowdControlLib;

public class CrowdControl
{
    private readonly SniClient _sniClient;
    private readonly string? _libVersion;

    private List<Message> _crowdControlMessageQueue;
    
    public string? LibVersion { get => _libVersion; }

    public CrowdControl(SniClient sniClient, List<Message> crowdControlMessageQueue)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;

        _crowdControlMessageQueue = crowdControlMessageQueue;
    }

    public async Task ExecuteAsync()
    {
        while (true)
        {
            await Task.Delay(5000);
        }
    }
}