using System.Collections.Generic;
using TestOfferPopup.Extensions;
using TestOfferPopup.Scopes;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace TestOfferPopup.Utilities
{
    [InitializeOnLoad]
    public static class EditorAddressablesUtility
    {
        private static bool AssetSettingsExists => AddressableAssetSettingsDefaultObject.SettingsExists;

        private static AddressableAssetSettings AssetSettings => AddressableAssetSettingsDefaultObject.Settings;

        private static AddressableAssetGroup DefaultGroup => AssetSettings.DefaultGroup;

        static EditorAddressablesUtility()
        {
            Editor.finishedDefaultHeaderGUI += OnFinishedDefaultHeaderGUI;
        }

        private static void OnFinishedDefaultHeaderGUI(Editor editor)
        {
            using (ListScope<(IAddressable Addressable, AddressableAssetEntry AssetEntry)>.Create(out var entries))
            {
                SelectEntries(editor.targets, entries);

                if (entries.Count == 0)
                {
                    return;
                }

                if (!GUILayout.Button("Update Address"))
                {
                    return;
                }

                foreach (var (addressable, assetEntry) in entries)
                {
                    assetEntry.address = $"{addressable.GetType().Name}/{addressable.Name}";
                }
            }
        }

        private static void SelectEntries(IEnumerable<Object> targets, ICollection<(IAddressable, AddressableAssetEntry)> entries)
        {
            if (!AssetSettingsExists)
            {
                return;
            }

            foreach (var target in targets)
            {
                if (!target.IsPersistent() || !target.TryGetAddressable(out var addressable))
                {
                    continue;
                }

                var assetGuid = target.GetAssetGuid();

                if (TryGetAssetEntry(assetGuid, out var assetEntry))
                {
                    entries.Add((addressable, assetEntry));
                }
            }
        }

        public static void CreateOrMoveEntry(string assetGuid)
        {
            AssetSettings.CreateOrMoveEntry(assetGuid, DefaultGroup);
        }

        public static bool TryGetAssetEntry(string assetGuid, out AddressableAssetEntry assetEntry)
        {
            if (!AssetSettingsExists || !assetGuid.IsGuid())
            {
                assetEntry = default;
                return false;
            }

            assetEntry = AssetSettings.FindAssetEntry(assetGuid);

            return assetEntry != null;
        }

        public static bool AssetEntryExists(string assetGuid)
        {
            return TryGetAssetEntry(assetGuid, out _);
        }
    }
}