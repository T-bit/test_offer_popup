using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestOfferPopup.Extensions
{
    public static class AddressableExtensions
    {
        public static T Clone<T>(this T self, Transform parent = default)
            where T : class, IAddressable
        {
            if (!(self is Object value))
            {
                throw new InvalidOperationException();
            }

            return value.Instantiate(parent)
                        .WithName(value.name)
                        .Cast<T>();
        }

        public static void Destroy(this IAddressable self, bool unload = true)
        {
            var reference = self.BaseReference;

            switch (self)
            {
                case Component component:
                    component.gameObject.Destroy();
                    break;
                case Object unityObject:
                    unityObject.Destroy();
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (unload)
            {
                reference.Unload();
            }
        }
    }
}