using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestOfferPopup.Extensions
{
    public static class ReferenceExtensions
    {
        public static UniTask<Object> LoadAsync(this Reference self, CancellationToken cancellationToken)
        {
            return AssetUtility.LoadAsync(self, cancellationToken);
        }

        public static async UniTask<T> LoadAsync<T>(this Reference<T> self, CancellationToken cancellationToken)
            where T : class
        {
            var asset = await self.ToReference().LoadAsync(cancellationToken);

            switch (asset)
            {
                case T derivedAsset:
                    return derivedAsset;
                case GameObject gameObject:
                    if (gameObject.TryGetComponent<T>(out var component))
                    {
                        return component;
                    }
                    break;
            }

            throw new InvalidCastException($"Unable to cast object {asset.name} of type {asset.GetType().Name} to type {typeof(T).Name}. {nameof(Reference.AssetGuid)}: {self.AssetGuid}");
        }

        public static void Unload(this Reference self)
        {
            AssetUtility.Unload(self);
        }

        public static void Unload<T>(this Reference<T> self)
            where T : class
        {
            self.ToReference().Unload();
        }
    }
}