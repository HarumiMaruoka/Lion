using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public static class ThrowAnimationExtensions
{
    public static Coroutine PlayThrowAnimation(this MonoBehaviour throwObject, Transform targetPosition, float duration, Vector3 initialVelocity,
        Action onCompleted = default, CancellationToken token = default)
    {
        return throwObject.StartCoroutine(
            ThrowAnimationAsync(throwObject.transform, targetPosition, duration, initialVelocity, onCompleted, token));
    }

    private static float TimeScale => GameSpeedManager.Instance.TimeScale;

    public static IEnumerator ThrowAnimationAsync(Transform start, Transform end, float duration, Vector3 initialVelocity,
        Action onCompleted = default, CancellationToken token = default)
    {
        var position = start.position;
        var velocity = initialVelocity;

        while (duration > 0f && !token.IsCancellationRequested)
        {
            var acceleration = Vector3.zero;

            var diff = end.position - position;
            acceleration += (diff - velocity * duration) * 2f / (duration * duration);

            duration -= Time.deltaTime * TimeScale;

            velocity += acceleration * Time.deltaTime * TimeScale;
            position += velocity * Time.deltaTime * TimeScale;
            start.position = position;

            yield return null;
        }
        onCompleted?.Invoke();
    }
}