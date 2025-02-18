﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestOfferPopup.Extensions;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace TestOfferPopup.Services
{
    public class AssetCache : IAssetCache
    {
        private readonly Dictionary<string, string> _addressToGuidEntries;
        private readonly Dictionary<string, string> _guidToNameEntries;

        public AssetCache()
        {
            _addressToGuidEntries = new Dictionary<string, string>(4096);
            _guidToNameEntries = new Dictionary<string, string>(4096);

            Cache();
        }

        private void Cache()
        {
            foreach (var resourceLocator in Addressables.ResourceLocators)
            {
                foreach (var key in resourceLocator.Keys)
                {
                    if (!(key is string guid) || !guid.IsGuid())
                    {
                        continue;
                    }

                    if (!resourceLocator.Locate(key, null, out var locations))
                    {
                        continue;
                    }

                    var location = locations[0];

                    var name = location.PrimaryKey.IsGuid()
                        ? Path.GetFileNameWithoutExtension(location.InternalId)
                        : location.PrimaryKey;

                    _addressToGuidEntries[name] = guid;
                    _guidToNameEntries[guid] = name;
                }
            }
        }

        #region IAssetCache

        IEnumerable<string> IAssetCache.AssetAddresses => _addressToGuidEntries.Keys;

        bool IAssetCache.TryGetAssetGuid(string assetAddress, out string assetGuid)
        {
            return _addressToGuidEntries.TryGetValue(assetAddress, out assetGuid);
        }

        bool IAssetCache.TryGetAssetName(string assetGuid, out string assetName)
        {
            return _guidToNameEntries.TryGetValue(assetGuid, out assetName);
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            _addressToGuidEntries.Clear();
            _guidToNameEntries.Clear();
        }

        #endregion
    }
}