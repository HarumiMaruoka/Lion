using Lion.Weapon.Behaviour.MirageDaggerModules;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // ミラージュダガー (Mirage Dagger):
    // 幻影の短剣を投げ、敵に当たると分身して複数の敵に攻撃する。
    // 短剣は敵を追尾し、高速で飛び交う。
    public class MirageDagger : WeaponBehaviour
    {
        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        [SerializeField]
        private Dagger _daggerPrefab;
        [SerializeField]
        private Transform _spawnPoint;

        private float AttackSpeed => Parameter == null ? 1f : Parameter.AttackSpeed;
        private float WaitTime => Mathf.Clamp(_maxFireInterval - AttackSpeed * 0.01f, _minFireInterval, _maxFireInterval);

        private float DaggerSpeed => Parameter == null ? 8f : Parameter.AttackSpeed;

        [SerializeField]
        private VirtualJoystick _joystick;

        private float _timer;

        private void OnEnable()
        {
            _timer = WaitTime;
        }

        private void Update()
        {
            // 向きを更新する。
            Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (_joystick.IsDragging) moveDir += _joystick.Vector;

            if (moveDir != Vector2.zero)
            {
                transform.right = moveDir.normalized;
            }


            // 攻撃用のタイマーを更新する。タイマーが0になったら攻撃する。
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Fire();
                _timer = WaitTime;
            }
        }

        private void Fire()
        {
            var dagger = Instantiate(_daggerPrefab, _spawnPoint.position, transform.rotation);
            dagger.Velocity = transform.right * DaggerSpeed;
            dagger.Parameter = Parameter;
        }
    }
}