using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour.ForceBarrierModules
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D _collider;
        [SerializeField]
        private float _scale = 1.5f;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        public IWeaponParameter Parameter { get; set; }

        private float Duration => Parameter == null ? 3f : 3f + Parameter.Duration / 100f;

        private async void Start()
        {
            try
            {
                // バリア展開
                for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
                {
                    var scale = Mathf.Lerp(0, _scale, t / _fadeDuration);
                    transform.localScale = new Vector3(scale, scale, 1);
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }
                transform.localScale = new Vector3(_scale, _scale, 1);

                // 待機
                for (float t = 0; t < Duration; t += Time.deltaTime)
                {
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }

                // バリア消滅
                for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
                {
                    var scale = Mathf.Lerp(_scale, 0, t / _fadeDuration);
                    transform.localScale = new Vector3(scale, scale, 1);
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }

                Destroy(gameObject);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}