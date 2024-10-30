using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Services;
using UnityEngine;

namespace TestOfferPopup.Utilities
{
    public static class AssetUtility
    {
        private static IAssetService Service => GameUtility.GetService<IAssetService>();

        public static UniTask<Object> LoadAsync(Reference reference, CancellationToken cancellationToken)
        {
            return Service.LoadAsync(reference, cancellationToken);
        }

        public static void Unload(Reference reference)
        {
            Service.Unload(reference);
        }
    }
}