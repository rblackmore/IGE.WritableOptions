namespace IGE.ApplicationPreferences
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using System;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    /// <summary>
    /// Writable Options, which can be injected into application components.
    /// Can make changes to an options POCO class, and write them back to JSON file (default appsettings.json).
    /// Should be registered as Transient.
    /// </summary>
    /// <typeparam name="T">The POCO Class to hold options, and to serialize.</typeparam>
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly IOptionsMonitor<T> options;
        private readonly string section;
        private readonly string filePath;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public WritableOptions(
            IHostEnvironment hostEnvironment,
            IOptionsMonitor<T> options,
            string section,
            string filePath,
            JsonSerializerOptions jsonSerializerOptions = null)
        {
            this.hostEnvironment = hostEnvironment;
            this.options = options;
            this.section = section;
            this.filePath = filePath;
            this.jsonSerializerOptions = jsonSerializerOptions ?? DefaultSerializerOptions;

            this.Initialize();
        }


        /// <inheritdoc/>
        public T Value => this.options.CurrentValue;

        /// <inheritdoc/>
        public T Get(string name) => this.options.Get(name);

        /// <summary>
        /// Applies changes to configuration setting and saves to file.
        /// </summary>
        /// <param name="applyChanges">Action which applies changes to the configuration object.</param>
        public void Update(Action<T> applyChanges)
        {
            var physicalPath = GetPhysicalFilePath();

            var rootNode = JsonNode.Parse(File.ReadAllText(physicalPath));

            var sectionObject = rootNode[this.section]?.Deserialize<T>() ?? new T();

            applyChanges(sectionObject);

            var sectionJson = JsonSerializer.Serialize(sectionObject);

            rootNode[this.section] = JsonNode.Parse(sectionJson);
            
            var fileStream = File.OpenWrite(physicalPath);

            var writer = new Utf8JsonWriter(fileStream, new() 
            { 
                Indented = true,
            });

            rootNode.WriteTo(writer, jsonSerializerOptions);

            writer.Flush();
        }

        private JsonSerializerOptions DefaultSerializerOptions => new()
            {
                WriteIndented = true,
            };

        private void Initialize()
        {
            var physicalPath = GetPhysicalFilePath();

            // Create new file, and initilize with empty root object.
            if (!File.Exists(physicalPath))
            {
                using var fileStream = File.OpenWrite(physicalPath);

                using var writer = new Utf8JsonWriter(fileStream, new()
                {
                    Indented = true,
                });

                writer.WriteStartObject();

                writer.WriteEndObject();

                writer.Flush();
            }
        }

        private string GetPhysicalFilePath()
        {
            var fileProvider = this.hostEnvironment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(this.filePath);
            var physicalPath = fileInfo.PhysicalPath;
            return physicalPath;
        }
    }
}
