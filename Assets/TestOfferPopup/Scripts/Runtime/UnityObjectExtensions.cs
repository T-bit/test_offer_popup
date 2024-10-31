using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestOfferPopup
{
    public static class UnityObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this Object self)
        {
            if (Application.IsPlaying(self))
            {
                Object.Destroy(self);
            }
            else
            {
                Object.DestroyImmediate(self);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Instantiate<T>(this T self, Transform parent = default, bool worldPositionStays = false)
            where T : Object
        {
            return Object.Instantiate(self, parent, worldPositionStays);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WithName<T>(this T self, string name)
            where T: Object
        {
            self.name = name;

            return self;
        }

        public static T Cast<T>(this Object self)
        {
            if (!(self is T value))
            {
                throw new InvalidCastException();
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DontDestroyOnLoad(this Object self)
        {
            Object.DontDestroyOnLoad(self);
        }
    }
}