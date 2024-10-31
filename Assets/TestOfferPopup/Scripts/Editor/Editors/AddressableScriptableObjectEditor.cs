using TestOfferPopup.Helpers;
using TestOfferPopup.Utilities;
using UnityEditor;

namespace TestOfferPopup.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AddressableScriptableObject), true)]
    public class AddressableScriptableObjectEditor : Editor
    {
        private void OnEnable()
        {
            foreach (var targetObject in serializedObject.targetObjects)
            {
                if (EditorAssetUtility.TryGetAssetGuid(targetObject, out var assetGuid))
                {
                    ReferenceHelper.SetReference<AddressableScriptableObject>(targetObject, new Reference(assetGuid));
                }
            }
        }
    }
}