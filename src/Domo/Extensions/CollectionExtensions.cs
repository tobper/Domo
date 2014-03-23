using System.Collections.Generic;

namespace Domo.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}