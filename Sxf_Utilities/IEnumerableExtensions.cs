using System;
using System.Collections.Generic;

namespace Sxf_Utilities
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerables, Action<T> action)
        {
            foreach (var enumerable in enumerables)
            {
                action(enumerable);
            }
        }
    }
}