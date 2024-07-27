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

        private float DaggerSpeed => 8f;

        private VirtualJoystick Joystick => VirtualJoystick.Instance;

        private float _timer;
        private Vector3 _lastPos;

        private void OnEnable()
        {
            _lastPos = transform.position - Vector3.left;
            _timer = WaitTime;
        }

        private void Update()
        {
            // 向きを更新する。
            var currentPos = transform.position;
            if (Vector3.SqrMagnitude(currentPos - _lastPos) > 0.01f)
            {
                transform.right = (currentPos - _lastPos).normalized;
                _lastPos = currentPos;
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