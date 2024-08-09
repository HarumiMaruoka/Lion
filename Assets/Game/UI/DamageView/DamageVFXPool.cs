using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.UI
{
    public class DamageVFXPool : MonoBehaviour
    {
        [SerializeField]
        private DamageVFX _prefab;
        [SerializeField]
        private Transform _parent;

        private Stack<DamageVFX> _inactives = new Stack<DamageVFX>();

        public static DamageVFXPool Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) Debug.LogWarning("Instance already exists.");

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public DamageVFX Create(Vector3 worldPosition, float value)
        {
            // var canvasPosition = Camera.main.WorldToScreenPoint(worldPosition);
            var randomX = UnityEngine.Random.Range(0.3f, 0.3f);
            var randomY = UnityEngine.Random.Range(0.3f, 0.3f);
            var canvasPosition = worldPosition + new Vector3(randomX, randomY);

            DamageVFX vfx;

            if (_inactives.Count > 0)
            {
                vfx = _inactives.Pop();
                vfx.gameObject.SetActive(true);
                vfx.transform.position = canvasPosition;
            }
            else
            {
                vfx = Instantiate(_prefab, canvasPosition, Quaternion.identity, _parent);
            }

            vfx.Value = value;
            return vfx;
        }

        public void Destroy(DamageVFX vfx)
        {
            vfx.gameObject.SetActive(false);
            _inactives.Push(vfx);
        }
    }
}