using Lion.Enemy;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour.FreezeCannonModules
{
    public class FirstBullet : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SecondBullet _secondBulletPrefab;

        private bool _isHit = false;
        private float _lifeTime = 2f;
        private Vector2 _velocity;

        public IWeaponParameter Parameter { get; set; }

        private float Speed => Parameter == null ? 10f : 8f + Parameter.AttackSpeed / 100f;

        private void Start()
        {
            StartCoroutine(RunSequence());
        }

        public void Initialize(Vector2 direction)
        {
            if (direction == Vector2.zero) direction = Vector2.left;
            _velocity = direction * Speed;

            transform.position += (Vector3)direction * 1f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private IEnumerator RunSequence()
        {
            yield return GenerateState();
            yield return ShootState();
            yield return HitState();
        }

        private IEnumerator GenerateState()
        {
            _collider2D.enabled = false;
            _animator.Play("Ice_Shard_Start");
            _rigidbody2D.velocity = Vector2.zero;

            yield return null;
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
        }

        private IEnumerator ShootState()
        {
            _collider2D.enabled = true;
            _animator.Play("Ice_Shard_Static");
            _rigidbody2D.velocity = _velocity;

            yield return null;
            for (float t = 0; t < _lifeTime; t += Time.deltaTime)
            {
                if (_isHit) yield break;

                yield return null;
            }
        }

        private IEnumerator HitState()
        {
            _collider2D.enabled = false;
            _animator.Play("Ice_Shard_End");
            _rigidbody2D.velocity = Vector2.zero;

            yield return null;
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                _isHit = true;
                var frozen = enemy.gameObject.AddComponent<Frozen>();
                frozen.Enemy = enemy;

                frozen.OnDamaged += CreateSecondBullet;
            }
        }

        private void CreateSecondBullet(EnemyController enemy)
        {
            var secondBullet = Instantiate(_secondBulletPrefab, enemy.transform.position, Quaternion.identity);
            secondBullet.Parameter = Parameter;
        }
    }
}