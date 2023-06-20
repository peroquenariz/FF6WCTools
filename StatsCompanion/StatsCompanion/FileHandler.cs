using System.IO;
using System.Text.Json;

namespace StatsCompanion
{
    /// <summary>
    /// Handles all file operations.
    /// </summary>
    internal class FileHandler
    {
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
        private readonly string _appDirectory;
        private readonly string _runsDirectory;
        private readonly string _crashlogDirectory;

        public string AppDirectory { get => _appDirectory; }
        public string RunsDirectory { get => _runsDirectory; }
        public string CrashlogDirectory { get => _crashlogDirectory; }

        public FileHandler()
        {
            _appDirectory = Directory.GetCurrentDirectory();
            _runsDirectory = $"{_appDirectory}\\runs";
            _crashlogDirectory = $"{_appDirectory}\\crashlog";

            CheckDirectory(RunsDirectory);
            CheckDirectory(CrashlogDirectory);
        }

        /// <summary>
        /// Checks if the directories exist, if not, create them.
        /// </summary>
        private void CheckDirectory (string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Writes a string into a text file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="content">The string to write.</param>
        public static void WriteStringToFile (string path, string content)
        {
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Takes the run data and serializes the JSON with pretty format.
        /// </summary>
        /// <param name="runArguments">The run data.</param>
        /// <returns>A formatted JSON string, ready to write to a file.</returns>
        public string SerializeJson(Arguments runArguments)
        {
            string serializedJson = JsonSerializer.Serialize(runArguments, _jsonOptions);
            return serializedJson;
        }
    }
}