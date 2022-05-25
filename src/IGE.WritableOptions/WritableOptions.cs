// <copyright file="WritableOptions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions;

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

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
  private readonly JsonSerializerOptions jsonSerializerOptions;

  public WritableOptions(
      string filePath,
      string section,
      IOptionsMonitor<TOptions> options,
      Func<JsonSerializerOptions> defaultSerializerOptions = null)
  {
    this.jsonFilePath = filePath;
    this.sectionName = section;
    this.options = options;

    this.jsonSerializerOptions =
      (defaultSerializerOptions is null)
      ? JsonFileHelper.DefaultSerializerOptions()
      : defaultSerializerOptions();
  }

  /// <inheritdoc/>
  public TOptions Value => this.options.CurrentValue;

  /// <inheritdoc/>
  public TOptions Get(string name) => this.options.Get(name);

  /// <inheritdoc/>
  public void Update(Action<TOptions> applyChanges, JsonSerializerOptions serializerOptions = null)
  {
    JsonFileHelper.AddOrUpdateSection(this.jsonFilePath, this.sectionName, applyChanges, serializerOptions);
  }
}
