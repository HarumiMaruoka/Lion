using System;
using UnityEngine;

namespace Lion.Enemy
{
    // 敵を氷結状態にする。
    public class Frozen : MonoBehaviour
    {
        public EnemyController Enemy { get; set; }
        public float FrozenTime { get; set; } = 5f;
        private float _frozenTimer;

        // 凍結エフェクト
        private static GameObject _frozenEffectPrefab;
        private GameObject _currentFrozenEffect;

        private void Start()
        {
            if (!_frozenEffectPrefab) _frozenEffectPrefab = Resources.Load<GameObject>("FrozenEffect");

            Freeze();
            Enemy.OnDamaged += OnDamageTakened;
            Enemy.OnDead += OnEnemyDead;
        }

        private void Update()
        {
            _frozenTimer -= Time.deltaTime;
            if (_frozenTimer <= 0)
            {
                Destroy(this);
            }
        }

        public event Action<EnemyController> OnDamaged;

        private void OnDestroy()
        {
            Enemy.OnDamaged -= OnDamageTakened;
            Enemy.OnDead -= OnEnemyDead;

            Unfreeze();
        }

        private void OnDamageTakened() // 敵のダメージを受けた時の処理
        {
            OnDamaged?.Invoke(Enemy);
            Destroy(this);
        }

        private void OnEnemyDead()
        {
            Destroy(this);
        }

        // 敵を凍結させる
        public void Freeze()
        {
            _frozenTimer = FrozenTime;

            // 凍結エフェクトを表示
            if (_frozenEffectPrefab)
            {
                _currentFrozenEffect = Instantiate(_frozenEffectPrefab, transform.position, Quaternion.identity, transform);
            }

            // ここで敵の動きを停止する処理を追加
            if (!Enemy.IsFreezed) Enemy.SpriteRenderer.color = new Color(0.5f, 0.5f, 1f, 1f);
            Enemy.Freeze();
        }

        // 凍結状態を解除する
        public void Unfreeze()
        {
            // 凍結エフェクトを削除
            if (_currentFrozenEffect)
            {
                Destroy(_currentFrozenEffect.gameObject);
            }

            // ここで敵の動きを再開する処理を追加
            Enemy.Unfreeze();
            if (!Enemy.IsFreezed) Enemy.SpriteRenderer.color = Color.white;
        }
    }
}