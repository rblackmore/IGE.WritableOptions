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
  public class WritableOptionsSettings
  {
    public string FileName { get; set; } = "appsettings.json";

    public string? SectionName { get; set; }

    public Func<JsonSerializerOptions>? JsonSerializerOptionsFactory { get; set; }

    public IConfigurationRoot? ConfigurationRoot { get; set; }

  }

  public static IServiceCollection AddWritableOptions<T>(
    this IServiceCollection services,
    Action<WritableOptionsSettings>? configure = null)
    where T : class, new()
  {
    var settings = new WritableOptionsSettings();
    configure?.Invoke(settings);

    var fileName = settings.FileName;
    var configRoot = settings.ConfigurationRoot;
    var sectionName = settings.SectionName;
    var configSection = configRoot.GetSection(sectionName);
    var jsonSerializerOptionsFactory = settings.JsonSerializerOptionsFactory;

    services.Configure<T>(configSection);

    services.AddTransient<IWritableOptions<T>>(provider =>
    {
      string jsonFilePath;

      var environment = provider.GetService<IHostEnvironment>();

      if (environment is not null)
      {
        var fileProvider = environment.ContentRootFileProvider;
        var fileInfo = fileProvider.GetFileInfo(fileName);
        jsonFilePath = fileInfo.PhysicalPath;
      }
      else
      {
        jsonFilePath = Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory,
          fileName);
      }

      var options = provider.GetService<IOptionsMonitor<T>>();

      return new WritableOptions<T>(
        jsonFilePath,
        sectionName,
        options,
        configRoot,
        jsonSerializerOptionsFactory);
    });

    return services;
  }

  public static IServiceCollection AddWritableOptions<T>(
      this IServiceCollection services,
      IConfigurationRoot configRoot,
      string sectionName,
      string fileName = "appsettings.json",
      Func<JsonSerializerOptions>? jsonSerializerOptionsFactory = null)
      where T : class, new()
  {
    Guard.Against.Null(services, nameof(services));
    Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
    Guard.Against.NullOrWhiteSpace(sectionName, nameof(sectionName));

    return services.AddWritableOptions<T>(options =>
    {
      options.FileName = fileName;
      options.ConfigurationRoot = configRoot;
      options.SectionName = sectionName;
      options.JsonSerializerOptionsFactory = jsonSerializerOptionsFactory;
    });
  }
}
