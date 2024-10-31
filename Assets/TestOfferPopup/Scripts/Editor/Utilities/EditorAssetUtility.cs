using UnityEditor;
using UnityEngine;

namespace TestOfferPopup.Utilities
{
    public static class EditorAssetUtility
    {
        public static bool TryGetAssetGuid(Object asset, out string assetGuid)
        {
            return AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out assetGuid, out long _);
        }
    }
}