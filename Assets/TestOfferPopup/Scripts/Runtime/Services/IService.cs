using System.Threading;
using Cysharp.Threading.Tasks;

namespace TestOfferPopup.Services
{
    public interface IService
    {
        bool Initialized { get; }

        UniTask InitializeAsync(CancellationToken cancellationToken);

        UniTask ReleaseAsync(CancellationToken cancellationToken);
    }
}