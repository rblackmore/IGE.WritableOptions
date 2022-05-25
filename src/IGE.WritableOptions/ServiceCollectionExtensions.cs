// <copyright file="ServiceCollectionExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions;

using System;
using System.IO;
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
      IConfigurationSection   configSection,
      string sectionName,
      string filePath = "appsettings.json",
      Func<JsonSerializerOptions> defaultSerializerOptions = null)
      where T : class, new()
  {
    Guard.Against.Null(services, nameof(services));
    Guard.Against.NullOrWhiteSpace(filePath, nameof(filePath));
    Guard.Against.NullOrWhiteSpace(sectionName, nameof(sectionName));

    services.Configure<T>(configSection);

    services.AddTransient<IWritableOptions<T>>(provider =>
    {
      string jsonFilePath;

      var environment = provider.GetService<IHostEnvironment>();

      if (environment != null)
      {
        var fileProvider = environment.ContentRootFileProvider;
        var fileInfo = fileProvider.GetFileInfo(filePath);
        jsonFilePath = fileInfo.PhysicalPath;
      }
      else
      {
        jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
      }

      var options = provider.GetService<IOptionsMonitor<T>>();

      return new WritableOptions<T>(jsonFilePath, sectionName, options, defaultSerializerOptions);
    });

    return services;
  }
}
