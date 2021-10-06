// <copyright file="WritableOptions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    /// <inheritdoc/>
    public class WritableOptions<TOptions> : IWritableOptions<TOptions>
        where TOptions : class, new()
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly IOptionsMonitor<TOptions> options;
        private readonly string section;
        private readonly string filePath;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public WritableOptions(
            IHostEnvironment hostEnvironment,
            IOptionsMonitor<TOptions> options,
            string section,
            string filePath,
            JsonSerializerOptions jsonSerializerOptions = null)
        {
            this.hostEnvironment = hostEnvironment;
            this.options = options;
            this.section = section;
            this.filePath = filePath;
            this.jsonSerializerOptions =
                jsonSerializerOptions ?? DefaultSerializerOptions;

            this.Initialize();
        }

        /// <inheritdoc/>
        public TOptions Value => this.options.CurrentValue;

        private static JsonSerializerOptions DefaultSerializerOptions => new()
        {
            WriteIndented = true,
        };

        private string PhysicalFilePath
        {
            get
            {
                var fileProvider = this.hostEnvironment.ContentRootFileProvider;
                var fileInfo = fileProvider.GetFileInfo(this.filePath);
                var physicalPath = fileInfo.PhysicalPath;
                return physicalPath;
            }
        }

        /// <inheritdoc/>
        public TOptions Get(string name) => this.options.Get(name);

        /// <inheritdoc/>
        public void Update(Action<TOptions> applyChanges)
        {
            var physicalPath = this.PhysicalFilePath;

            var rootNode = JsonNode.Parse(File.ReadAllText(physicalPath));

            var sectionObject = rootNode[this.section]?.Deserialize<TOptions>() ?? new TOptions();

            applyChanges(sectionObject);

            var sectionJson = JsonSerializer.Serialize(sectionObject);

            rootNode[this.section] = JsonNode.Parse(sectionJson);

            var fileStream = File.OpenWrite(physicalPath);

            var writer = new Utf8JsonWriter(fileStream, new()
            {
                Indented = true,
            });

            rootNode.WriteTo(writer, this.jsonSerializerOptions);

            writer.Flush();
        }

        private void Initialize()
        {
            var physicalPath = this.PhysicalFilePath;

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
    }
}
