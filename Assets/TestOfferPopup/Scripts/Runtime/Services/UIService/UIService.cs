﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Fragments;
using UnityEngine;

namespace TestOfferPopup.Services
{
    /// <summary>
    /// Basic implementation, without counters.
    /// </summary>
    public class UIService : Service, IUIService
    {
        [NonSerialized]
        private readonly Dictionary<IFragmentModel, IFragment> _fragments = new Dictionary<IFragmentModel, IFragment>();

        [SerializeField]
        private Canvas _canvas;

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _fragments.Clear();

            return UniTask.CompletedTask;
        }

        #region IUIService

        IEnumerable<IFragment> IUIService.ActiveFragments => _fragments.Values;

        async UniTask<IFragment> IUIService.OpenFragmentAsync(Reference<IFragment> fragmentReference, IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            if (_fragments.TryGetValue(fragmentModel, out var fragment))
            {
                return fragment;
            }

            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, cancellationToken);
            var linkedToken = linkedTokenSource.Token;
            var fragmentPrefab = await fragmentReference.LoadAsync(linkedToken);

            _fragments[fragmentModel] = fragment = fragmentPrefab.Clone(_canvas.transform);

            await fragment.OpenAsync(fragmentModel, linkedToken);

            return fragment;
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