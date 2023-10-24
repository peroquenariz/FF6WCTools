using FF6WCToolsLib;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CrowdControlLib;

public class CrowdControl
{
    private readonly SniClient _sniClient;
    private readonly string? _libVersion;
    
    public string? LibVersion { get => _libVersion; }

    public CrowdControl(SniClient sniClient)
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _sniClient = sniClient;
    }

    public async Task ExecuteAsync()
    {
        while (true)
        {
            await Task.Delay(5000);
        }
    }
}