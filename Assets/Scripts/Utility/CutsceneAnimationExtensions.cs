using System;
using System.Collections;
using UnityEngine;

public static class CutsceneAnimationExtensions
{
    public static Coroutine StartRotateAnimationCoroutine(this MonoBehaviour monoBehaviour, float rotateSpeed, float duration, RotateDirection dir)
    {
        return monoBehaviour.StartCoroutine(RotateAnimationAsync(monoBehaviour.transform, rotateSpeed, duration, dir));
    }

    public static IEnumerator RotateAnimationAsync(Transform target, float duration, float rotateSpeed = 1f, RotateDirection dir = RotateDirection.Right)
    {
        float angle = 0f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            target.rotation = rotation;

            if (dir == RotateDirection.Right)
                angle += rotateSpeed * Time.deltaTime;
            else
                angle -= rotateSpeed * Time.deltaTime;

            yield return null;
        }
    }

    public enum RotateDirection
    {
        Right, Left
    }
}