using Lion.Ally;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    [RequireComponent(typeof(Button))]
    public class AllyGetButton : MonoBehaviour
    {
        [SerializeField] private int _count;

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            foreach (var ally in AllyManager.Instance.AllySheet)
            {
                ally.Count += _count;
            }
        }
    }
}