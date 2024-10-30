using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Services;
using UnityEngine;

namespace TestOfferPopup
{
    /// <summary>
    /// Basic game logic. Just runs services.
    /// </summary>
    public class Game : MonoBehaviour
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

        #region Unity

        private void Awake()
        {
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

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #endregion
    }
}