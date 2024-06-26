using Lion.Item;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    [RequireComponent(typeof(Button))]
    public class ItemGetButton : MonoBehaviour
    {
        [SerializeField]
        private int _count;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            foreach (var item in ItemManager.Instance.ItemSheet)
            {
                item.Count += _count;
            }
        }
    }
}