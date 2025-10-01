// Created/modified by Arkarin0 under one more more license(s).

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arkarin0.DotNet.Arcade.Sdk.Tests
{
    public static class CollectionExtensions
    {
        public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
