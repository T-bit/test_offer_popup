using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TestOfferPopup
{
    public class AddressableSpriteLoader : AddressableLoaderBase<Sprite>
    {
        public UniTask SetSpriteAsync(Image image, Reference<Sprite> reference)
        {
            return !IsReferenceLoaded(reference)
                ? LoadAndSetAsync(reference, image)
                : UniTask.CompletedTask;
        }

        public UniTask SetSpriteAsync(SpriteRenderer spriteRenderer, Reference<Sprite> reference)
        {
            return !IsReferenceLoaded(reference)
                ? LoadAndSetAsync(reference, spriteRenderer)
                : UniTask.CompletedTask;
        }

        private async UniTask LoadAndSetAsync(Reference<Sprite> reference, Image image)
        {
            image.enabled = false;

            var sprite = await LoadAssetAsync(reference);

            image.sprite = sprite;
            image.enabled = true;
        }

        private async UniTask LoadAndSetAsync(Reference<Sprite> reference, SpriteRenderer spriteRenderer)
        {
            spriteRenderer.enabled = false;

            var sprite = await LoadAssetAsync(reference);

            spriteRenderer.sprite = sprite;
            spriteRenderer.enabled = true;
        }
    }
}