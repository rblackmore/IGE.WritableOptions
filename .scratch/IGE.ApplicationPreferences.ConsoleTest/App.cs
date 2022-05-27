namespace IGE.WritableOptions.ConsoleTest;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Spectre.Console;

using System.Threading;
using System.Threading.Tasks;

public class App : IHostedService
{
  readonly IOptions<Settings> options;

  public App(IOptions<Settings> options)
  {
    this.options = options;
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    var config = this.options.Value;

    var printData = string.Format("Text: {0}\nName: {1}\nScore: {2}",config.Text, config.Name, config.Score.ToString());

    AnsiConsole.WriteLine(printData);

    //options.Update(settings =>
    //{
    //  settings.Text = AnsiConsole.Ask<string>("Some Text: ");
    //  settings.Name = AnsiConsole.Ask<string>("Name: ");
    //  settings.Score = AnsiConsole.Ask<int>("Score: ");
    //});

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
