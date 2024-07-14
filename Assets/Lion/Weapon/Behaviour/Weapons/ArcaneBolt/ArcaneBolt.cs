using Lion.Weapon.Behaviour.ArcaneBoltModule;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class ArcaneBolt : WeaponBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        [SerializeField]
        private float _minWaitTime = 0.5f;
        [SerializeField]
        private float _maxWaitTime = 1.5f;

        private Bullet _bullet;

        public float Speed => Parameter == null ? 10f : Parameter.AttackSpeed;

        private void OnEnable()
        {
            StartCoroutine(RunSequence());
        }

        private void OnDisable()
        {
            if (_bullet) Destroy(_bullet.gameObject);
        }

        private IEnumerator RunSequence()
        {
            while (enabled)
            {
                yield return Shoot();
                yield return Wait();
            }
        }

        private IEnumerator Shoot()
        {
            if (_bullet) Destroy(_bullet.gameObject);
            _bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            _bullet.Parameter = Parameter;

            // バレットが存在する間、次のフレームまで待機
            while (_bullet != null)
            {
                yield return null;
            }
        }

        private IEnumerator Wait()
        {
            var waitDuration = _minWaitTime;
            if (Parameter != null) waitDuration = CalculateAdjustedWaitTime();

            // 指定された時間だけ待機
            for (float t = 0; t < waitDuration; t += Time.deltaTime)
            {
                yield return null;
            }
        }

        private float CalculateAdjustedWaitTime()
        {
            return Mathf.Clamp(_maxWaitTime - Speed / 100f, _minWaitTime, _maxWaitTime);
        }
    }
}