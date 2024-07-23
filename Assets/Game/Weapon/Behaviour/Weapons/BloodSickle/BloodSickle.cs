using Lion.Weapon.Behaviour.BloodSickleModules;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class BloodSickle : WeaponBehaviour
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
            var bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.Parameter = Parameter;
        }
    }
}