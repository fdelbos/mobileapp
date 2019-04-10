﻿using System;

namespace Toggl.Multivac.Extensions
{
    public static class LazyExtensions
    {
        public static Lazy<TResult> Select<T, TResult>(this Lazy<T> lazy, Func<T, TResult> transform)
            => new Lazy<TResult>(() => transform(lazy.Value));
    }
}
