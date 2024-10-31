using System.Collections.Generic;
using UnityEngine;

namespace TestOfferPopup.Offers
{
    [CreateAssetMenu(fileName = "Offer", menuName = "TestOfferPopup/Offer")]
    public sealed class Offer : AddressableScriptableObject<IOffer>, IOffer
    {
        [SerializeField]
        private string _title;

        [SerializeField]
        [TextArea]
        private string _description;

        [SerializeField]
        private Reference<Sprite> _iconReference;

        [SerializeField]
        private float _price;

        [SerializeField]
        private float _discount;

        [SerializeField]
        private OfferConsumable[] _consumables;

        #region IOffer

        string IOffer.Title => _title;

        string IOffer.Description => _description;

        Reference<Sprite> IOffer.IconReference => _iconReference;

        float IOffer.Price => _price;

        float IOffer.Discount => _discount;

        IEnumerable<OfferConsumable> IOffer.Consumables => _consumables;

        #endregion
    }
}