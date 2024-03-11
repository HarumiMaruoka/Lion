using System;
using System.Collections;
using System.Threading;
using UnityEngine;

[Serializable]
public class Regeneration : IFoodEffect
{
    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _interval;
    [SerializeField]
    private float _amount;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    public IEnumerator RunAsync(CancellationToken token)
    {
        int count = 0;
        for (float elapsed = 0f; elapsed < _duration; elapsed += Time.deltaTime * TimeScale)
        {
            if (elapsed > _interval * count)
            {
                PlayerController.Current.Heal(_amount);
                count++;
            }

            yield return null;
        }
    }
}