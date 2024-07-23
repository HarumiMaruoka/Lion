using Lion.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.Behaviour.MirageDaggerModules
{
    public class Dagger : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        public int MaxHitCount { get; private set; } = 3;
        public int HitCount { get; private set; } = 0;
        public HashSet<EnemyController> Hits { get; private set; } = new HashSet<EnemyController>();

        [SerializeField]
        private float _lifeTime = 3f;

        [SerializeField]
        private Dagger _daggerPrefab;
        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        private float _lifeTimer = 0f;

        private float PhysicalPower => Parameter == null ? 3f : Parameter.PhysicalPower;

        public Vector2 Velocity
        {
            get => _rigidbody2D.velocity;
            set => _rigidbody2D.velocity = value;
        }

        private void Update()
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= _lifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                if (!Hits.Add(enemy)) return;

                HitCount++;
                enemy.PhysicalDamage(PhysicalPower, Parameter?.Actor);
                if (HitCount < MaxHitCount)
                {
                    // 分身の片方は自身の進行方向に対して30度傾ける。
                    // もう片方は逆方向に30度傾ける。
                    var left = Quaternion.Euler(0, 0, 30) * Velocity;
                    var right = Quaternion.Euler(0, 0, -30) * Velocity;
                    CreateDagger(left);
                    CreateDagger(right);

                    Destroy(gameObject);
                }
            }
        }

        private void CreateDagger(Vector2 velocity)
        {
            var rot = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
            var dagger = Instantiate(_daggerPrefab, transform.position, rot);
            dagger.MaxHitCount = MaxHitCount;
            dagger.HitCount = HitCount;
            dagger.Hits = Hits;
            dagger.Parameter = Parameter;

            dagger.Velocity = velocity;
        }
    }
}