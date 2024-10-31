using Cysharp.Threading.Tasks;

namespace TestOfferPopup.Fragments
{
    public abstract class FragmentModel : IFragmentModel
    {
        public UniTaskCompletionSource CloseSource { get; set; }

        #region IFragmentModel

        UniTaskCompletionSource IFragmentModel.CloseSource => CloseSource;

        #endregion
    }
}