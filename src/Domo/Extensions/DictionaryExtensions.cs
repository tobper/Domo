using System;
using System.Collections.Generic;

namespace Domo.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> items, TKey key, TValue value)
        {
            var list = TryGetValue(items, key, () => new List<TValue>());

            lock (list)
            {
                list.Add(value);
            }
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            TValue value;
            if (items.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key, Func<TValue> factoryDelegate)
        {
            TValue value;

            if (!items.TryGetValue(key, out value))
            {
                lock (items)
                {
                    if (!items.TryGetValue(key, out value))
                    {
                        value = factoryDelegate();
                        items.Add(key, value);
                    }
                }
            }

            return value;
        }
    }
}