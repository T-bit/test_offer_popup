using UnityEngine;

namespace TestOfferPopup.Consumables
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "TestOfferPopup/Consumable")]
    public sealed class Consumable : AddressableScriptableObject<IConsumable>, IConsumable
    {
        [SerializeField]
        private string _title;

        [SerializeField]
        private string _description;

        [SerializeField]
        private Reference<Sprite> _iconReference;

        #region IConsumable

        string IConsumable.Title => _title;

        string IConsumable.Description => _description;

        Reference<Sprite> IConsumable.IconReference => _iconReference;

        #endregion
    }
}