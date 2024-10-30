using TestOfferPopup.Extensions;
using TestOfferPopup.Utilities;
using UnityEditor;
using UnityEngine;

namespace TestOfferPopup.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AddressableScriptableObject), true)]
    public class AddressableScriptableObjectEditor : Editor
    {
        private const string ReferencePath = "_reference";

        private void OnEnable()
        {
            var referenceProperty = serializedObject.FindProperty(ReferencePath);

            if (!(referenceProperty is { propertyType: SerializedPropertyType.ManagedReference }))
            {
                Debug.LogError($"Couldn't get {nameof(SerializedPropertyType.ManagedReference)} property at path {ReferencePath}.".AddContext(this));
                return;
            }

            // TODO: Debug and fix
            var asset = serializedObject.targetObject;

            if (asset == null || !EditorAssetUtility.TryGetAssetGuid(asset, out var assetGuid))
            {
                return;
            }

            referenceProperty.managedReferenceValue = new Reference(assetGuid);
        }
    }
}