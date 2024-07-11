using System;
using System.Collections;
using UnityEngine;

public static class SpriteRendererExtensions
{
    public static IEnumerator FadeIn(this SpriteRenderer spriteRenderer, float duration)
    {
        var startAlpha = spriteRenderer.color.a;
        var endAlpha = 1f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            var alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, endAlpha);
    }

    public static IEnumerator FadeOut(this SpriteRenderer spriteRenderer, float duration)
    {
        var startAlpha = spriteRenderer.color.a;
        var endAlpha = 0f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            var alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, endAlpha);
    }
}
