using System.Collections.Generic;
using System.Threading;
using TestOfferPopup.Services;

namespace TestOfferPopup
{
    public interface IGame
    {
        IEnumerable<IService> Services { get; }

        CancellationToken CancellationToken { get; }
    }
}