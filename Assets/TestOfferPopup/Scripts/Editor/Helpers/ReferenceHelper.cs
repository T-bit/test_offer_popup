using TestOfferPopup.Extensions;
using UnityEngine;

namespace TestOfferPopup.Helpers
{
    public static class ReferenceHelper
    {
        private const string ReferencePath = "_reference";

        public static void SetReference<T>(Object unityObject, Reference reference)
            where T: Object, IAddressable
        {
            if (!typeof(T).TryGetFieldInfo(ReferencePath, out var referenceFieldInfo) ||
                !referenceFieldInfo.TryGetValue(unityObject, out Reference oldReference) ||
                oldReference == reference)
            {
                return;
            }

            referenceFieldInfo.TrySetValue(unityObject, reference);
            unityObject.ForceSaveAsset();
        }
    }
}