using Lion.Weapon.Behaviour.MetallicSwarmModules;
using System;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // メタリックスウォーム (Metallic Swarm):
    // 金属の小型ドローン群を放つ。
    // ドローンは誘導ミサイルを発射したのち、自爆する。
    public class MetallicSwarm : WeaponBehaviour
    {
        [SerializeField]
        private float _minWaitTime = 0.5f;
        [SerializeField]
        private float _maxWaitTime = 1.8f;

        [SerializeField]
        private Drone _droneaPrefab;

        private Drone _drone;

        private void OnEnable()
        {
            StartCoroutine(RunSequence());
        }

        private void OnDisable()
        {
            if (_drone) Destroy(_drone.gameObject);
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
            if (_drone) Destroy(_drone.gameObject);

            _drone = Instantiate(_droneaPrefab, transform.position, Quaternion.identity, transform);
            _drone.Parameter = Parameter;

            while (_drone != null)
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