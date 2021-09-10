using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IGE.ApplicationPreferences
{
    public class Preferences<T> where T : class
    {
        private readonly string filepath;

        private T value;

        public T Value => this.value;

        public Preferences(string filePath, T preferences)
        {
            this.filepath = filePath;
            this.value = preferences;
        }

        private static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true,
        };

        public static Preferences<T> Create(string fileName)
        {
            var path = GetLocalFilePath(fileName);

            if (File.Exists(path))
            {
                var prefs = JsonSerializer.Deserialize<T>(File.ReadAllText(path), JsonOptions);
                return new Preferences<T>(path, prefs);
            }

            var newPrefs = Activator.CreateInstance<T>();
            var preferences = new Preferences<T>(path, newPrefs);

            preferences.SaveSettings();

            return preferences;
        }

        private static string GetLocalFilePath(string fileName)
        {
            string current = Directory.GetCurrentDirectory();

            string filepath = Path.Combine(current + "\\", $"{fileName}.json");

            return filepath;
        }

        public void SaveSettings()
        {
            string json = JsonSerializer.Serialize(this.value, JsonOptions);

            File.WriteAllText(this.filepath, json);
        }
    }
}
