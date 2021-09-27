using Microsoft.Extensions.Hosting;

using Spectre.Console;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace IGE.ApplicationPreferences.ConsoleTest
{
    public class TestApp : IHostedService
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var rootNode = JsonNode.Parse(File.ReadAllText("data.json"));

            AnsiConsole.WriteLine($"{rootNode.ToJsonString(jsonSerializerOptions)}");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine($"Goodby from {nameof(TestApp)}");
            return Task.CompletedTask;
        }
    }
}
