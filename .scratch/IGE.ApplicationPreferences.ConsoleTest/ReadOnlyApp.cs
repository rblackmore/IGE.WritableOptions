namespace IGE.WritableOptions.ConsoleTest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using IGE.WritableOptions.ConsoleTest;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using Spectre.Console;

    internal class ReadOnlyApp : IHostedService
    {
        private readonly IWritableOptions<MyConfig> options;

        public ReadOnlyApp(IWritableOptions<MyConfig> options)
        {
            this.options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine($"Name: {this.options.Value.Name}");
            AnsiConsole.WriteLine($"Score: {this.options.Value.Score}");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
