namespace IGE.WritableOptions.ConsoleTest;

using IGE.WritableOptions.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class Program
{
  public static void Main(string[] args)
  {
    CreateHostBuilder(args).Build().Run();
  }

  public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .UseWritableOptions<Settings>(nameof(Settings), "config/settings.json")
        .ConfigureServices((context, services) =>
        {
          services.AddHostedService<App>();
        });
}
