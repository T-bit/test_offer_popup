using System;
using TestOfferPopup.Services;

namespace TestOfferPopup.Exceptions
{
    public sealed class ServiceNotFoundException<T> : Exception
        where T : class, IService
    {
        public static string ErrorMessage => $"{nameof(Service)} \"{typeof(T).Name}\" wasn't found!";

        public ServiceNotFoundException()
            : base(ErrorMessage)
        {
        }
    }
}