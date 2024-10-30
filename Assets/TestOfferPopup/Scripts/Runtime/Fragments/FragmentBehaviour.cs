using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TestOfferPopup.Fragments
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Fragment))]
    public abstract class FragmentBehaviour : MonoBehaviour, IFragmentBehaviour
    {
        private CancellationToken _destroyCancellationToken;

        [HideInInspector]
        [SerializeField]
        private Fragment _fragment;

        private IFragment Fragment => _fragment;

        protected IFragmentModel FragmentModel => Fragment.Model;

        protected virtual UniTask OnOpenAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnCloseAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        #region IFragmentBehaviour

        async UniTask IFragmentBehaviour.OpenAsync(CancellationToken cancellationToken)
        {
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _destroyCancellationToken);
            var linkedToken = linkedTokenSource.Token;

            await OnOpenAsync(linkedToken);
        }

        async UniTask IFragmentBehaviour.CloseAsync(CancellationToken cancellationToken)
        {
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _destroyCancellationToken);
            var linkedToken = linkedTokenSource.Token;

            await OnCloseAsync(linkedToken);
        }

        #endregion

        #region Unity

        private void Awake()
        {
            _destroyCancellationToken = gameObject.GetCancellationTokenOnDestroy();
        }

        private void OnValidate()
        {
            _fragment = GetComponent<Fragment>();
        }

        #endregion
    }

    public abstract class FragmentBehaviour<T> : FragmentBehaviour
        where T : IFragmentModel
    {
        protected T DerivedFragmentModel => (T)FragmentModel;
    }
}