// Created/modified by Arkarin0 under one more more license(s).

using System.Collections.Generic;
using System.Linq;

internal static class EnumerableExtensions
{
    /// <summary>
    /// If <paramref name="source"/> is null, returns an empty enumerable of <typeparamref
    /// name="T"/>. Otherwise, returns <paramref name="source"/>.
    /// </summary>
    /// <remarks>
    /// Extension methods use "." calling syntax yet allow null parameters, so this method can be
    /// used to treat "null" and "empty list" the same with very little extra code.
    /// </remarks>
    public static IEnumerable<T> NullAsEmpty<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }
}
