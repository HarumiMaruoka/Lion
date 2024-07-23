using Lion.Weapon.Behaviour.LifeReaperModules;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // ライフリーパー (Life Reaper):
    // 巨大な鎌を振り回し、前方の広範囲に渡って敵を薙ぎ払う。
    // 敵を倒すと少量の体力を吸収し、プレイヤーの体力を回復する。
    public class LifeReaper : WeaponBehaviour
    {
        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        [SerializeField]
        private Bullet _bulletPrefab;

        private float AttackSpeed => Parameter == null ? 1f : Parameter.AttackSpeed;
        private float WaitTime => Mathf.Clamp(_maxFireInterval - AttackSpeed * 0.01f, _minFireInterval, _maxFireInterval);

        private float _timer;

        private void OnEnable()
        {
            _timer = WaitTime;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Fire();
                _timer = WaitTime;
            }
        }

        private void Fire()
        {
            var angle = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
            var distance = 0f;
            var position = transform.position + angle * Vector3.right * distance;

            var isFlip = UnityEngine.Random.Range(0, 2) == 0;

            var instance = Instantiate(_bulletPrefab, position, angle);
            instance.Parameter = Parameter;

            var scale = instance.transform.localScale;
            scale.x = isFlip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            instance.transform.localScale = scale;
        }
    }
}