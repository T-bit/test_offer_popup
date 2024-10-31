using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;


namespace TestOfferPopup.Services
{
    /// <summary>
    /// Implementation with counter and prioritization support.
    /// </summary>
    public sealed partial class AssetService : Service, IAssetService
    {
        [NonSerialized]
        private readonly Dictionary<string, IEntry> _entries = new Dictionary<string, IEntry>();

        [NonSerialized]
        private readonly List<PriorityItem> _loadingQueue = new List<PriorityItem>();

        [NonSerialized]
        private bool _processLoadingQueue;

        [NonSerialized]
        private int _countLoadingObjects;

        [NonSerialized]
        private IAssetCache _assetCache;

        [NonSerialized]
        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField]
        private int _loadingObjectsLimit = 25;

        protected override async UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            await Addressables.InitializeAsync();
            _assetCache = new AssetCache();
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _entries.Clear();
            _loadingQueue.Clear();
            _assetCache.Dispose();
            _assetCache = null;

            return UniTask.CompletedTask;
        }

        private void DecreaseReferenceLink(Reference reference)
        {
            if (!_entries.TryGetValue(reference.AssetGuid, out var value))
            {
                Debug.Log($"Couldn't find entry for reference {reference.AssetGuid}".AddContext<AssetService>());
                return;
            }

            value.CountReferences--;
            if (value.CountReferences > 0)
            {
                return;
            }

            _entries.Remove(reference.AssetGuid);

            for (var i = 0; i < _loadingQueue.Count; i++)
            {
                if (_loadingQueue[i].Reference.AssetGuid != reference.AssetGuid)
                {
                    continue;
                }

                _loadingQueue.RemoveAt(i);
                break;
            }

            value.Release();
        }

        private async UniTask ProcessLoadingQueueAsync(CancellationToken cancellationToken)
        {
            _processLoadingQueue = true;
            _countLoadingObjects = 0;

            try
            {
                await UniTask.NextFrame(cancellationToken);

                while (_loadingQueue.Count > 0 || _countLoadingObjects > 0)
                {
                    if (_countLoadingObjects >= _loadingObjectsLimit)
                    {
                        await UniTask.NextFrame(cancellationToken);
                        continue;
                    }

                    if (_loadingQueue.Count == 0)
                    {
                        await UniTask.NextFrame(cancellationToken);
                        continue;
                    }

                    RunLoadingHighPriorityItem();
                }
            }
            finally
            {
                _cancellationTokenSource?.CancelAndDispose();
                _cancellationTokenSource = null;

                _processLoadingQueue = false;
            }
        }

        private async UniTask InternalLoadAsync(Reference reference)
        {
            var entry = _entries[reference.AssetGuid];

            entry.CountReferences++;
            _countLoadingObjects++;

            try
            {
                await entry.StartAsync(reference, GameUtility.CancellationToken);
            }
            finally
            {
                DecreaseReferenceLink(reference);
                _countLoadingObjects--;
            }
        }

        private void RunLoadingHighPriorityItem()
        {
            var highPriorityItem = _loadingQueue[0];

            for (var i = 1; i < _loadingQueue.Count; i++)
            {
                if (_loadingQueue[i].Priority > highPriorityItem.Priority)
                {
                    highPriorityItem = _loadingQueue[i];
                }
            }

            _loadingQueue.Remove(highPriorityItem);
            InternalLoadAsync(highPriorityItem.Reference).Forget();
        }

        #region IAssetService

        IAssetCache IAssetService.AssetCache => _assetCache;

        async UniTask<Object> IAssetService.LoadAsync<T>(Reference reference, CancellationToken cancellationToken)
        {
            if (_entries.TryGetValue(reference.AssetGuid, out var value))
            {
                value.CountReferences++;

                try
                {
                    if (value is IEntry<T> derivedEntry)
                    {
                        var result = await derivedEntry.CompletionSource.Task
                            .AttachExternalCancellation(cancellationToken: cancellationToken);
                        return result as Object;
                    }

                    var defaultEntry = (IEntry<Object>)value;
                    return await defaultEntry.CompletionSource.Task
                        .AttachExternalCancellation(cancellationToken: cancellationToken);
                }
                catch
                {
                    DecreaseReferenceLink(reference);
                    throw;
                }
            }

            IEntry entry;

            if (typeof(Object).IsAssignableFrom(typeof(T)))
            {
                entry = new Entry<T>(new UniTaskCompletionSource<T>());
            }
            else
            {
                entry = new Entry<Object>(new UniTaskCompletionSource<Object>());
            }

            _entries[reference.AssetGuid] = entry;
            _loadingQueue.Add(new PriorityItem(reference, 0));

            if (!_processLoadingQueue)
            {
                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);
                ProcessLoadingQueueAsync(_cancellationTokenSource.Token).Forget();
            }

            try
            {
                if (entry is IEntry<T> derivedEntry)
                {
                    var result = await derivedEntry.CompletionSource.Task
                        .AttachExternalCancellation(cancellationToken: cancellationToken);
                    return result as Object;
                }

                var defaultEntry = (IEntry<Object>)entry;
                return await defaultEntry.CompletionSource.Task
                    .AttachExternalCancellation(cancellationToken: cancellationToken);
            }
            catch
            {
                DecreaseReferenceLink(reference);
                throw;
            }
        }

        void IAssetService.Unload(Reference reference)
        {
            DecreaseReferenceLink(reference);
        }

        #endregion
    }
}