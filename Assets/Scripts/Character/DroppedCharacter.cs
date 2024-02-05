using System;
using System.Collections;
using UnityEngine;

public class DroppedCharacter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public Action<DroppedCharacter> OnDead { get; internal set; }

    private RaceCharacterData _raceCharacterData;

    public void Initialize(Vector3 position, RaceCharacterData data)
    {
        _raceCharacterData = data;

        transform.position = position;
        _spriteRenderer.sprite = data.Sprite;

        PlayCollectAnimation();
    }

    [SerializeField]
    private float _collectAnimationInitialVelocity;
    [SerializeField]
    private float _collectAnimationDuration;

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
        CharacterInventory.Instance.GetCharacter(_raceCharacterData);
        OnDead?.Invoke(this);
    }
}