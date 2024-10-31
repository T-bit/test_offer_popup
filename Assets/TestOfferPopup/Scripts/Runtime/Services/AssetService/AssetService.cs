using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace TestOfferPopup.Services
{
    /// <summary>
    /// Basic implementation, without counters.
    /// </summary>
    public sealed class AssetService : Service, IAssetService
    {
        [NonSerialized]
        private readonly Dictionary<string, AsyncOperationHandle<Object>> _entries = new Dictionary<string, AsyncOperationHandle<Object>>();

        [NonSerialized]
        private IAssetCache _assetCache;

        protected override async UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            await Addressables.InitializeAsync();
            _assetCache = new AssetCache();
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _entries.Clear();
            _assetCache.Dispose();
            _assetCache = null;

            return UniTask.CompletedTask;
        }

        #region IAssetService

        IAssetCache IAssetService.AssetCache => _assetCache;

        UniTask<Object> IAssetService.LoadAsync(Reference reference, CancellationToken cancellationToken)
        {
            var key = reference.AssetGuid;

            if (!_entries.TryGetValue(key, out var value))
            {
                _entries[key] = value = Addressables.LoadAssetAsync<Object>(key);
            }

            return value.ToUniTask(cancellationToken: cancellationToken);
        }

        void IAssetService.Unload(Reference reference)
        {
            if (_entries.TryRemoveValue(reference.AssetGuid, out var handle))
            {
                Addressables.Release(handle);
            }
        }

        #endregion
    }
}