using Lion.Damage;
using Lion.Enemy;
using UnityEngine;

namespace Lion.Weapon.Behaviour.EternalFlameModules
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _damageInterval = 0.5f;
        [SerializeField]
        private ParticleSystem _damageVFXPrefab;

        public IWeaponParameter Parameter { get; set; }
        public EternalFlame EternalFlame { get; set; }
        public Transform Center { get; set; } // 中心座標
        public float Speed { get; set; } // 速度
        public float Elapsed { get; set; } // 経過時間
        public float Radius { get; set; } // 半径
        public float Count { get; set; } // 生成数
        public float MaxCount { get; set; } // 最大生成数

        public float LifeTime => 5f;
        public float DamageDuration => Parameter == null ? 10f : Parameter.Duration * 0.8f; // 継続ダメージの持続時間
        private float MagicPower => Parameter == null ? 12f : Parameter.MagicPower;
        private float AngleOffset => ((Mathf.PI * 2) / MaxCount) * Count; // 角度

        private Vector2 Direction => new Vector2(Mathf.Cos((Elapsed * Speed) + AngleOffset), Mathf.Sin((Elapsed * Speed) + AngleOffset)); // 方向
        private Vector2 Position => new Vector2(Center.position.x, Center.position.y) + Direction * Radius; // 位置

        private void Start()
        {
            transform.position = Position;
        }

        private void Update()
        {
            Elapsed += Time.deltaTime;
            transform.position = Position;
            if (Elapsed >= LifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            EternalFlame.Bullets.Remove(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                var continualDamage = enemy.gameObject.AddComponent<ContinualMagicDamage>();
                continualDamage.Initialzie(Parameter?.Actor, enemy, _damageInterval, DamageDuration, MagicPower);

                if (_damageVFXPrefab != null)
                {
                    var vfx = Instantiate(_damageVFXPrefab, collision.transform.position, Quaternion.identity, enemy.transform);
                    continualDamage.StateVFX = vfx;
                }

                Destroy(gameObject);
            }
        }
    }
}