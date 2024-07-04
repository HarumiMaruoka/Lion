using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    [RequireComponent(typeof(Toggle))]
    public class DebugModeToggle : MonoBehaviour
    {
        private Toggle _toggle;
        private Toggle Toggle => _toggle ??= GetComponent<Toggle>();

        [SerializeField]
        private GameObject[] _debugObjects;
        [SerializeField]
        private MonoBehaviour[] _debugScripts;

        private void Awake()
        {
            Toggle.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(_toggle.isOn);
        }

        public event Action<bool> DebugModeChanged;
        public bool IsDebugMode => Toggle.isOn;

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