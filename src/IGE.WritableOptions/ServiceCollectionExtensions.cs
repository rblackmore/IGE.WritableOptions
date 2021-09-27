// <copyright file="ServiceCollectionExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

#pragma warning disable HAA0302 // Display class allocation to capture closure
#pragma warning disable HAA0301 // Closure Allocation Source
#pragma warning disable HAA0303 // Lambda or anonymous method in a generic method allocates a delegate instance
namespace IGE.WritableOptions
{
    using System.Text.Json;

    using Ardalis.GuardClauses;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureWritableOptions<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json",
            JsonSerializerOptions jsonSerializerOptions = null)
            where T : class, new()
        {
            Guard.Against.NullOrWhiteSpace(file, nameof(file));

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
#pragma warning restore HAA0303 // Lambda or anonymous method in a generic method allocates a delegate instance
#pragma warning restore HAA0301 // Closure Allocation Source
#pragma warning restore HAA0302 // Display class allocation to capture closure