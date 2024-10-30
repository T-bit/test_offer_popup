using System.Collections.Generic;
using UnityEngine.Assertions;

namespace TestOfferPopup.Utilities
{
    public static class PoolUtility<T>
        where T : class, new()
    {
        private static readonly Stack<T> _stack = new Stack<T>();

        public static void Push(T value)
        {
            Assert.IsFalse(_stack.Contains(value));
            _stack.Push(value);
        }

        public static T Pull()
        {
            return _stack.Count > 0
                ? _stack.Pop()
                : new T();
        }
    }

    public static class PoolUtility
    {
        public static void PushList<T>(List<T> value)
        {
            PoolUtility<List<T>>.Push(value);
        }

        public static List<T> PullList<T>()
        {
            return PoolUtility<List<T>>.Pull();
        }
    }
}