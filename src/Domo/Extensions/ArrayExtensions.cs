using System;

namespace Domo.Extensions
{
    public static class ArrayExtensions
    {
        public static TConverted[] Convert<TOriginal, TConverted>(this TOriginal[] items, Func<TOriginal, TConverted> converter)
        {
            var converted = new TConverted[items.Length];

            for (var i = 0; i < converted.Length; i++)
            {
                converted[i] = converter(items[i]);
            }

            return converted;
        }
    }
}