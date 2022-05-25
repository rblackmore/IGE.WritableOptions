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

            var sectionName = nameof(MyConfig);
            var configSection = context.Configuration.GetSection(sectionName);

            services.ConfigureWritableOptions<MyConfig>(configSection, sectionName, "resources/myconfig.json");
          });
}
