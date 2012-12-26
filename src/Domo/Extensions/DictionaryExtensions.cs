using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Domo.Extensions
{
    [DebuggerStepThrough]
    public static class DictionaryExtensions
    {
        public static void Add<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> items, TKey key, TValue value)
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

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key, Func<TKey, TValue> factoryDelegate)
        {
            return TryGetValue(items, key, () => factoryDelegate(key));
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