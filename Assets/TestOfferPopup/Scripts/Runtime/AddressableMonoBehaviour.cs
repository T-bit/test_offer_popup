using UnityEngine;

namespace TestOfferPopup
{
    public abstract class AddressableMonoBehaviour : MonoBehaviour, IAddressable
    {
        [SerializeField]
        private Reference _reference;

        protected virtual string Name => name;

        protected Reference BaseReference => _reference;

        #region IAddressable

        string IAddressable.Name => Name;

        Reference IAddressable.BaseReference => BaseReference;

        #endregion
    }

    public abstract class AddressableMonoBehaviour<T> : AddressableMonoBehaviour, IAddressable<T>
        where T : class
    {
        #region IAddressable<T>

        Reference<T> IAddressable<T>.Reference => BaseReference.ToReference<T>();

        #endregion
    }
}