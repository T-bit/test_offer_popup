using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Fragments;
using TestOfferPopup.Services;
using TestOfferPopup.Utilities;
using UnityEngine;

namespace TestOfferPopup
{
    /// <summary>
    /// Basic game logic. Just runs services (without prioritization) and opens main screen.
    /// </summary>
    public class Game : MonoSingleton<Game>, IGame
    {
        private readonly List<IService> _services = new List<IService>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        private async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _services.InitializeAsync(cancellationToken);
            await UIUtility.OpenFragmentAsync<MainScreen>(new EmptyFragmentModel(), cancellationToken);
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
            Debug.Log("Game stop.".AddContext(this));
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