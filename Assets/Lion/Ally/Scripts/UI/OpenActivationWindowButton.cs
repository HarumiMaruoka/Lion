using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Ally.UI
{
    [RequireComponent(typeof(Button))]
    public class OpenActivationWindowButton : MonoBehaviour
    {
        [SerializeField]
        private AllyWindow _allyWindow;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OpenActivationWindow);

        }

        private void OpenActivationWindow()
        {
            _allyWindow.gameObject.SetActive(true);
            _allyWindow.OnSelected += OnSelected;
            _allyWindow.OnDisabled += OnWindowClosed;
        }

        private void OnSelected(AllyData selected)
        {
            AllyManager.Instance.ActivatedAlly = selected;
        }

        private void OnWindowClosed()
        {
            _allyWindow.OnSelected -= OnSelected;
            _allyWindow.OnDisabled -= OnWindowClosed;
        }
    }
}