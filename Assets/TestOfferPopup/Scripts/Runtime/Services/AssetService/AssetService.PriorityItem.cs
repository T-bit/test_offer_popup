namespace TestOfferPopup.Services
{
    public partial class AssetService
    {
        private readonly struct PriorityItem
        {
            public readonly Reference Reference;

            public readonly int Priority;

            public PriorityItem(Reference reference, int priority)
            {
                Reference = reference;
                Priority = priority;
            }
        }
    }
}