using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Domo.Extensions
{
    [DebuggerNonUserCode]
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
            return TryGetValue(items, key, default(TValue));
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key, TValue defaultValue)
        {
            TValue value;
            if (items.TryGetValue(key, out value))
                return value;

            return defaultValue;
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key, Func<TKey, TValue> factoryDelegate)
        {
            TValue value;

            if (!items.TryGetValue(key, out value))
            {
                lock (items)
                {
                    if (!items.TryGetValue(key, out value))
                    {
                        value = factoryDelegate(key);
                        items.Add(key, value);
                    }
                }
            }

            return value;
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

        public static object TryGetValue(this IDictionary dictionary, object key, Func<object> factoryDelegate)
        {
            if (dictionary.Contains(key))
                return dictionary[key];

            var value = factoryDelegate();

            dictionary.Add(key, value);

            return value;
        }
    }
}