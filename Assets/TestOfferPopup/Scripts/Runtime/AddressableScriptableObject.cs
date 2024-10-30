using UnityEngine;

namespace TestOfferPopup
{
    public abstract class AddressableScriptableObject : ScriptableObject, IAddressable
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

    public abstract class AddressableScriptableObject<T> : AddressableScriptableObject, IAddressable<T>
        where T : class
    {
        #region IAddressable<T>

        Reference<T> IAddressable<T>.Reference => BaseReference.ToReference<T>();

        #endregion
    }
}