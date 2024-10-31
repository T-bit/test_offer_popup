using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Consumables;
using TestOfferPopup.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestOfferPopup.Fragments
{
    public sealed class ConsumableView : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private TMP_Text _countText;

        public void Initialize(Reference<IConsumable> consumableReference, uint count)
        {
            _cancellationTokenSource?.CancelAndDispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(gameObject.GetCancellationTokenOnDestroy());


        }

        private async UniTask InitializeAsync(Reference<IConsumable> consumableReference, uint count, CancellationToken cancellationToken)
        {

        }

        public void Release()
        {
            _cancellationTokenSource?.CancelAndDispose();
            _cancellationTokenSource = null;

            gameObject.Destroy();
        }
    }
}