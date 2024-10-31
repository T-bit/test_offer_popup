using UnityEngine;

namespace TestOfferPopup.Consumables
{
    public interface IConsumable
    {
        string Title { get; }

        string Description { get; }

        Reference<Sprite> IconReference { get; }
    }
}