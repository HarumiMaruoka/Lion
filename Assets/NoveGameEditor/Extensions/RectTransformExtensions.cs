using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Glib.NovelGameEditor
{
    public static class RectTransformExtensions
    {
        public static async UniTask MoveAsync(this RectTransform rectTransform, Vector2 position, float duration)
        {
            var from = rectTransform.anchoredPosition;
            var to = position;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(from, to, t / duration);
                await UniTask.Yield();
            }

            rectTransform.anchoredPosition = to;
        }
    }
}