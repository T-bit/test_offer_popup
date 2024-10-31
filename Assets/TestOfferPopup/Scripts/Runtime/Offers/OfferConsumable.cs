using System;
using TestOfferPopup.Consumables;
using UnityEngine;

namespace TestOfferPopup.Offers
{
    [Serializable]
    public sealed class OfferConsumable
    {
        [SerializeField]
        private Reference<IConsumable> _consumableReference;

        [SerializeField]
        private uint _count;

        public Reference<IConsumable> ConsumableReference => _consumableReference;

        public uint Count => _count;
    }
}