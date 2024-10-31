using UnityEngine;

namespace TestOfferPopup.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject self)
            where T : Component
        {
            return self.TryGetComponent(out T component)
                ? component
                : self.AddComponent<T>();
        }
    }
}