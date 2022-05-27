// <copyright file="WritableOptions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions;

using System;
using System.Text.Json;

using IGE.WritableOptions.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

/// <inheritdoc/>
public class WritableOptions<TOptions> : IWritableOptions<TOptions>
    where TOptions : class, new()
{
  private readonly string jsonFilePath;
  private readonly string sectionName;
  private readonly IOptionsMonitor<TOptions> options;
  private readonly IConfigurationRoot configRoot;
  private readonly JsonSerializerOptions jsonSerializerOptions;

  public WritableOptions(
    string jsonFilePath,
    string sectionName,
    IOptionsMonitor<TOptions> optionsMonitor,
    IConfigurationRoot configRoot,
    Func<JsonSerializerOptions>? defaultSerializerOptionsDelegate = null)
  {
    this.jsonFilePath = jsonFilePath;
    this.sectionName = sectionName;
    this.options = optionsMonitor;
    this.configRoot = configRoot;

    this.jsonSerializerOptions =
      (defaultSerializerOptionsDelegate is null)
      ? JsonFileHelper.DefaultSerializerOptions.Invoke()
      : defaultSerializerOptionsDelegate.Invoke();
  }

  /// <inheritdoc/>
  public TOptions Value => this.options.CurrentValue;

  /// <inheritdoc/>
  public TOptions Get(string name) => this.options.Get(name);

  /// <inheritdoc/>
  public void Update(Action<TOptions> applyChanges, bool reload = true, JsonSerializerOptions? serializerOptions = null)
  {
    JsonFileHelper.AddOrUpdateSection(this.jsonFilePath, this.sectionName, applyChanges, serializerOptions ?? this.jsonSerializerOptions);

    if (reload)
      this.configRoot?.Reload();
  }
}
