using System;
using System.Collections.Generic;

namespace TestOfferPopup.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool TryGetFirst<T>(this IEnumerable<T> self, out T value)
        {
            foreach (var item in self)
            {
                value = item;
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> self, Predicate<T> predicate, out T value)
        {
            foreach (var item in self)
            {
                if (!predicate(item))
                {
                    continue;
                }

                value = item;
                return true;
            }

            value = default;
            return false;
        }
    }
}