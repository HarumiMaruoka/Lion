using Cysharp.Threading.Tasks;
using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.ElectricChargerModules
{
    public class ElectricField : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        private float MagicPower => Parameter == null ? 9f : Parameter.MagicPower;

        [SerializeField] private float _minSize = 0.5f;
        [SerializeField] private float _maxSize = 5f;

        private float _elapsed = 0f;
        private float _currentSize = 0f;

        private float _minScaleSpeed = 0.2f;
        private float _maxScaleSpeed = 1f;

        private float AttackSpeed => Parameter == null ? 1f : Parameter.AttackSpeed;
        private float CurrentScaleSpeed => Mathf.Clamp(_minScaleSpeed + AttackSpeed / 100f, _minScaleSpeed, _maxScaleSpeed);

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
                _currentSize += CurrentScaleSpeed * Time.deltaTime;
                transform.localScale = new Vector3(_currentSize, _currentSize, 1f);
            }
        }

        [SerializeField]
        private ElectricChain _chainPrefab;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Collider2D _collider;

        private async void OnTriggerEnter2D(Collider2D other)
        {
            if (isHit) return;

            if (EnemyManager.TryGetEnemy(other.gameObject, out var enemy))
            {
                if (enemy == null) return;

                var chain = Instantiate(_chainPrefab, transform.position, transform.rotation);
                chain.MaxCount = 10;
                chain.Range = 5f;

                isHit = true;

                chain.Fire(enemy).Forget();
                Destroy(gameObject);
            }
        }
    }
}