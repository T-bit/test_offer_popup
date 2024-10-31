using System;
using TestOfferPopup.Extensions;
using TestOfferPopup.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestOfferPopup
{
    [Serializable]
    public struct Reference : IComparable<Reference>, IReference
    {
        [SerializeField]
        private string _assetGuid;

        public Reference(string assetGuid)
        {
            Assert.IsTrue(assetGuid.IsGuid());

            _assetGuid = assetGuid;
        }

        public readonly string AssetGuid => _assetGuid ?? string.Empty;

        public readonly bool HasValidAssetGuid => Guid.TryParse(AssetGuid, out _);

        public static bool operator ==(Reference x, Reference y) => x.AssetGuid == y.AssetGuid;

        public static bool operator !=(Reference x, Reference y) => x.AssetGuid != y.AssetGuid;

        public readonly override bool Equals(object other)
        {
            return other != null && other.GetHashCode() == GetHashCode();
        }

        public readonly override int GetHashCode()
        {
            return AssetGuid.GetHashCode();
        }

        public readonly Reference<T> ToReference<T>()
            where T : class
        {
            return string.IsNullOrWhiteSpace(AssetGuid) ? default : new Reference<T>(AssetGuid);
        }

        public readonly override string ToString()
        {
            return AssetGuid;
        }

        #region IComparable<Reference>

        int IComparable<Reference>.CompareTo(Reference other)
        {
            return ReferenceUtility.Compare(this, other);
        }

        #endregion
    }

    [Serializable]
    public struct Reference<T> : IComparable<Reference<T>>, IReference
        where T : class
    {
        [SerializeField]
        private string _assetGuid;

        public Reference(string assetGuid)
        {
            Assert.IsTrue(assetGuid.IsGuid());

            _assetGuid = assetGuid;
        }

        public readonly string AssetGuid => _assetGuid ?? string.Empty;

        public readonly bool HasValidAssetGuid => Guid.TryParse(AssetGuid, out _);

        public static bool operator ==(Reference<T> x, Reference y) => x.AssetGuid == y.AssetGuid;

        public static bool operator ==(Reference<T> x, Reference<T> y) => x.AssetGuid == y.AssetGuid;

        public static bool operator !=(Reference<T> x, Reference y) => x.AssetGuid != y.AssetGuid;

        public static bool operator !=(Reference<T> x, Reference<T> y) => x.AssetGuid != y.AssetGuid;

        public static implicit operator Reference(Reference<T> value)
        {
            return value.ToReference();
        }

        public readonly override bool Equals(object other)
        {
            return other != null && other.GetHashCode() == GetHashCode();
        }

        public readonly override int GetHashCode()
        {
            return AssetGuid.GetHashCode();
        }

        public readonly Reference ToReference()
        {
            return string.IsNullOrWhiteSpace(AssetGuid) ? default : new Reference(AssetGuid);
        }

        public readonly Reference<TRefence> ToReference<TRefence>()
            where TRefence : class
        {
            return string.IsNullOrWhiteSpace(AssetGuid) ? default : new Reference<TRefence>(AssetGuid);
        }

        public readonly override string ToString()
        {
            return AssetGuid;
        }

        #region IComparable<Reference<T>>

        int IComparable<Reference<T>>.CompareTo(Reference<T> other)
        {
            return ReferenceUtility.Compare(this, other);
        }

        #endregion
    }
}