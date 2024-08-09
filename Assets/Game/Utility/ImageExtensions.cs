using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static IEnumerator FadeAsync(this Image image, float to, float duration, CancellationToken token = default)
    {
        var col = image.color;
        var startColor = col;
        col.a = to;
        var endColor = col;

        for (var t = 0f; t < duration && !token.IsCancellationRequested; t += Time.deltaTime)
        {
            image.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;
        }
        image.color = endColor;
    }

    public static IEnumerator FadeInAsync(this Image image, float duration, CancellationToken token = default)
    {
        return image.FadeAsync(1, duration, token);
    }

    public static IEnumerator FadeOutAsync(this Image image, float duration, CancellationToken token = default)
    {
        return image.FadeAsync(0, duration, token);
    }
}