// <copyright file="IWritableOptions.cs" company="Ryan Blackmore">.
// Copyright © 2021 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace IGE.WritableOptions
{
    using System;

    using Microsoft.Extensions.Options;

    public interface IWritableOptions<out T> : IOptionsSnapshot<T>
        where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
