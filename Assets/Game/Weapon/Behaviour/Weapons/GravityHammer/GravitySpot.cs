using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.GravityHammerModules
{
    public class GravitySpot : MonoBehaviour
    {
        [SerializeField]
        private Explosion _explosionPrefab;

        public IWeaponParameter Parameter { get; set; }

        private float MagicPower => Parameter == null ? 1f : Parameter.MagicPower;
        private float GravityPower => 0.05f + MagicPower / 10000f;

        private float _timer;
        private float Lifetime => Parameter == null ? 3f : 3f + Parameter.Duration;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= Lifetime)
            {
                var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                explosion.Parameter = Parameter;
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                var direction = (enemy.transform.position - transform.position).normalized;
                var force = direction * GravityPower;
                enemy.Rigidbody2D.MovePosition(enemy.transform.position - force);
            }
        }
    }
}