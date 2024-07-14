using Lion.Weapon.Behaviour.EternalFlameModules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class EternalFlame : WeaponBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        [SerializeField]
        private float _minWaitTime = 0.8f;
        [SerializeField]
        private float _maxWaitTime = 3.0f;

        private float AttackSpeed => Parameter == null ? 3f : Parameter.AttackSpeed;
        private float RotateSpeed => 2.5f + AttackSpeed * 0.01f;
        private float WaitTime => Mathf.Clamp(_maxWaitTime - AttackSpeed * 0.01f, _minWaitTime, _maxWaitTime);

        [SerializeField]
        private int _count = 3;

        private HashSet<Bullet> _bullets = new HashSet<Bullet>();
        public HashSet<Bullet> Bullets => _bullets;

        private void OnEnable()
        {
            StartCoroutine(RunSequence());
        }

        private void OnDisable()
        {
            DestroyAllBullets();
            StopAllCoroutines();
        }

        private IEnumerator RunSequence()
        {
            while (enabled)
            {
                yield return StartCoroutine(Fire());
                yield return StartCoroutine(Wait());
            }
        }

        private IEnumerator Fire()
        {
            DestroyAllBullets();

            float elapsed = 0f;
            float interval = 0.1f;
            float radius = 2.5f;
            float speed = RotateSpeed;
            int count = _count;

            for (int i = 0; i < count; i++)
            {
                var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                bullet.transform.SetParent(transform);

                bullet.EternalFlame = this;
                bullet.Center = transform;
                bullet.Elapsed = elapsed;
                bullet.Radius = radius;
                bullet.Speed = speed;
                bullet.Count = i;
                bullet.MaxCount = count;
                bullet.Parameter = Parameter;

                _bullets.Add(bullet);

                // インターバル待機
                for (float t = 0; t < interval; t += Time.deltaTime)
                {
                    yield return null;
                    elapsed += Time.deltaTime;
                }
            }

            // すべての弾が消えるまで待機
            while (_bullets.Count > 0)
            {
                yield return null;
            }
        }

        private IEnumerator Wait()
        {
            var waitTime = WaitTime;
            for (float t = 0; t < waitTime; t += Time.deltaTime)
            {
                yield return null;
            }
        }

        private void DestroyAllBullets()
        {
            foreach (var bullet in _bullets)
            {
                if (bullet)
                {
                    Destroy(bullet.gameObject);
                }
            }
            _bullets.Clear();
        }
    }
}