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
        UniTask<Object> LoadAsync(Reference reference, CancellationToken cancellationToken);

        void Unload(Reference reference);
    }
}