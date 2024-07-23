using Lion.Enemy;
using Lion.Player;
using System;
using UnityEngine;


namespace Lion.Weapon.Behaviour.ArcaneBoltModule
{
    public class Bullet : Missile
    {
        public IWeaponParameter Parameter { get; set; }
        public float Speed => Parameter == null ? 10f : Parameter.AttackSpeed;

        [SerializeField] private float _minTime = 0.6f;
        [SerializeField] private float _maxTime = 3f;
        [SerializeField] private Explosion _explosionPrefab;

        private Collider2D[] _nearCollider = new Collider2D[100];

        protected override void Start()
        {
            base.Start();
            // ターゲットを設定
            Target = SearchNearlestEnemy()?.transform;
            // 命中までの時間を設定
            var randomOffset = UnityEngine.Random.Range(-0.5f, 0.5f);
            Period = Mathf.Clamp(_maxTime - Speed / 100f + randomOffset, _minTime, _maxTime);
            // 最大加速度を設定
            MaxAccelThreshold = 10f;

            // ランダムな初速を設定
            var angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            var length = UnityEngine.Random.Range(1f, 5f);

            float x = Mathf.Cos(angle) * length;
            float y = Mathf.Sin(angle) * length;

            Velocity = new Vector3(x, y, 0);
        }

        public EnemyController SearchNearlestEnemy()
        {
            float searchRadius = 10f; // 検索範囲の半径
            EnemyController nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            // 現在の位置からsearchRadius内のコライダーを取得
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, searchRadius, _nearCollider);
            for (int i = 0; i < count; i++)
            {
                var hitCollider = _nearCollider[i];
                if (EnemyManager.TryGetEnemy(hitCollider.gameObject, out var enemy))
                {
                    float distance = Vector3.SqrMagnitude(transform.position - enemy.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }

            return nearestEnemy;
        }

        protected override void Fire()
        {
            // 爆発エフェクトを生成
            var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.Parameter = Parameter;

            Destroy(gameObject);
        }
    }
}