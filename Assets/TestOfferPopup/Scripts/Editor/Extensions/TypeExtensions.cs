using System;
using System.Reflection;
using TestOfferPopup.Utilities;

namespace TestOfferPopup.Extensions
{
    public static class TypeExtensions
    {
        public static bool TryGetFieldInfo(this Type self, string name, out FieldInfo fieldInfo, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic, bool silent = false)
        {
            return ReflectionUtility.TryGetFieldInfo(self, name, out fieldInfo, bindingFlags, silent);
        }

        public static bool TryGetNestedType(this Type self, string name, out Type nestedType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic, bool silent = false)
        {
            return ReflectionUtility.TryGetNestedType(self, name, out nestedType, bindingFlags, silent);
        }
    }
}