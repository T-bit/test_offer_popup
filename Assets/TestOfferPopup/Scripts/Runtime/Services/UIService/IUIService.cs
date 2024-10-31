using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Fragments;

namespace TestOfferPopup.Services
{
    /// <summary>
    /// Service responsible for operations with fragments, e.g. opening, closing, etc.
    /// </summary>
    public interface IUIService : IService
    {
        IEnumerable<IFragment> ActiveFragments { get; }

        UniTask OpenFragmentAsync(Reference<IFragment> fragmentReference, IFragmentModel fragmentModel, CancellationToken cancellationToken);

        UniTask CloseFragmentAsync(IFragmentModel fragmentModel, CancellationToken cancellationToken);
    }
}