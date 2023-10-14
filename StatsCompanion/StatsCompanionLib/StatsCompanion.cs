using System.Reflection;

namespace StatsCompanionLib
{

    public static class StatsCompanion
    {
        private static readonly string _libVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        public static string LibVersion { get => _libVersion; }

        //public StatsCompanion()
        //{
        //    _libVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();
        //}
    }
}
