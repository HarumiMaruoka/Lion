using Lion.Item;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI.UpgradeWithItem
{
    public class ItemIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _itemIconView;
        [SerializeField]
        private TMPro.TextMeshProUGUI _label;

        private ItemData _itemData;
        private int _needCount;

        public void ApplyView(ItemData itemData, int needCount)
        {
            if (_itemData) _itemData.OnCountChanged -= OnCountChanged;
            _itemData = itemData;
            _itemData.OnCountChanged += OnCountChanged;

            _needCount = needCount;
            _itemIconView.sprite = itemData.Icon;

            OnCountChanged(_itemData.Count);
        }

        private void OnDestroy()
        {
            if (_itemData) _itemData.OnCountChanged -= OnCountChanged;
        }

        private void OnCountChanged(int haveCount)
        {
            if (haveCount >= _needCount)
            {
                _label.color = Color.green;
            }
            else
            {
                _label.color = Color.red;
            }

            _label.text =
                $"Have: {haveCount}\n" +
                $"Need: {_needCount}";
        }
    }
}