using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public static class TextExtensions
{
    private static float TimeScale => GameSpeedManager.Instance.TimeScale;

    public static IEnumerator FadeAsync(this Text text, float to, float duration, CancellationToken token = default)
    {
        var col = text.color;
        var startColor = col;
        col.a = to;
        var endColor = col;

        for (var t = 0f; t < duration && !token.IsCancellationRequested; t += Time.deltaTime * TimeScale)
        {
            text.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;
        }
        text.color = endColor;
    }
}