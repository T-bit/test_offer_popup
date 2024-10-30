using System;
using System.Collections.Generic;
using TestOfferPopup.Utilities;

namespace TestOfferPopup.Scopes
{
    public readonly struct ListScope<T> : IDisposable
    {
        private readonly List<T> _list;

        private ListScope(List<T> list)
        {
            _list = list;
        }

        public static ListScope<T> Create(out List<T> list)
        {
            list = PoolUtility.PullList<T>();
            return new ListScope<T>(list);
        }

        public void Dispose()
        {
            _list.Clear();
            PoolUtility.PushList(_list);
        }
    }
}