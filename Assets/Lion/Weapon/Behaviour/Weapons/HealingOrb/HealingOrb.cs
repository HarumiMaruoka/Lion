using System;
using Lion.CameraUtility;
using Lion.Weapon.Behaviour.HealingOrbModules;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // ヒーリングオーブ (Healing Orb):
    // 一定時間ごとにランダムな座標に生成されるオーブ。
    // オーブは周囲を照らし、エリア内の味方を回復する。
    public class HealingOrb : WeaponBehaviour
    {
        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        [SerializeField]
        private Orb _orbPrefab;

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
            var top = Camera.main.GetWorldTopRight().y;
            var left = Camera.main.GetWorldBottomLeft().x;
            var right = Camera.main.GetWorldTopRight().x;

            var x = UnityEngine.Random.Range(left, right);
            var y = top;

            var position = new Vector3(x, y, 0);

            var bullet = Instantiate(_orbPrefab, position, Quaternion.identity);
            bullet.Parameter = Parameter;
        }
    }
}