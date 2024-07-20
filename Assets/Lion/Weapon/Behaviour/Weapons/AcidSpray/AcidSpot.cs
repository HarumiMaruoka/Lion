using Lion.Damage;
using Lion.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.Behaviour.AcidSprayModule
{
    public class AcidSpot : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private float _damageInterval = 0.5f;

        private IWeaponParameter _weaponParameter;

        public float DamageInterval => _damageInterval; // 継続ダメージ状態がダメージを与える間隔
        public float DamageDuration => _weaponParameter == null ? 10f : _weaponParameter.Duration * 0.8f; // 継続ダメージの持続時間
        public float LifeTime => _weaponParameter == null ? 2.5f : _weaponParameter.Duration * 0.6f; // 酸の持続時間
        public float MagicAttackPower => _weaponParameter == null ? 1f : _weaponParameter.MagicPower; // 魔法攻撃力

        private Coroutine _fadeIn = null;
        private Coroutine _fadeOut = null;

        private float _elapsed = 0f;

        public void Initialize(IWeaponParameter parameter)
        {
            _weaponParameter = parameter;
        }

        private void Start()
        {
            var col = _spriteRenderer.color;
            _spriteRenderer.color = new Color(col.r, col.g, col.b, 0f);
            _fadeIn = StartCoroutine(_spriteRenderer.FadeIn(0.5f));
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;

            if (_fadeOut == null && LifeTime - _elapsed < 0.5f)
            {
                StopCoroutine(_fadeIn);
                _fadeOut = StartCoroutine(_spriteRenderer.FadeOut(0.5f));
            }

            if (_elapsed >= LifeTime)
            {
                Destroy(gameObject);
                OnDestroyed?.Invoke();
            }
        }

        public event Action OnDestroyed;

        [SerializeField]
        private ParticleSystem _damageVFXPrefab;

        private Dictionary<GameObject, ContinualMagicDamage> _continualDamageRegistry = new Dictionary<GameObject, ContinualMagicDamage>();

        public void OnTriggerEnter2D(Collider2D collision)
        {
            // 継続ダメージ状態を付与する。
            if (EnemyManager.Instance.EnemyPool.TryGetEnemy(collision.gameObject, out var enemy))
            {
                // 既に継続ダメージ状態を持っている場合は何もしない。
                if (_continualDamageRegistry.ContainsKey(enemy.gameObject)) return;

                var continualDamage = enemy.gameObject.AddComponent<ContinualMagicDamage>();
                continualDamage.Initialzie(_weaponParameter?.Actor, enemy, DamageInterval, DamageDuration, MagicAttackPower);
                _continualDamageRegistry.Add(enemy.gameObject, continualDamage);

                if (_damageVFXPrefab != null)
                {
                    var vfx = Instantiate(_damageVFXPrefab, enemy.transform);
                    continualDamage.StateVFX = vfx;
                }
            }
        }
    }
}