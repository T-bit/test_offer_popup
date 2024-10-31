using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Fragments;
using TestOfferPopup.Services;

namespace TestOfferPopup.Utilities
{
    public static class UIUtility
    {
        private static IUIService Service => GameUtility.GetService<IUIService>();

        public static UniTask OpenFragmentAsync<T>(IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            var fragmentReference = AssetUtility.GetReferenceByAddress<IFragment>($"{nameof(Fragment)}/{typeof(T).Name}");
            return OpenFragmentAsync(fragmentReference, fragmentModel, cancellationToken);
        }

        public static UniTask OpenFragmentAsync(Reference<IFragment> fragmentReference, IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            return Service.OpenFragmentAsync(fragmentReference, fragmentModel, cancellationToken);
        }

        public static UniTask CloseFragmentAsync(IFragmentModel fragmentModel, CancellationToken cancellationToken)
        {
            return Service.CloseFragmentAsync(fragmentModel, cancellationToken);
        }
    }
}