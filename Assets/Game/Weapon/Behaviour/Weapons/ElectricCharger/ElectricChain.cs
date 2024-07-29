using Cysharp.Threading.Tasks;
using Lion.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.Behaviour.ElectricChargerModules
{
    public class ElectricChain : MonoBehaviour
    {
        // 既に攻撃した敵のリスト
        private HashSet<EnemyController> _alreadyAttackedEnemies = new HashSet<EnemyController>();

        public int MaxCount { get; set; } = 3;
        public float Range { get; set; } = 5f;

        public async UniTask Fire(EnemyController firstTarget)
        {
            // 現在の対象にダメージを与え、次の対象を探す。
            // 次の対象がいない場合は攻撃を終了する。
            // 次の対象がいる場合は、次の対象を設定し、攻撃を続ける。

            var currentTarget = firstTarget;

            for (int i = 0; i < MaxCount; i++)
            {
                if (currentTarget == null) break;

                currentTarget.MagicDamage(9f, null);
                _alreadyAttackedEnemies.Add(currentTarget);

                var nextTarget = SearchNearlestEnemy();
                if (nextTarget != null)
                {
                    CreateChainVFX(currentTarget.transform, nextTarget.transform);
                }
                currentTarget = nextTarget;

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            Destroy(gameObject);
        }

        [SerializeField]
        private SpriteRenderer _chainVFXPrefab;

        private void CreateChainVFX(Transform from, Transform to)
        {
            var center = (from.position + to.position) / 2f;
            var angle = Vector2.SignedAngle(Vector2.up, to.position - from.position);

            var vfx = Instantiate(_chainVFXPrefab, center, Quaternion.Euler(0, 0, angle));

            var distance = Vector2.Distance(from.position, to.position);
            vfx.size = new Vector2(vfx.size.x, distance);

            Destroy(vfx.gameObject, 0.1f);
        }

        private Collider2D[] _nears = new Collider2D[10];

        private EnemyController SearchNearlestEnemy()
        {
            var nearCount = Physics2D.OverlapCircleNonAlloc(transform.position, Range, _nears);

            float minDistance = float.MaxValue;
            EnemyController near = null;

            for (int i = 0; i < nearCount; i++)
            {
                var n = _nears[i];
                if (EnemyManager.TryGetEnemy(n.gameObject, out var enemy))
                {
                    if (_alreadyAttackedEnemies.Contains(enemy)) continue;

                    var distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        near = enemy;
                    }
                }
            }

            return near;
        }
    }
}