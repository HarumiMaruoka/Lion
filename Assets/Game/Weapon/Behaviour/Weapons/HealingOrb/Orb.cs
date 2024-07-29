using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using Lion.CameraUtility;
using System;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour.HealingOrbModules
{
    public class Orb : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        [SerializeField]
        private HealingArea _healingSpotPrefab;

        [SerializeField]
        private Ease _ease = Ease.Linear;
        [SerializeField]
        private float _fallingDuration = 1f;

        TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> _moveTween;

        private void Start()
        {
            _moveTween = transform.DOMove(GetTargetPosition(), _fallingDuration).SetEase(_ease).OnComplete(OnComplete);
        }

        private void OnDestroy()
        {
            _moveTween.Kill();
        }

        private Vector3 GetTargetPosition()
        {
            var top = Camera.main.GetWorldTopRight().y;
            var bottom = Camera.main.GetWorldBottomLeft().y;

            var x = transform.position.x;
            var y = UnityEngine.Random.Range(bottom, top);

            return new Vector3(x, y, 0);
        }

        private async void OnComplete()
        {
            try
            {
                var startScale = transform.localScale;
                var duration = 0.5f;

                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    var scaleX = Mathf.Lerp(startScale.x, 0, t / duration);
                    var scaleY = Mathf.Lerp(startScale.y, 0, t / duration);
                    transform.localScale = new Vector3(scaleX, scaleY, 1);
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }
                var healingSpot = Instantiate(_healingSpotPrefab, transform.position, Quaternion.Euler(90, 0, 0));
                healingSpot.Parameter = Parameter;

                Destroy(gameObject);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}