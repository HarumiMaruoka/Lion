using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    [RequireComponent(typeof(Toggle))]
    public class DebugModeToggle : MonoBehaviour
    {
        private Toggle _toggle;

        [SerializeField]
        private GameObject[] _debugObjects;
        [SerializeField]
        private MonoBehaviour[] _debugScripts;

        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(_toggle.isOn);
        }

        public event Action<bool> DebugModeChanged;
        public bool IsDebugMode => _toggle.isOn;

        private void OnValueChanged(bool isOn)
        {
            DebugModeChanged?.Invoke(isOn);
            foreach (var debugObject in _debugObjects)
            {
                debugObject.SetActive(isOn);
            }
            foreach (var debugScript in _debugScripts)
            {
                debugScript.enabled = isOn;
            }
        }
    }
}