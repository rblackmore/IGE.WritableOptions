namespace IGE.WritableOptions.ConsoleTest;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Spectre.Console;

using System.Threading;
using System.Threading.Tasks;

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

    var printData = string.Format("Name: {0}\nScore: {1}", config.Name, config.Score.ToString());

    AnsiConsole.WriteLine(printData);


    //options.Update(config =>
    //{
    //  config.Name = AnsiConsole.Ask<string>("Enter Your Name:");
    //  config.Score = AnsiConsole.Ask<int>("Enter Score:");
    //});

    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    AnsiConsole.WriteLine("Goodbye, Now");
    return Task.CompletedTask;
  }
}
