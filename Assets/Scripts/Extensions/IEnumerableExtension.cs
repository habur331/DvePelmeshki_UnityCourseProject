using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class IEnumerableExtension
    {
        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }
    }
}