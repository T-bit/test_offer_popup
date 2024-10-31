using TestOfferPopup.Helpers;
using TestOfferPopup.Utilities;
using UnityEditor;

namespace TestOfferPopup.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AddressableMonoBehaviour), true)]
    public class AddressableMonoBehaviourEditor : Editor
    {
        private void OnEnable()
        {
            foreach (var targetObject in serializedObject.targetObjects)
            {
                if (EditorAssetUtility.TryGetAssetGuid(targetObject, out var assetGuid))
                {
                    ReferenceHelper.SetReference<AddressableMonoBehaviour>(targetObject, new Reference(assetGuid));
                }
            }
        }
    }
}