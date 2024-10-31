using TestOfferPopup.Utilities;
using UnityEditor;
using UnityEngine;

namespace TestOfferPopup.Drawers
{
    [CustomPropertyDrawer(typeof(IReference), true)]
    public sealed class ReferencePropertyDrawer : PropertyDrawer
    {
        private Object _asset;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // TODO: Filter types
            var assetGuidProperty = property.FindPropertyRelative("_assetGuid");

            LoadAsset(assetGuidProperty.stringValue);

            var selectedObject = EditorGUI.ObjectField(position, label, _asset, typeof(Object), false);

            if (selectedObject == null || !EditorAssetUtility.TryGetAssetGuid(selectedObject, out var assetGuid))
            {
                return;
            }

            _asset = selectedObject;
            assetGuidProperty.stringValue = assetGuid;
        }

        private void LoadAsset(string assetGuid)
        {
            if (string.IsNullOrWhiteSpace(assetGuid) || _asset != null)
            {
                return;
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

            _asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        }
    }
}