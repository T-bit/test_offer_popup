namespace TestOfferPopup
{
    public interface IAddressable
    {
        string Name { get; }

        Reference BaseReference { get; }
    }

    public interface IAddressable<T> : IAddressable
        where T : class
    {
        Reference<T> Reference { get; }
    }
}