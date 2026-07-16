using System.Text.Json;

namespace Hospital.Back
{
    public static class JsonFileService
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        public static List<T> Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }
            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        public static void Save<T>(string filePath, List<T> data)
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(filePath, json);
        }

        public static void AppendLine(string filePath, string line)
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.AppendAllText(filePath, line + Environment.NewLine);
        }
    }
}