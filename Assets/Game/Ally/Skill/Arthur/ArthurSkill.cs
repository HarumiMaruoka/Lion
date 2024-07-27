using Cysharp.Threading.Tasks;
using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Ally.Skill
{
    public class ArthurSkill : SkillBase
    {
        [SerializeField]
        private float _scaleTime = 0.5f;
        [SerializeField]
        private float _duration = 1f;
        [SerializeField]
        private float _shrinkTime = 0.5f;

        [SerializeField]
        private float _laserWidth = 0.1f;
        [SerializeField]
        private float _flashSize = 0.1f;

        [SerializeField]
        private LineRenderer _laser;
        [SerializeField]
        private Transform _flash;

        private async void Start()
        {
            try
            {
                _laser.startWidth = 0;
                _laser.endWidth = 0;
                _flash.localScale = Vector3.zero;

                for (float t = 0f; t <= _scaleTime; t += Time.deltaTime)
                {
                    _laser.startWidth = Mathf.Lerp(0, _laserWidth, t / _scaleTime);
                    _laser.endWidth = Mathf.Lerp(0, _laserWidth, t / _scaleTime);
                    _flash.localScale = Vector3.one * Mathf.Lerp(0, _flashSize, t / _scaleTime);

                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }

                _laser.startWidth = _laserWidth;
                _laser.endWidth = _laserWidth;
                _flash.localScale = Vector3.one * _flashSize;

                for (float t = 0f; t <= _duration; t += Time.deltaTime)
                {
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }

                for (float t = 0f; t <= _shrinkTime; t += Time.deltaTime)
                {
                    _laser.startWidth = Mathf.Lerp(_laserWidth, 0, t / _shrinkTime);
                    _laser.endWidth = Mathf.Lerp(_laserWidth, 0, t / _shrinkTime);
                    _flash.localScale = Vector3.one * Mathf.Lerp(_flashSize, 0, t / _shrinkTime);

                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException)
            {
                return;
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                enemy.MagicDamage(10, Owner);
            }
        }
    }
}