using System;
using System.Reflection;
using TestOfferPopup.Extensions;
using UnityEngine;

namespace TestOfferPopup.Utilities
{
    public static class ReflectionUtility
    {
        public static bool TryGetFieldInfo(Type type, string name, out FieldInfo fieldInfo, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic, bool silent = false)
        {
            fieldInfo = type.GetField(name, bindingFlags);

            if (!silent && fieldInfo == null)
            {
                Debug.LogError($"{nameof(ReflectionUtility).AddBold()}: Couldn't get {type.Name} {name} {nameof(FieldInfo)}.");
            }

            return fieldInfo != null;
        }

        public static bool TryGetNestedType(Type type, string name, out Type nestedType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic, bool silent = false)
        {
            nestedType = type.GetNestedType(name, bindingFlags);

            if (!silent && nestedType == null)
            {
                Debug.LogError($"{nameof(ReflectionUtility).AddBold()}: Couldn't get {type.Name} {name} nested type.");
            }

            return nestedType != null;
        }

        public static bool TryGetValue<T>(FieldInfo fieldInfo, object target, out T value, bool silent = false)
        {
            try
            {
                if (fieldInfo.GetValue(target) is T innerValue)
                {
                    value = innerValue;
                    return true;
                }
            }
            catch (Exception exception)
            {
                if (!silent)
                {
                    Debug.LogError($"{nameof(ReflectionUtility).AddBold()}: Couldn't get value of type {typeof(T).Name} from object {target}. Exception:\n{exception}");
                }
            }

            if (!silent)
            {
                Debug.LogError($"{nameof(ReflectionUtility).AddBold()}: Couldn't get value of type {typeof(T).Name} from object {target}.");
            }

            value = default;
            return false;
        }

        public static void TrySetValue(FieldInfo fieldInfo, object target, object value, bool silent = false)
        {
            try
            {
                fieldInfo.SetValue(target, value);
            }
            catch (Exception exception)
            {
                if (!silent)
                {
                    Debug.LogError($"{nameof(ReflectionUtility).AddBold()}: Couldn't set value {value} to object {target}. Exception:\n{exception}");
                }
            }
        }
    }
}