using Cysharp.Threading.Tasks;

namespace TestOfferPopup.Fragments
{
    public interface IFragmentModel
    {
        UniTaskCompletionSource CloseSource { get; }
    }
}