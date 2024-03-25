using System;
using System.Collections;
using UnityEngine;

namespace Character
{
    public class DroppedCharacter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public Action<DroppedCharacter> OnDead { get; internal set; }

        private CharacterSpeciesData _speciesInfo;

        public void Initialize(Vector3 position, CharacterSpeciesData speciesInfo)
        {
            _speciesInfo = speciesInfo;

            transform.position = position;
            _spriteRenderer.sprite = speciesInfo.Sprite;

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
            this.PlayThrowAnimation(player.transform, _collectAnimationDuration, initialVelocity,
                OnCollectAnimationCompleted);
        }

        private void OnCollectAnimationCompleted()
        {
            CharacterInventory.Instance.AddCharacter(_speciesInfo);
            OnDead?.Invoke(this);
        }
    }
}