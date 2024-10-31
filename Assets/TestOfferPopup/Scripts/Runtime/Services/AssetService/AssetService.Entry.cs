using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TestOfferPopup.Services
{
    public partial class AssetService
    {
        private interface IEntry
        {
            UniTask StartAsync(Reference reference, CancellationToken cancellationToken);

            int CountReferences { get; set; }

            void Release();
        }

        private interface IEntry<T> : IEntry
            where T : class
        {
            UniTaskCompletionSource<T> CompletionSource { get; set; }
        }

        private class Entry<T> : IEntry<T>
            where T : class
        {
            public UniTaskCompletionSource<T> CompletionSource { get; set; }

            private AsyncOperationHandle<T> _operationHandle;

            public int CountReferences { get; set; }

            public Entry(UniTaskCompletionSource<T> completionSource)
            {
                CompletionSource = completionSource;
                CountReferences = 1;
            }

            #region IEntry

            async UniTask IEntry.StartAsync(Reference reference, CancellationToken cancellationToken)
            {
                _operationHandle = Addressables.LoadAssetAsync<T>(reference.AssetGuid);
                try
                {
                    var loadedObject = await _operationHandle.ToUniTask(cancellationToken: cancellationToken);
                    CompletionSource.TrySetResult(loadedObject);
                }
                catch (Exception exception)
                {
                    CompletionSource.TrySetException(exception);
                }
            }

            void IEntry.Release()
            {
                if (_operationHandle.IsValid())
                {
                    Addressables.Release(_operationHandle);
                }
            }

            #endregion
        }
    }
}