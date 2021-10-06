// <copyright file="IWritableOptions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions
{
    using System;

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
        /// <param name="applyChanges">Action delegate used to apply changes to TOptions object.</param>
        void Update(Action<TOptions> applyChanges);
    }
}