using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestOfferPopup.Utilities
{
    public static class AssetUtility
    {
        private static IAssetService Service => GameUtility.GetService<IAssetService>();

        private static IAssetCache AssetCache => Service.AssetCache;

        public static IEnumerable<string> AssetAddresses => AssetCache.AssetAddresses;

        public static bool TryGetAssetGuid(string assetAddress, out string assetGuid)
        {
            return AssetCache.TryGetAssetGuid(assetAddress, out assetGuid);
        }

        public static bool TryGetAssetName(string assetGuid, out string assetName)
        {
            return AssetCache.TryGetAssetName(assetGuid, out assetName);
        }

        public static bool TryGetReferenceByAddress<T>(string assetAddress, out Reference<T> reference)
            where T : class
        {
            if (TryGetAssetGuid(assetAddress, out var assetGuid) &&
                TryGetReferenceByGuid(assetGuid, out reference))
            {
                return true;
            }

            reference = default;
            return false;
        }

        public static Reference<T> GetReferenceByAddress<T>(string assetAddress)
            where T : class
        {
            if (!TryGetReferenceByAddress<T>(assetAddress, out var reference))
            {
                throw new InvalidKeyException(assetAddress, typeof(T));
            }

            return reference;
        }

        public static UniTask<Object> LoadAsync(Reference reference, CancellationToken cancellationToken)
        {
            return Service.LoadAsync(reference, cancellationToken);
        }

        public static void Unload(Reference reference)
        {
            Service.Unload(reference);
        }

        private static bool TryGetReferenceByGuid<T>(string assetGuid, out Reference<T> reference)
            where T : class
        {
            if (IsValidAssetGuid(assetGuid))
            {
                reference = new Reference<T>(assetGuid);
                return true;
            }

            reference = default;
            return false;
        }

        private static bool IsValidAssetGuid(string assetGuid)
        {
            return assetGuid.IsGuid() &&
                   Addressables.ResourceLocators.Any(resourceLocator => resourceLocator.Locate(assetGuid, null, out _));
        }
    }
}