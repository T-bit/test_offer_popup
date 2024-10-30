using System.Threading;

namespace TestOfferPopup.Extensions
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelAndDispose(this CancellationTokenSource self)
        {
            self.Cancel();
            self.Dispose();
        }
    }
}