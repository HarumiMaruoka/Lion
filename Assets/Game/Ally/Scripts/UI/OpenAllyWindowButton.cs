using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Ally.UI
{
    [RequireComponent(typeof(Button))]
    public class OpenAllyWindowButton : MonoBehaviour
    {
        [SerializeField] private AllyWindow _allyWindow;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenAllyWindow);
        }

        private void OpenAllyWindow()
        {
            _allyWindow.Open(AllyManager.Instance.AllySheet);
        }
    }
}