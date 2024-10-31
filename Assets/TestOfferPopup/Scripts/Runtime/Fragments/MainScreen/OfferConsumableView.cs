using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestOfferPopup.Fragments
{
    public class OfferConsumableView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _consumableDropdown;

        [SerializeField]
        private TMP_InputField _countInputField;

        [SerializeField]
        private Button _deleteButton;

        public int ConsumableIndex => _consumableDropdown.value;

        public uint Count => uint.TryParse(_countInputField.text, out var count)
            ? count
            : 0;

        public void Initialize(IEnumerable<string> consumableAddresses, int consumableIndex, uint count, Action<OfferConsumableView> deleteClick)
        {
            foreach (var consumableAddress in consumableAddresses)
            {
                _consumableDropdown.options.Add(new TMP_Dropdown.OptionData(consumableAddress.Split('/')[1]));
            }

            _consumableDropdown.SetValueWithoutNotify(consumableIndex);
            _countInputField.text = count.ToString("N0");
            _deleteButton.onClick.AddListener(OnDeleteClick);

            return;

            void OnDeleteClick()
            {
                deleteClick?.Invoke(this);
            }
        }

        public void Release()
        {
            _consumableDropdown.ClearOptions();
            _deleteButton.onClick.RemoveAllListeners();

            gameObject.Destroy();
        }

        public void EnableDeleteButton(bool interactable)
        {
            _deleteButton.interactable = interactable;
        }
    }
}