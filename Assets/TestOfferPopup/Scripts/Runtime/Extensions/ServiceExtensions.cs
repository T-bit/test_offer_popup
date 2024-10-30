using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestOfferPopup.Services;

namespace TestOfferPopup.Extensions
{
    public static class ServiceExtensions
    {
        public static UniTask InitializeAsync(this IEnumerable<IService> self, CancellationToken cancellationToken)
        {
            return self.Select(item => item.InitializeAsync(cancellationToken))
                       .WhenAll();
        }

        public static UniTask ReleaseAsync(this IEnumerable<IService> self, CancellationToken cancellationToken)
        {
            return self.Select(item => item.ReleaseAsync(cancellationToken))
                       .WhenAll();
        }
    }
}