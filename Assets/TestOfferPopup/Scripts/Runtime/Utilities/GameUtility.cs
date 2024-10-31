using System.Threading;
using TestOfferPopup.Exceptions;
using TestOfferPopup.Extensions;
using TestOfferPopup.Services;
using UnityEngine;

namespace TestOfferPopup.Utilities
{
    public static class GameUtility
    {
        private static IGame GameInstance => Game.Instance;

        public static CancellationToken CancellationToken => GameInstance.CancellationToken;

        public static bool TryGetService<T>(out T service, bool silent = true)
            where T : class, IService
        {
            if (GameInstance != null && GameInstance.TryGetService(out service))
            {
                return true;
            }

            if (!silent)
            {
                Debug.LogWarning(ServiceNotFoundException<T>.ErrorMessage.AddContext(GameInstance));
            }

            service = default;
            return false;

        }

        public static T GetService<T>()
            where T : class, IService
        {
            if (TryGetService<T>(out var service))
            {
                return service;
            }

            throw new ServiceNotFoundException<T>();
        }
    }
}