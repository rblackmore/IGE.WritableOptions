using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System.Text.Json;

namespace IGE.WritableOptions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureWritableOptions<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json",
            JsonSerializerOptions jsonSerializerOptions = null) where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var environment = provider.GetService<IHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(environment, options, section.Key, file, jsonSerializerOptions);
            });

            return services;
        }
    }
}
