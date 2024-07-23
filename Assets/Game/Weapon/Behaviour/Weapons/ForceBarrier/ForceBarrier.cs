using Lion.Weapon.Behaviour.ForceBarrierModules;
using System;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // フォースバリア (Force Barrier):
    // プレイヤーの周囲にエネルギーフィールドを展開し、接触する敵を弾き飛ばす。
    // バリアが展開されている間は、プレイヤーは無敵状態になる。
    public class ForceBarrier : WeaponBehaviour
    {
        [SerializeField]
        private float _minWaitTime = 3f;
        [SerializeField]
        private float _maxWaitTime = 5f;

        [SerializeField]
        private Bullet _bulletPrefab;

        private Bullet _bullet;

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
            while (this.enabled)
            {
                yield return Fire();
                yield return Wait();
            }
        }

        private IEnumerator Fire()
        {
            if (_bullet) Destroy(_bullet.gameObject);

            _bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, transform);
            _bullet.Parameter = Parameter;

            while (_bullet != null)
            {
                yield return null;
            }
        }

        private IEnumerator Wait()
        {
            var waitDuration = _minWaitTime;
            if (Parameter != null) waitDuration = Mathf.Clamp(_maxWaitTime - Parameter.AttackSpeed / 100f, _minWaitTime, _maxWaitTime);

            for (float t = 0; t < waitDuration; t += Time.deltaTime)
            {
                yield return null;
            }
        }
    }
}