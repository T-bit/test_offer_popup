using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Fragments;
using TestOfferPopup.Services;

namespace TestOfferPopup.Extensions
{
    public static class UIServiceExtensions
    {
        public static UniTask CloseFragmentAsync(this IUIService self, Reference<IFragment> fragmentReference, CancellationToken cancellationToken)
        {
            return self.ActiveFragments.TryGetFirst(item => item.Reference == fragmentReference, out var fragment)
                ? self.CloseFragmentAsync(fragment.Model, cancellationToken)
                : UniTask.CompletedTask;
        }
    }
}