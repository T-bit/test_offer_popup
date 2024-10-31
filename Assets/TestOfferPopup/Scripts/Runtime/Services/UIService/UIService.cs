using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Fragments;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestOfferPopup.Services
{
    /// <summary>
    /// Basic implementation, without counters.
    /// </summary>
    public class UIService : Service, IUIService
    {
        private Canvas _canvas;

        [NonSerialized]
        private readonly Dictionary<IFragmentModel, IFragment> _fragments = new Dictionary<IFragmentModel, IFragment>();

        [SerializeField]
        private Canvas _canvasPrefab;

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            Assert.IsNotNull(_canvasPrefab);

            _canvas = _canvasPrefab.Instantiate().WithName(_canvasPrefab.name);
            _canvas.gameObject.DontDestroyOnLoad();

            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _fragments.Clear();
            _canvas.Destroy();
            _canvas = null;

            return UniTask.CompletedTask;
        }

        #region IUIService

        IEnumerable<IFragment> IUIService.ActiveFragments => _fragments.Values;

        async UniTask IUIService.OpenFragmentAsync(Reference<IFragment> fragmentReference, IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            if (_fragments.ContainsKey(fragmentModel))
            {
                return;
            }

            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, cancellationToken);
            var linkedToken = linkedTokenSource.Token;
            var fragmentPrefab = await fragmentReference.LoadAsync(linkedToken);

            var fragment = _fragments[fragmentModel] = fragmentPrefab.Clone(_canvas.transform);

            await fragment.OpenAsync(fragmentModel, linkedToken);
        }

        async UniTask IUIService.CloseFragmentAsync(IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            if (!_fragments.TryRemoveValue(fragmentModel, out var fragment) || fragment == null)
            {
                return;
            }

            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, cancellationToken);
            var linkedToken = linkedTokenSource.Token;

            await fragment.CloseAsync(linkedToken);

            fragment.Destroy();
        }

        #endregion
    }
}