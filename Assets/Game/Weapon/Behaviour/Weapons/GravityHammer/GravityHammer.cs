using Lion.CameraUtility;
using Lion.Weapon.Behaviour.GravityHammerModules;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // グラビティハンマー (Gravity Hammer):
    // 地面に叩きつけることで周囲の敵を引き寄せ、次に大きな衝撃波で一掃する。
    // 引き寄せ効果で敵をまとめて攻撃できる。
    public class GravityHammer : WeaponBehaviour
    {
        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        [SerializeField]
        private Hammer _hammerPrefab;

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
            var position = Camera.main.GetRandomCameraArea();

            var _hammer = Instantiate(_hammerPrefab, position, transform.rotation);
            _hammer.Parameter = Parameter;
        }
    }
}