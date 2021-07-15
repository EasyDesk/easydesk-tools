using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyDesk.Core
{
    public static class HashCodeUtils
    {
        public static int CombineHashCodes<T>(this IEnumerable<T> items, Func<T, int> hashCodeSelector = null)
        {
            return items
                .Select(hashCodeSelector ?? (x => x.GetHashCode()))
                .Aggregate(0, (h, c) => h ^ c);
        }

        public static int CombineHashCodes<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer) =>
            items.CombineHashCodes(comparer.GetHashCode);
    }
}
