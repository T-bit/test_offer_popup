using System.Reflection;
using TestOfferPopup.Utilities;

namespace TestOfferPopup.Extensions
{
    public static class FieldInfoExtensions
    {
        public static bool TryGetValue<T>(this FieldInfo self, object target, out T value, bool silent = false)
        {
            return ReflectionUtility.TryGetValue(self, target, out value, silent);
        }

        public static void TrySetValue(this FieldInfo self, object target, object value, bool silent = false)
        {
            ReflectionUtility.TrySetValue(self, target, value, silent);
        }
    }
}