using System;

namespace TestOfferPopup.Extensions
{
    public static class StringExtensions
    {
        public static bool IsGuid(this string self)
        {
            return Guid.TryParse(self, out _);
        }

        public static string AddContext<T>(this string self)
        {
            return self.AddContext(typeof(T).Name);
        }

        public static string AddContext<T>(this string self, T context)
            where T : class
        {
            return self.AddContext(context.GetType().Name);
        }

        public static string AddContext(this string self, string context)
        {
            return $"{context.AddBold()}: {self}";
        }

        public static string AddBold(this string self)
        {
            return $"<b>{self}</b>";
        }
    }
}