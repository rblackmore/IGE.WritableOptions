using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Text.Json;

namespace IGE.WritableOptions.ConsoleTest
{
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

                    var configurationSection = context.Configuration.GetSection(nameof(MyConfig));

                    services.ConfigureWritableOptions<MyConfig>(configurationSection, "appsettings.myconfig.json");
                });
    }
}