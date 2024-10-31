using System.Collections.Generic;
using TestOfferPopup.Utilities;
using UnityEditor;
using UnityEngine;

namespace TestOfferPopup.Drawers
{
    [CustomPropertyDrawer(typeof(IReference), true)]
    public sealed class ReferencePropertyDrawer : PropertyDrawer
    {
        private readonly Dictionary<SerializedProperty, Object> _currentAssets = new Dictionary<SerializedProperty, Object>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // TODO: Filter types
            var assetGuidProperty = property.FindPropertyRelative("_assetGuid");

            LoadAsset(property, assetGuidProperty.stringValue);

            var selectedAsset = EditorGUI.ObjectField(position, label, GetCurrentAsset(property), typeof(Object), false);

            if (selectedAsset == null || !EditorAssetUtility.TryGetAssetGuid(selectedAsset, out var assetGuid))
            {
                return;
            }

            assetGuidProperty.stringValue = assetGuid;
            SetCurrentAsset(property, selectedAsset);
        }

        private Object GetCurrentAsset(SerializedProperty property)
        {
            return _currentAssets.TryGetValue(property, out var asset)
                ? asset
                : null;
        }

        private void SetCurrentAsset(SerializedProperty property, Object asset)
        {
            _currentAssets[property] = asset;
        }

        private void LoadAsset(SerializedProperty property, string assetGuid)
        {
            if (string.IsNullOrWhiteSpace(assetGuid) || GetCurrentAsset(property) != null)
            {
                return;
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

            SetCurrentAsset(property, AssetDatabase.LoadAssetAtPath<Object>(assetPath));
        }
    }
}