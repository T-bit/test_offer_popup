using System.Collections.Generic;
using UnityEngine;

namespace TestOfferPopup.Offers
{
    public interface IOffer : IAddressable<IOffer>
    {
        string Title { get; }

        string Description { get; }

        Reference<Sprite> IconReference { get; }

        float Price { get; }

        float Discount { get; }

        IEnumerable<OfferConsumable> Consumables { get; }
    }
}