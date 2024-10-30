using System.Collections.Generic;
using TestOfferPopup.Services;

namespace TestOfferPopup
{
    public interface IGame
    {
        IEnumerable<IService> Services { get; }
    }
}