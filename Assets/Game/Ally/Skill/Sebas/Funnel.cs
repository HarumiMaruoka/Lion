using Lion.Enemy;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Ally.Skill
{
    public class Funnel : MonoBehaviour
    {
        private IActor _owner;
        public IActor Owner
        {
            get { return _owner != null ? _owner : PlayerController.Instance; }
            set { _owner = value; _move.Center = _owner.transform; }
        }

        private float MagicPower => Owner == null ? 3 : Owner.MagicPower;

        [SerializeField]
        private FunnelMovement _move;
        [SerializeField]
        private float _radius = 3f;
        [SerializeField]
        private float _coolTime = 1.0f;
        [SerializeField]
        private float _lifeTime = 10f;
        [SerializeField]
        private FunnelBeamVFX _vfxPrefab;

        private float _coolTimer = 0.0f;
        private float _lifeTimer = 0.0f;

        private void Update()
        {
            _coolTimer += Time.deltaTime;
            _lifeTimer += Time.deltaTime;

            if (_coolTimer > _coolTime) Fire();
            if (_lifeTimer > _lifeTime) Die();
        }

        private void Fire()
        {
            _coolTimer = UnityEngine.Random.Range(-0.1f, 0.1f);
            var enemy = SearchEnemy();

            if (!enemy) return;

            enemy.MagicDamage(MagicPower, Owner);
            var vfx = Instantiate(_vfxPrefab);
            vfx.Initialize(transform, enemy.transform.position);
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private EnemyController SearchEnemy()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            for (int i = 0; i < colliders.Length; i++)
            {
                var coll = colliders[i];
                if (EnemyManager.TryGetEnemy(coll.gameObject, out var enemy))
                {
                    return enemy;
                }
            }
            return null;
        }
    }
}