using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private float _exp;
    [SerializeField]
    private float _collectDistance;

    public float EXP => _exp;

    public Action<Gem> OnDead { get; internal set; }

    [SerializeField]
    private float _collectAnimationInitialVelocity;
    [SerializeField]
    private float _collectAnimationDuration;

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        PlayCollectAnimation();
    }

    private void PlayCollectAnimation()
    {
        var player = PlayerController.Current;
        if (player == null) return;

        var dir = (this.transform.position - player.transform.position).normalized;
        var initialVelocity = dir * _collectAnimationInitialVelocity;
        this.StartThrowAnimationCoroutine(player.transform, _collectAnimationDuration, initialVelocity,
            OnCollectAnimationCompleted);
    }

    private void OnCollectAnimationCompleted()
    {
        var player = PlayerController.Current;
        if (player == null) return;

        player.CollectGem(this);
        OnDead?.Invoke(this);
    }
}