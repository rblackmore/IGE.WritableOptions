// <copyright file="IWritableOptions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions;

using System;
using System.Text.Json;

using Microsoft.Extensions.Options;

/// <summary>
/// Interface Contract.
/// Used to apply changes to TOptions object and serialize to json file.
/// </summary>
/// <typeparam name="TOptions">The options type to serialize.</typeparam>
public interface IWritableOptions<out TOptions> : IOptionsSnapshot<TOptions>
    where TOptions : class, new()
{

  /// <summary>
  /// Applies configuration changes to TOptions object.
  /// Then serializes to Json file.
  /// </summary>
  /// <param name="applyChanges">Delegate for apply changes to Options Object.</param>
  /// <param name="reload">Sets weather to reload configuration after applying new Settings. Default <see langword="true"/>.</param>
  /// <param name="serializerOptions">Json Serilizer Options.</param>
  public void Update(Action<TOptions> applyChanges, bool reload = true, JsonSerializerOptions? serializerOptions = null);
}
