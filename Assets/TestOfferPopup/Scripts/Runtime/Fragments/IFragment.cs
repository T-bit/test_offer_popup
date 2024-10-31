using System.Threading;
using Cysharp.Threading.Tasks;

namespace TestOfferPopup.Fragments
{
    public interface IFragment : IAddressable<IFragment>
    {
        IFragmentModel Model { get; }

        UniTask OpenAsync(IFragmentModel model, CancellationToken cancellationToken);

        UniTask CloseAsync(CancellationToken cancellationToken);
    }
}