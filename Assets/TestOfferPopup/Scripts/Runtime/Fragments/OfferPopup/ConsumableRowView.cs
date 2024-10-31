using System.Collections.Generic;
using TestOfferPopup.Consumables;
using UnityEngine;

namespace TestOfferPopup.Fragments
{
    public sealed class ConsumableRowView : MonoBehaviour
    {
        private readonly List<ConsumableView> _consumableViews = new List<ConsumableView>();

        [SerializeField]
        private ConsumableView _consumableViewPrefab;

        public void AddView(Reference<IConsumable> consumableReference, uint count)
        {
            var consumableView = _consumableViewPrefab.Instantiate(transform);
            consumableView.Initialize(consumableReference, count);
            _consumableViews.Add(consumableView);
        }

        public void Release()
        {
            foreach (var consumableView in _consumableViews)
            {
                consumableView.Release();
            }

            _consumableViews.Clear();
            gameObject.Destroy();
        }
    }
}