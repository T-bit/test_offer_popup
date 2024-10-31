using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using UnityEngine;

namespace TestOfferPopup
{
    public class AddressableLoaderBase<T> : MonoBehaviour
        where T : Object
    {
        private Reference<T> _reference;
        private bool _loaded;
        private CancellationTokenSource _cancellationTokenSource;

        protected bool IsReferenceLoaded(Reference<T> reference)
        {
            return _loaded && _reference.AssetGuid == reference.AssetGuid;
        }

        protected async UniTask<T> LoadAssetAsync(Reference<T> reference)
        {
            Clear();
            _cancellationTokenSource = new CancellationTokenSource();

            var loadedAsset = await reference.LoadAsync(_cancellationTokenSource.Token);

            _loaded = true;
            _reference = reference;
            return loadedAsset;
        }

        public void Clear()
        {
            _cancellationTokenSource?.CancelAndDispose();
            _cancellationTokenSource = null;

            // ReSharper disable once InvertIf
            if (_loaded)
            {
                _reference.Unload();
                _loaded = false;
            }
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}