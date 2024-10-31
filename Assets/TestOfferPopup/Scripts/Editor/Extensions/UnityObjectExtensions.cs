using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestOfferPopup.Extensions
{
    public static class UnityObjectExtensions
    {
public static string GetAssetGuid(this Object self)
        {
            Assert.IsNotNull(self);

            var assetPath = self.GetAssetPath();
            var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);

            Assert.IsFalse(string.IsNullOrWhiteSpace(assetGuid));

            return assetGuid;
        }

        public static string GetAssetPath(this Object self)
        {
            Assert.IsTrue(self.TryGetAssetPath(out var assetPath));

            return assetPath;
        }

        public static bool TryGetAssetPath(this Object self, out string assetPath)
        {
            if (self == null)
            {
                assetPath = default;
                return false;
            }

            assetPath = AssetDatabase.GetAssetPath(self);

            return !string.IsNullOrWhiteSpace(assetPath);
        }

        public static bool IsPersistent(this Object self)
        {
            return EditorUtility.IsPersistent(self);
        }

        public static bool IsMainAsset(this Object self)
        {
            return AssetDatabase.IsMainAsset(self);
        }

        public static bool IsPreviewSceneObject(this Object self)
        {
            return EditorSceneManager.IsPreviewSceneObject(self);
        }

        public static void SetDirty(this Object self, bool dirty)
        {
            Assert.IsNotNull(self);

            if (dirty)
            {
                EditorUtility.SetDirty(self);
            }
            else
            {
                EditorUtility.ClearDirty(self);
            }
        }

        public static void ForceSaveAsset(this Object self)
        {
            Assert.IsNotNull(self);
            EditorUtility.SetDirty(self);
            AssetDatabase.SaveAssetIfDirty(self);
        }

        public static void ForceReserializeAsset(this Object self)
        {
            var assetPath = self.GetAssetPath();
            AssetDatabase.ForceReserializeAssets(GetAssetPaths());

            IEnumerable<string> GetAssetPaths()
            {
                yield return assetPath;
            }
        }

        public static bool TryGetAddressable(this Object target, out IAddressable addressable)
        {
            if (target is IAddressable addressableTarget)
            {
                addressable = addressableTarget;
                return true;
            }

            if (!target.IsMainAsset() && target.TryGetAssetPath(out var assetPath))
            {
                target = AssetDatabase.LoadMainAssetAtPath(assetPath);
            }

            if (target is GameObject gameObject && gameObject.TryGetComponent(out addressable))
            {
                return true;
            }

            addressable = default;
            return false;
        }
    }
}