using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Services;
using UnityEngine;

namespace TestOfferPopup
{
    /// <summary>
    /// Basic game logic. Just runs services and opens main screen.
    /// </summary>
    public class Game : MonoSingleton<Game>, IGame
    {
        private readonly List<IService> _services = new List<IService>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        private UniTask StartAsync(CancellationToken cancellationToken)
        {
            return _services.InitializeAsync(cancellationToken);
        }

        private UniTask StopAsync(CancellationToken cancellationToken)
        {
            return _services.ReleaseAsync(cancellationToken);
        }

        #region IGame

        IEnumerable<IService> IGame.Services => _services;

        #endregion

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            gameObject.GetComponentsInChildren(_services);
        }

        private void Start()
        {
            Debug.Log("Game start.".AddContext(this));
            gameObject.DontDestroyOnLoad();
            StartAsync(CancellationToken).Forget();
        }

        private void OnApplicationQuit()
        {
            StopAsync(CancellationToken).Forget();
        }

        protected override void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            base.OnDestroy();
        }

        #endregion
    }
}