using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestOfferPopup.Services
{
    [DisallowMultipleComponent]
    public abstract class Service : MonoBehaviour, IService
    {
        [NonSerialized]
        private bool _initialized;

        [NonSerialized]
        private CancellationTokenSource _cancellationTokenSource;

        protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

        protected virtual UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        #region IService

        bool IService.Initialized => _initialized;

        async UniTask IService.InitializeAsync(CancellationToken cancellationToken)
        {
            Assert.IsFalse(_initialized);

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(gameObject.GetCancellationTokenOnDestroy());

            await OnInitializeAsync(cancellationToken);

            _initialized = true;
        }

        async UniTask IService.ReleaseAsync(CancellationToken cancellationToken)
        {
            Assert.IsTrue(_initialized);

            _cancellationTokenSource.CancelAndDispose();
            _cancellationTokenSource = null;

            await OnReleaseAsync(cancellationToken);

            _initialized = false;
        }

        #endregion
    }
}