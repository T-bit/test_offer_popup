using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Consumables;
using TestOfferPopup.Extensions;
using TestOfferPopup.Offers;
using TestOfferPopup.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestOfferPopup.Fragments
{
    public sealed class MainScreenView : FragmentBehaviour<EmptyFragmentModel>
    {
        private readonly List<string> _offerAddresses = new List<string>();
        private readonly List<string> _consumableAddresses = new List<string>();
        private readonly List<OfferConsumableView> _offerConsumableViews = new List<OfferConsumableView>();

        private CancellationTokenSource _setOfferCancellationTokenSource;
        private bool _offerPopupOpened;

        private Reference<Sprite> _iconReference;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private uint _minOfferConsumableCount;

        [SerializeField]
        private uint _maxOfferConsumableCount;

        [SerializeField]
        private OfferConsumableView _offerConsumableViewPrefab;

        [SerializeField]
        private Transform _offerConsumablesContainer;

        [SerializeField]
        private TMP_Dropdown _offerDropdown;

        [SerializeField]
        private Button _openPopupButton;

        [SerializeField]
        private TMP_InputField _titleInputField;

        [SerializeField]
        private TMP_InputField _descriptionInputField;

        [SerializeField]
        private TMP_InputField _priceInputField;

        [SerializeField]
        private TMP_InputField _discountInputField;

        [SerializeField]
        private Button _addConsumableButton;

        protected override UniTask OnOpenAsync(CancellationToken cancellationToken)
        {
            GetAddresses();
            SetOfferDropdown();

            _offerDropdown.onValueChanged.AddListener(OnOfferDropdownValueChanged);
            _openPopupButton.onClick.AddListener(OnOpenPopupButtonClick);
            _addConsumableButton.onClick.AddListener(OnAddConsumableButtonClick);

            if (_offerAddresses.Count == 0)
            {
                return UniTask.CompletedTask;
            }

            var offerReference = AssetUtility.GetReferenceByAddress<IOffer>(_offerAddresses[0]);

            _setOfferCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, gameObject.GetCancellationTokenOnDestroy());

            return SetOfferSettingsAsync(offerReference, false, cancellationToken);
        }

        protected override UniTask OnCloseAsync(CancellationToken cancellationToken)
        {
            _setOfferCancellationTokenSource?.CancelAndDispose();
            _setOfferCancellationTokenSource = null;

            _offerDropdown.onValueChanged.RemoveListener(OnOfferDropdownValueChanged);
            _openPopupButton.onClick.RemoveListener(OnOpenPopupButtonClick);
            _addConsumableButton.onClick.RemoveListener(OnAddConsumableButtonClick);

            _offerAddresses.Clear();
            _consumableAddresses.Clear();

            return UniTask.CompletedTask;
        }

        private void GetAddresses()
        {
            foreach (var assetAddress in AssetUtility.AssetAddresses)
            {
                if (assetAddress.StartsWith(nameof(Offer)))
                {
                    _offerAddresses.Add(assetAddress);
                }
                else if (assetAddress.StartsWith(nameof(Consumable)))
                {
                    _consumableAddresses.Add(assetAddress);
                }
            }
        }

        private void SetOfferDropdown()
        {
            foreach (var offerAddress in _offerAddresses)
            {
                _offerDropdown.options.Add(new TMP_Dropdown.OptionData(offerAddress.Split('/')[1]));
            }
        }

        private void SetOfferSettings(string offerAddress)
        {
            var offerReference = AssetUtility.GetReferenceByAddress<IOffer>(offerAddress);

            _setOfferCancellationTokenSource?.CancelAndDispose();
            _setOfferCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(gameObject.GetCancellationTokenOnDestroy());
            _canvasGroup.interactable = false;

            SetOfferSettingsAsync(offerReference, true, _setOfferCancellationTokenSource.Token).Forget();
        }

        private async UniTask SetOfferSettingsAsync(Reference<IOffer> offerReference, bool setInteractable, CancellationToken cancellationToken)
        {
            try
            {
                var offer = await offerReference.LoadAsync(cancellationToken);

                _iconReference = offer.IconReference;
                _titleInputField.SetTextWithoutNotify(offer.Title);
                _descriptionInputField.SetTextWithoutNotify(offer.Description);
                _priceInputField.SetTextWithoutNotify(offer.Price.ToString("F"));
                _discountInputField.SetTextWithoutNotify(offer.Discount.ToString("F"));

                ResetConsumables(offer.Consumables);
                offerReference.Unload();
            }
            finally
            {
                if (setInteractable)
                {
                    _canvasGroup.interactable = true;
                }
            }
        }

        private void ResetConsumables(IEnumerable<OfferConsumable> offerConsumables)
        {
            foreach (var offerConsumableView in _offerConsumableViews)
            {
                offerConsumableView.Release();
            }

            _offerConsumableViews.Clear();

            foreach (var offerConsumable in offerConsumables)
            {
                var offerConsumableView = _offerConsumableViewPrefab.Instantiate(_offerConsumablesContainer);
                var consumableIndex = Mathf.Max(AssetUtility.TryGetAssetName(offerConsumable.ConsumableReference.AssetGuid, out var assetName)
                    ? _consumableAddresses.FindIndex(item => item == assetName)
                    : 0, 0);

                offerConsumableView.Initialize(_consumableAddresses, consumableIndex, offerConsumable.Count, OnConsumableDeleteClick);
                _offerConsumableViews.Add(offerConsumableView);
            }

            ResetAddDeleteButtons();
        }

        private void ResetAddDeleteButtons()
        {
            _addConsumableButton.interactable = _offerConsumableViews.Count < _maxOfferConsumableCount;

            var deleteInteractable = _offerConsumableViews.Count > _minOfferConsumableCount;

            foreach (var offerConsumableView in _offerConsumableViews)
            {
                offerConsumableView.EnableDeleteButton(deleteInteractable);
            }
        }

        private async UniTask OpenOfferPopupAsync(CancellationToken cancellationToken)
        {
            var offerPopupModel = new OfferPopupFragmentModel();
            await UIUtility.OpenFragmentAsync<OfferPopupView>(offerPopupModel, cancellationToken);
            await offerPopupModel.CloseSource.Task;

            Debug.Log($"{nameof(OfferPopupView)} is closed. Purchase {(offerPopupModel.Purchased ? "succeeded" : "failed")}.");
        }

        private void OnConsumableDeleteClick(OfferConsumableView offerConsumableView)
        {
            _offerConsumableViews.Remove(offerConsumableView);
            offerConsumableView.Release();

            ResetAddDeleteButtons();
        }

        private void OnOfferDropdownValueChanged(int offerIndex)
        {
            SetOfferSettings(_offerAddresses[offerIndex]);
        }

        private void OnOpenPopupButtonClick()
        {
            if (_offerPopupOpened)
            {
                return;
            }

            OpenOfferPopupAsync(gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnAddConsumableButtonClick()
        {
            var offerConsumableView = _offerConsumableViewPrefab.Instantiate(_offerConsumablesContainer);

            offerConsumableView.Initialize(_consumableAddresses, 0, 1, OnConsumableDeleteClick);
            _offerConsumableViews.Add(offerConsumableView);

            ResetAddDeleteButtons();
        }
    }
}