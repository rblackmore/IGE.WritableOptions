using ConsoleScratch.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Spectre.Console;

CreateHostBuilder(args).Build().Run();


static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<App>();

        var configurationRoot = context.Configuration;

        services.Configure<GameConfigurations>(GameConfigurations.Guest,
            context
            .Configuration
            .GetSection($"{nameof(GameConfigurations)}:{nameof(GameConfigurations.Guest)}"));

        services.Configure<GameConfigurations>(GameConfigurations.Personalize,
            context
            .Configuration
            .GetSection($"{nameof(GameConfigurations)}:{nameof(GameConfigurations.Personalize)}"));
    });

internal class App : IHostedService
{
    private IOptionsMonitor<GameConfigurations> gameOptions;

    public App(IOptionsMonitor<GameConfigurations> gameConfiguration)
    {
        this.gameOptions = gameConfiguration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var input = String.Empty;

        do
        {
            input = AnsiConsole.Prompt<string>(
                new SelectionPrompt<string>()
                .Title("Select Account")
                .PageSize(5)
                .AddChoices(new[]
                {
                    GameConfigurations.Guest,
                    GameConfigurations.Personalize,
                    "exit"
                })
                );

            GameConfigurations gameConfig = null;

            if (input == GameConfigurations.Guest)
                gameConfig = this.gameOptions.Get(GameConfigurations.Guest);
            
            if (input == GameConfigurations.Personalize)
                gameConfig = this.gameOptions.Get(GameConfigurations.Personalize);

            AnsiConsole.MarkupLine($"[green]Hello, {gameConfig.PlayerName}!!![/]");
            AnsiConsole.MarkupLine($"[green]Player Count:[/] [blue]{gameConfig.PlayerCount}[/]");
            AnsiConsole.MarkupLine($"[green]Controller Support:[/] [blue]{gameConfig.ControllerSupport}[/]");

            AnsiConsole.Write($"Press Enter to Continue, type 'exit' to close: ");
            input = Console.ReadLine().Trim();

        } while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase));

        AnsiConsole.WriteLine("Hello, World Again");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine($"[red3_1]Closing Down...bye[/]");
        return Task.CompletedTask;
    }
}