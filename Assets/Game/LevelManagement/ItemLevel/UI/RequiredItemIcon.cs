using Lion.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI
{
    public class RequiredItemIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMPro.TextMeshProUGUI _haveCountText;
        [SerializeField] private TMPro.TextMeshProUGUI _needCountText;

        private int _itemID;
        private int _currentAmount;
        private int _requiredAmount;

        private ItemData FetchItemData(int itemID) => ItemManager.Instance.ItemSheet.GetItemData(itemID);

        public void Setup(int itemID, int requiredAmount)
        {
            _itemID = itemID;
            FetchItemData(_itemID).OnCountChanged += SetCurrentAmount;

            var itemData = FetchItemData(itemID);
            _icon.sprite = itemData.Icon;
            SetRequiredAmount(requiredAmount);
            SetCurrentAmount(itemData.Count);
        }

        private void OnDestroy()
        {
            FetchItemData(_itemID).OnCountChanged -= SetCurrentAmount;
        }

        public void SetRequiredAmount(int requiredAmount)
        {
            _requiredAmount = requiredAmount;
            _needCountText.text = $"Need: {requiredAmount}";
            gameObject.SetActive(requiredAmount > 0);
            UpdateTextColorBasedOnAmount();
        }

        private void SetCurrentAmount(int haveAmount)
        {
            _currentAmount = haveAmount;
            _haveCountText.text = $"Have: {haveAmount}";
            UpdateTextColorBasedOnAmount();
        }

        private void UpdateTextColorBasedOnAmount()
        {
            _haveCountText.color = _currentAmount < _requiredAmount ? Color.red : Color.green;
        }
    }
}