using Lion.Minion;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    [RequireComponent(typeof(Button))]
    public class MinionGetButton : MonoBehaviour
    {
        [SerializeField] private int _count;

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            foreach (var minion in MinionManager.Instance.MinionSheet)
            {
                minion.Count += _count;
            }
        }
    }
}