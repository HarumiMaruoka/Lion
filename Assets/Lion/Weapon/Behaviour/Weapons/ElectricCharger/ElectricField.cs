using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.ElectricChargerModules
{
    public class ElectricField : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        private float MagicPower => Parameter == null ? 9f : Parameter.MagicPower;

        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;

        private float _elapsed = 0f;
        private float _currentSize = 0f;

        private float _scaleSpeed = 1f;

        private void Start()
        {
            _currentSize = _minSize;
        }

        private bool isHit = false;

        private void Update()
        {
            if (isHit) return;

            _elapsed += Time.deltaTime;

            if (_currentSize < _maxSize)
            {
                _currentSize += _scaleSpeed * Time.deltaTime;
                transform.localScale = new Vector3(_currentSize, _currentSize, 1f);
            }
        }

        [SerializeField]
        private ElectricChain _chainPrefab;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private async void OnTriggerEnter2D(Collider2D other)
        {
            if (EnemyManager.TryGetEnemy(other.gameObject, out var enemy))
            {
                if (enemy == null) return;

                enemy.MagicDamage(MagicPower, null);

                var chain = Instantiate(_chainPrefab, transform.position, transform.rotation);
                chain.MaxCount = 3;
                chain.Range = 5f;

                _animator.enabled = false;
                _spriteRenderer.enabled = false;

                isHit = true;

                await chain.Fire(enemy);
                Destroy(gameObject);
            }
        }
    }
}