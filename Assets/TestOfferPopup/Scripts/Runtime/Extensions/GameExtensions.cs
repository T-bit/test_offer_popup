using System.Linq;
using TestOfferPopup.Services;

namespace TestOfferPopup.Extensions
{
    public static class GameExtensions
    {
        public static bool TryGetService<T>(this IGame self, out T service)
            where T : class, IService
        {
            return self.Services
                       .OfType<T>()
                       .TryGetFirst(out service);
        }
    }
}