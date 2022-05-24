using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Spectre.Console;

using System.Threading;
using System.Threading.Tasks;

namespace IGE.WritableOptions.ConsoleTest
{
    public class App : IHostedService
    {
        readonly IOptions<MyConfig> options;

        public App(IOptions<MyConfig> options)
        {
            this.options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = this.options.Value;

            AnsiConsole.WriteLine($"Name: {config.Name}");
            AnsiConsole.WriteLine($"Score: {config.Score}");

            //options.Update(config =>
            //{
            //    config.Name = AnsiConsole.Ask<string>("Enter Your Name:");
            //    config.Score = AnsiConsole.Ask<int>("Enter Score:");
            //});

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine("Goodbye, Now");
            return Task.CompletedTask;
        }
    }
}
