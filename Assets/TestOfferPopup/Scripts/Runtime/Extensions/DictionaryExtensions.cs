using System.Collections.Generic;

namespace TestOfferPopup.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryRemoveValue<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, out TValue value)
        {
            if (!self.TryGetValue(key, out value))
            {
                return false;
            }

            self.Remove(key);

            return true;
        }
    }
}