using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TestOfferPopup.Fragments
{
    public sealed class Fragment : AddressableMonoBehaviour<IFragment>, IFragment
    {
        private IFragmentModel _model;

        [HideInInspector]
        [SerializeField]
        private FragmentBehaviour _behaviour;

        [HideInInspector]
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private IFragmentBehaviour Behaviour => _behaviour;

        #region IFragment

        IFragmentModel IFragment.Model => _model;

        async UniTask IFragment.OpenAsync(IFragmentModel model, CancellationToken cancellationToken)
        {
            _model = model;

            if (_canvasGroup != null)
            {
                _canvasGroup.interactable = false;
            }

            try
            {
                await Behaviour.OpenAsync(cancellationToken);
            }
            finally
            {
                if (_canvasGroup != null)
                {
                    _canvasGroup.interactable = true;
                }
            }
        }

        async UniTask IFragment.CloseAsync(CancellationToken cancellationToken)
        {
            await Behaviour.CloseAsync(cancellationToken);

            _model.CloseSource?.TrySetResult();
            _model = null;
        }

        #endregion

        #region Unity

        private void OnValidate()
        {
            _behaviour = GetComponent<FragmentBehaviour>();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        #endregion
    }
}