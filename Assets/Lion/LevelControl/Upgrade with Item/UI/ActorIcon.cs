using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI.UpgradeWithItem
{
    [RequireComponent(typeof(Button))]
    public class ActorIcon : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _label;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private GameObject _activatedLabel;

        private void Start()
        {
            var button = GetComponent<Button>();
            if (button) button.onClick.AddListener(() => OnClicked?.Invoke(Item));
            else Debug.LogWarning("Button is not found.");
        }

        public IItemLevelUpable Item { get; private set; }

        public event Action<IItemLevelUpable> OnClicked;

        public void SetItem(IItemLevelUpable itemLevelUpable)
        {
            if (Item != null)
            {
                Item.OnLevelUp -= OnLevelUp;
                Item.OnActiveChanged -= OnActiveChanged;
            }

            Item = itemLevelUpable;
            UpdateLabel();

            Item.OnLevelUp += OnLevelUp;
            Item.OnActiveChanged += OnActiveChanged;

            _icon.sprite = Item.IconSprite;
            _activatedLabel.gameObject.SetActive(Item.IsActive);
        }

        private void OnDestroy()
        {
            if (Item != null)
            {
                Item.OnLevelUp -= OnLevelUp;
                Item.OnActiveChanged -= OnActiveChanged;
            }
        }

        private void OnLevelUp(int level)
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            _label.text = $"Lv: {Item.Level}/{Item.MaxLevel}";
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.gameObject.SetActive(isActive);
        }
    }
}