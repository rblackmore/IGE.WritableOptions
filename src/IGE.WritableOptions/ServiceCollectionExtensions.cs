// <copyright file="ServiceCollectionExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions
{
    using System;
    using System.Text.Json;

    using Ardalis.GuardClauses;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtensions
    {
        private static string FileName { get; set; }

        private static IConfigurationSection Section { get; set; }

        private static JsonSerializerOptions JsonSerializerOptions { get; set; }

        public static IServiceCollection ConfigureWritableOptions<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json",
            JsonSerializerOptions jsonSerializerOptions = null)
            where T : class, new()
        {
            Guard.Against.Null(services, nameof(services));
            FileName = Guard.Against.NullOrWhiteSpace(file, nameof(file));
            Section = Guard.Against.Null(section, nameof(section));
            JsonSerializerOptions = jsonSerializerOptions;

            services.AddTransient<IWritableOptions<T>>(WritableOptionsFactory<T>);

            services.Configure<T>(nameof(T), section);

            return services;
        }

        private static WritableOptions<T> WritableOptionsFactory<T>(IServiceProvider provider)
            where T : class, new()
        {
            var environment = provider.GetService<IHostEnvironment>();
            var options = provider.GetService<IOptionsMonitor<T>>();
            return new WritableOptions<T>(environment, options, Section.Key, FileName, JsonSerializerOptions);
        }
    }
}