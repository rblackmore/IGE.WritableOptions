namespace IGE.WritableOptions;

using System.Text.Json;

using Ardalis.GuardClauses;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
  public static IHostBuilder UseWritableOptions<T>(
    this IHostBuilder hostBuilder,
    string sectionName,
    string fileName = "appsettings.json",
    Func<JsonSerializerOptions>? jsonSerializerOptionsFactory = null)
    where T : class, new()
  {
    Guard.Against.Null(hostBuilder, nameof(hostBuilder));

    hostBuilder
      .ConfigureAppConfiguration((context, configBuilder) =>
      {
        configBuilder.AddJsonFile(fileName, optional: true, reloadOnChange: true);
      })
      .ConfigureServices((context, services) =>
      {
        services.AddWritableOptions<T>(options =>
        {
          options.FileName = fileName;
          options.SectionName = sectionName;
          options.ConfigurationRoot = (IConfigurationRoot)context.Configuration;
          options.JsonSerializerOptionsFactory = jsonSerializerOptionsFactory;
        });
      });

    return hostBuilder;
  }
}
