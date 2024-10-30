using System.Threading;
using Cysharp.Threading.Tasks;

namespace TestOfferPopup.Fragments
{
    public interface IFragmentBehaviour
    {
        UniTask OpenAsync(CancellationToken cancellationToken);

        UniTask CloseAsync(CancellationToken cancellationToken);
    }
}