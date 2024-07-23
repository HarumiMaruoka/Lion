using Cysharp.Threading.Tasks;
using Lion.Actor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.Behaviour.HealingOrbModules
{
    public class HealingArea : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        [SerializeField]
        private float _fadeDuration = 0.5f;
        [SerializeField]
        private float _healInterval = 0.5f;

        private float Duration => Parameter == null ? 3f : Parameter.Duration;
        private float Size => Parameter == null ? 1f : Parameter.Size;
        private float HealAmount => Parameter == null ? 1f : Parameter.MagicPower;

        private async void Start()
        {
            try
            {
                // 回復エリア展開
                for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
                {
                    var scale = Mathf.Lerp(0, Size, t / _fadeDuration);
                    transform.localScale = new Vector3(scale, scale, 1);
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }
                transform.localScale = new Vector3(Size, Size, 1);

                // 待機
                for (float t = 0; t < Duration; t += Time.deltaTime)
                {
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }

                // 回復エリア消滅
                for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
                {
                    var scale = Mathf.Lerp(Size, 0, t / _fadeDuration);
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

        private Dictionary<GameObject, ContinualHealing> _continualHealingDict = new Dictionary<GameObject, ContinualHealing>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ActorManager.TryGetActor(collision.gameObject, out var actor))
            {
                if (!_continualHealingDict.ContainsKey(collision.gameObject))
                {
                    var continualHealing = collision.gameObject.AddComponent<ContinualHealing>();
                    continualHealing.Actor = actor;
                    continualHealing.HealAmount = HealAmount;
                    continualHealing.HealInterval = _healInterval;
                    continualHealing.Duration = Duration;
                    _continualHealingDict[collision.gameObject] = continualHealing;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_continualHealingDict.TryGetValue(collision.gameObject, out var continualHealing))
            {
                Destroy(continualHealing);
                _continualHealingDict.Remove(collision.gameObject);
            }
        }

        private void OnDestroy()
        {
            foreach (var continualHealing in _continualHealingDict.Values)
            {
                Destroy(continualHealing);
            }
        }
    }
}