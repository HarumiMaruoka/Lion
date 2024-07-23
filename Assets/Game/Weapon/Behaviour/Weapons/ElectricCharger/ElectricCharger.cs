using Lion.Weapon.Behaviour.ElectricChargerModules;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    public class ElectricCharger : WeaponBehaviour
    {
        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        [SerializeField]
        private ElectricField _electricFieldPrefab;

        private ElectricField _electricField;

        private float AttackSpeed => Parameter == null ? 1f : Parameter.AttackSpeed;
        private float WaitTime => Mathf.Clamp(_maxFireInterval - AttackSpeed * 0.01f, _minFireInterval, _maxFireInterval);

        private void OnEnable()
        {
            StartCoroutine(RunSequence());
        }

        private void OnDisable()
        {
            if (_electricField) Destroy(_electricField.gameObject);
        }

        private IEnumerator RunSequence()
        {
            while (enabled)
            {
                yield return Fire();
                yield return Wait();
            }
        }

        private IEnumerator Fire()
        {
            if (_electricField) Destroy(_electricField.gameObject);
            _electricField = Instantiate(_electricFieldPrefab, transform.position, transform.rotation, transform);
            _electricField.Parameter = Parameter;

            while (_electricField)
            {
                yield return null;
            }
        }

        private IEnumerator Wait()
        {
            var w = WaitTime;
            for (float t = 0f; t < w; t += Time.deltaTime)
            {
                yield return null;
            }
        }
    }
}