using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestOfferPopup.Fragments
{
    public sealed class OfferPopupView : FragmentBehaviour<OfferPopupFragmentModel>
    {
        private readonly List<ConsumableRowView> _consumableRowViews = new List<ConsumableRowView>();

        [SerializeField]
        private ConsumableRowView _consumableRowViewPrefab;

        [SerializeField]
        private Transform _consumableViewRowsContainer;

        [SerializeField]
        private TMP_Text _titleText;

        [SerializeField]
        private TMP_Text _descriptionText;

        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private TMP_Text _oldPriceText;

        [SerializeField]
        private TMP_Text _priceText;

        [SerializeField]
        private GameObject _discountPanel;

        [SerializeField]
        private TMP_Text _discountText;

        [SerializeField]
        private Button _purchaseButton;

        [SerializeField]
        private Button _closeButton;

        protected override UniTask OnOpenAsync(CancellationToken cancellationToken)
        {
            _titleText.text = DerivedFragmentModel.Title;
            _descriptionText.text = DerivedFragmentModel.Description;

            var price = DerivedFragmentModel.Price;
            var discount = DerivedFragmentModel.Discount;
            var activeDiscount = !Mathf.Approximately(0, discount);

            _discountPanel.SetActive(activeDiscount);
            _oldPriceText.gameObject.SetActive(activeDiscount);

            _priceText.text = $"${price:F}";
            _discountText.text = $"-{discount:N0}%";
            _oldPriceText.text = $"${price * (1f + discount / 100f):F}";

            var consumableCount = 0;
            var consumableRowView = _consumableRowViewPrefab.Instantiate(_consumableViewRowsContainer);

            foreach (var (consumableReference, count) in DerivedFragmentModel.Consumables)
            {
                if (consumableCount > 0 && consumableCount % 3 == 0)
                {
                    consumableRowView = _consumableRowViewPrefab.Instantiate(_consumableViewRowsContainer);
                    _consumableRowViews.Add(consumableRowView);
                }

                consumableRowView.AddView(consumableReference, count);
                consumableCount++;
            }

            _purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);

            return UniTask.CompletedTask;
        }

        protected override UniTask OnCloseAsync(CancellationToken cancellationToken)
        {
            _purchaseButton.onClick.RemoveListener(OnPurchaseButtonClick);
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);

            foreach (var consumableRowView in _consumableRowViews)
            {
                consumableRowView.Release();
            }

            _consumableRowViews.Clear();

            return UniTask.CompletedTask;
        }

        private void OnPurchaseButtonClick()
        {
            DerivedFragmentModel.Purchased = true;

            UIUtility.CloseFragmentAsync(FragmentModel, GameUtility.CancellationToken).Forget();
        }

        private void OnCloseButtonClick()
        {
            UIUtility.CloseFragmentAsync(FragmentModel, GameUtility.CancellationToken).Forget();
        }
    }
}