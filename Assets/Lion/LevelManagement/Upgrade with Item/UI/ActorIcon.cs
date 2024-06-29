using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI.UpgradeWithItem
{
    [RequireComponent(typeof(Button))]
    public class ActorIcon : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _levelLabel;
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

        public IItemLevelable Item { get; private set; }

        public event Action<IItemLevelable> OnClicked;

        public void SetItem(IItemLevelable itemLevelUpable)
        {
            if (Item != null)
            {
                Item.ItemLevelManager.OnLevelChanged -= OnLevelUp;
                Item.OnActiveChanged -= OnActiveChanged;
            }

            Item = itemLevelUpable;
            UpdateLabel();

            Item.ItemLevelManager.OnLevelChanged += OnLevelUp;
            Item.OnActiveChanged += OnActiveChanged;

            _icon.sprite = Item.IconSprite;
            _activatedLabel.gameObject.SetActive(Item.IsActive);
        }

        private void OnDestroy()
        {
            if (Item != null)
            {
                Item.ItemLevelManager.OnLevelChanged -= OnLevelUp;
                Item.OnActiveChanged -= OnActiveChanged;
            }
        }

        private void OnLevelUp(int level)
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            _levelLabel.text = $"Lv: {Item.ItemLevelManager.CurrentLevel}/{Item.ItemLevelManager.MaxLevel}";
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.gameObject.SetActive(isActive);
        }
    }
}