using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.MetallicSwarmModules
{
    public class Missile : MonoBehaviour
    {
        private Transform _target;

        private float _searchRange = 10f;

        [SerializeField]
        private float _initialSpeed = 4f;

        [SerializeField]
        private float _period = 1.2f; // 命中までの時間

        private Vector3 _velocity;
        public float MaxAccelThreshold => 30f;

        public IWeaponParameter Parameter { get; set; }

        private Vector3 position;

        private void Start()
        {
            position = transform.position;

            var enemies = Physics2D.OverlapCircleAll(transform.position, _searchRange, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemies.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, enemies.Length);
                _target = enemies[index].transform;
            }

            var angle = UnityEngine.Random.Range(0, Mathf.PI * 2f);
            _velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _initialSpeed;

            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg);
        }

        private void Update()
        {
            if (_period < 0)
            {
                Fire();
                return;
            }

            _period -= Time.deltaTime;

            var acceleration = Vector3.zero;

            Vector3 diff;
            if (_target) diff = _target.position - position;
            else diff = Vector3.zero;

            acceleration += (diff - _velocity * _period) * 2 / (_period * _period);

            if (acceleration.sqrMagnitude > MaxAccelThreshold * MaxAccelThreshold)
            {
                acceleration = acceleration.normalized * MaxAccelThreshold;
            }

            _velocity += acceleration * Time.deltaTime;
            position += _velocity * Time.deltaTime;

            transform.position = position;

            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg);
        }

        [SerializeField]
        private MissileExplosion _explosionPrefab;

        protected virtual void Fire()
        {
            var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.Parameter = Parameter;
            Destroy(gameObject);
        }
    }
}