using System.Collections.Generic;
using TestOfferPopup.Consumables;
using UnityEngine;

namespace TestOfferPopup.Fragments
{
    public sealed class OfferPopupFragmentModel : FragmentModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Reference<Sprite> Icon { get; set; }

        public float Price { get; set; }

        public float Discount { get; set; }

        public IEnumerable<(Reference<IConsumable> ConsumableReference, uint Count)> Consumables { get; set; }

        public bool Purchased { get; set; }
    }
}