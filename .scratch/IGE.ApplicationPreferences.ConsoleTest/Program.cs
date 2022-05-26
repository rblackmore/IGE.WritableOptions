namespace IGE.WritableOptions.ConsoleTest;

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
          .ConfigureServices((context, services) =>
          {
            services.AddHostedService<App>();

            var sectionName = nameof(Settings);
            var configSection = context.Configuration.GetSection(sectionName);

            services.ConfigureWritableOptions<Settings>(configSection, sectionName, "resources/settings.json");
          });
}
