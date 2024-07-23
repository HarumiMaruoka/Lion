using Lion.Weapon.Behaviour.AcidSprayModule;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class AcidSpray : WeaponBehaviour
    {
        [SerializeField]
        private float _minWaitTime = 0.5f;
        [SerializeField]
        private float _maxWaitTime = 1.8f;

        [SerializeField]
        private SprayEffect _sprayEffectPrefab;

        private SprayEffect _sprayEffect;

        private void OnEnable()
        {
            StartCoroutine(RunSequence());
        }

        private void OnDisable()
        {
            if (_sprayEffect) Destroy(_sprayEffect.gameObject);
        }

        private IEnumerator RunSequence()
        {
            while (this.enabled)
            {
                yield return Spray();
                yield return Wait();
            }
        }

        private IEnumerator Spray()
        {
            if (_sprayEffect) Destroy(_sprayEffect.gameObject);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0f, 360f)));

            _sprayEffect = Instantiate(_sprayEffectPrefab, transform.position, rotation);
            _sprayEffect.Parameter = Parameter;

            while (_sprayEffect != null)
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