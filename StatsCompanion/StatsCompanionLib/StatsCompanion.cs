using System.Reflection;

namespace StatsCompanionLib;

/// <summary>
/// Stats Companion core.
/// </summary>
public class StatsCompanion
{
    private readonly string? _libVersion;

    public string? LibVersion { get => _libVersion; }

    public StatsCompanion()
    {
        _libVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    }
}
