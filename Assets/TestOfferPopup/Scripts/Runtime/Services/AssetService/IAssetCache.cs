using System;
using System.Collections.Generic;

namespace TestOfferPopup.Services
{
    public interface IAssetCache : IDisposable
    {
        IEnumerable<string> AssetAddresses
        {
            get;
        }

        bool TryGetAssetGuid(string assetAddress, out string assetGuid);

        bool TryGetAssetName(string assetGuid, out string assetName);
    }
}