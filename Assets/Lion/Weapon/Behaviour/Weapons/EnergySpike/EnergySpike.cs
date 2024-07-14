using Lion.Weapon.Behaviour.EnergySpikeModules;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class EnergySpike : WeaponBehaviour
    {
        [SerializeField]
        private Bullet _projectilePrefab;

        private float MinInterval => 0.5f;
        private float MaxInterval => 2;

        private float AttackSpeed => Parameter == null ? 1f : Parameter.AttackSpeed;
        private float WaitTime => Mathf.Clamp(MaxInterval - AttackSpeed * 0.01f, MinInterval, MaxInterval);

        private float _timer;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Fire();
                _timer = WaitTime; // 次の発射までの時間をリセット
            }
        }

        private void Fire()
        {
            var spike = Instantiate(_projectilePrefab, transform.position, transform.rotation);

            var angle = UnityEngine.Random.Range(0f, 360f);
            var scale = spike.transform.localScale;
            var xScale = angle > 90 && angle < 270 ? -scale.x : scale.x;
            angle -= angle > 90 && angle < 270 ? 180 : 0;

            spike.transform.rotation = Quaternion.Euler(0, 0, angle);
            spike.transform.localScale = new Vector3(xScale, scale.y, scale.z);

            foreach (var collider in spike.Colliders)
            {
                collider.Parameter = Parameter;
            }
        }
    }
}