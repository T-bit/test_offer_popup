using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TestOfferPopup.Services
{
    /// <summary>
    /// Service responsible for operations with assets, e.g. loading, unloading, etc.
    /// </summary>
    public interface IAssetService : IService
    {
        IAssetCache AssetCache { get; }

        UniTask<Object> LoadAsync<T>(Reference reference, CancellationToken cancellationToken)
            where T : class;

        void Unload(Reference reference);
    }
}