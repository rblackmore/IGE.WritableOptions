namespace IGE.WritableOptions.ConsoleTest;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using IGE.WritableOptions;

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
        .UseWritableOptions<Settings>(
        nameof(Settings),
        "config/settings.json",
        () =>  new JsonSerializerOptions
        {
          WriteIndented = true,
          Converters = { new JsonStringEnumConverter() },
        })
        .ConfigureServices((context, services) =>
        {
          services.AddHostedService<App>();
        });
}
