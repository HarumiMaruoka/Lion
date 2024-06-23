using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Glib.NovelGameEditor
{
    public static class ImageExtensions
    {
        public static async UniTask FadeInAsync(this Image image, float duration, CancellationToken token = default)
        {
            await FadeAsync(image, 1f, duration, token);
        }

        public static async UniTask FadeOutAsync(this Image image, float duration, CancellationToken token = default)
        {
            await FadeAsync(image, 0f, duration, token);
        }

        public static async UniTask FadeAsync(this Image image, float target, float duration, CancellationToken token = default)
        {
            var from = image.color;
            var to = image.color;
            to.a = target;

            for (var t = 0F; t < duration; t += Time.deltaTime)
            {
                image.color = Color.Lerp(from, to, t / duration);
                await UniTask.Yield(token);
            }
        }
    }
}