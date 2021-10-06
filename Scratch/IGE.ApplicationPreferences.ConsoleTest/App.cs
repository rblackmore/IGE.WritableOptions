using Microsoft.Extensions.Hosting;

using Spectre.Console;

using System.Threading;
using System.Threading.Tasks;

namespace IGE.WritableOptions.ConsoleTest
{
    public class App : IHostedService
    {
        readonly IWritableOptions<MyConfig> options;

        public App(IWritableOptions<MyConfig> options)
        {
            this.options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = this.options.Value;

            AnsiConsole.WriteLine($"Name: {this.options.Value.Name}");
            AnsiConsole.WriteLine($"Score: {this.options.Value.Score}");

            options.Update(config =>
            {
                config.Name = AnsiConsole.Ask<string>("Enter Your Name:");
                config.Score = AnsiConsole.Ask<int>("Enter Score:");
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine("Goodbye, Now");
            return Task.CompletedTask;
        }
    }
}
