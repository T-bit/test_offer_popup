using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Extensions;
using TestOfferPopup.Fragments;
using TestOfferPopup.Services;

namespace TestOfferPopup.Utilities
{
    public static class UIUtility
    {
        private static IUIService Service => GameUtility.GetService<IUIService>();

        public static UniTask<IFragment> OpenFragmentAsync(Reference<IFragment> fragmentReference, IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            return Service.OpenFragmentAsync(fragmentReference, fragmentModel, cancellationToken);
        }

        public static UniTask CloseFragmentAsync(IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            return Service.CloseFragmentAsync(fragmentModel, cancellationToken);
        }

        public static UniTask CloseFragmentAsync(Reference<IFragment> fragmentReference, CancellationToken cancellationToken)
        {
            return Service.CloseFragmentAsync(fragmentReference, cancellationToken);
        }
    }
}