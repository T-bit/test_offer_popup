using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TestOfferPopup.Extensions
{
    public static class ImageExtensions
    {
        public static UniTask SetIconAsync(this Image self, Reference<Sprite> spriteReference)
        {
            var loader = self.gameObject.GetOrAddComponent<AddressableSpriteLoader>();
            return loader.SetSpriteAsync(self, spriteReference);
        }
    }
}