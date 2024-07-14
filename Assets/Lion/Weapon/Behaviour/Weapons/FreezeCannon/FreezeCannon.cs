using System;
using UnityEngine;
using Lion.Weapon.Behaviour.FreezeCannonModules;
using Lion.Player;

namespace Lion.Weapon.Behaviour
{
    // フリーズキャノン (Freeze Cannon):
    // 敵に当たると凍結させる氷の弾を発射する。
    // 凍った敵は動けなくなり、次の攻撃で砕けると周囲に氷片が飛び散り、さらなるダメージを与える。
    public class FreezeCannon : WeaponBehaviour
    {
        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        [SerializeField]
        private FirstBullet _firstBulletPrefab;

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
            var bullet = Instantiate(_firstBulletPrefab, transform.position, transform.rotation);
            bullet.Parameter = Parameter;

            Vector3 direction = PlayerController.Instance.Direction;
            bullet.Initialize(direction);
        }
    }
}