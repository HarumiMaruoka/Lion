using System;
using System.Collections;
using UnityEngine;

public static class RectTransformExtensions
{
    public static Coroutine StartMoveAsync(this MonoBehaviour monoBehaviour, RectTransform rectTransform, Vector2 startPos, Vector2 endPos, float duration)
    {
        return monoBehaviour.StartCoroutine(
            MoveAsync(rectTransform, startPos, endPos, duration));
    }

    public static IEnumerator MoveAsync(this RectTransform rectTransform, Vector2 startPos, Vector2 endPos, float duration)
    {
        rectTransform.position = startPos;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            rectTransform.position = Vector2.Lerp(startPos, endPos, t / duration);
            yield return null;
        }
        rectTransform.position = endPos;
    }
}