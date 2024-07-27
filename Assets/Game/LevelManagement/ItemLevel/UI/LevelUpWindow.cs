using Lion.LevelManagement.ItemLevel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI
{
    /// <summary>
    /// アイテムを消費してレベルアップするウィンドウ。
    /// </summary>
    public class LevelUpWindow : MonoBehaviour
    {
        [SerializeField] private Image _actorImage;

        [SerializeField] private TMPro.TextMeshProUGUI _currentLevelText;
        [SerializeField] private TMPro.TextMeshProUGUI _nextLevelText;

        [SerializeField] private TMPro.TextMeshProUGUI _currentStatusText;
        [SerializeField] private TMPro.TextMeshProUGUI _nextStatusText;

        [SerializeField] private Button _nextLevelUpButton;
        [SerializeField] private Button _nextLevelDownButton;

        [SerializeField] private Transform _requiredItemIconsParent;
        [SerializeField] private RequiredItemIcon _requiredItemIconPrefab;

        [SerializeField] private Button _applyLevelButton;

        private ItemLevelManager _target;
        private ILevelBasedStatusManager _targetStatusTable;
        private LevelUpCostTable _targetCostTable;
        private ItemLevelChanger _itemLevelChanger = new ItemLevelChanger();
        private Dictionary<int, RequiredItemIcon> _requiredItemIcons = new Dictionary<int, RequiredItemIcon>();

        private void Start()
        {
            _nextLevelUpButton.onClick.AddListener(() => ChangeNextLevel(1));
            _nextLevelDownButton.onClick.AddListener(() => ChangeNextLevel(-1));
            _applyLevelButton.onClick.AddListener(ApplyLevel);
        }

        public void Open(Sprite actorImage, ItemLevelManager target,
            ILevelBasedStatusManager targetStatusTable, LevelUpCostTable targetCostTable)
        {
            gameObject.SetActive(true);

            UnsubscribeFromTarget();

            _actorImage.sprite = actorImage;
            _target = target;
            _targetStatusTable = targetStatusTable;
            _targetCostTable = targetCostTable;

            _itemLevelChanger.Setup(target, targetCostTable);

            SubscribeToTarget();

            RefreshUI();
        }

        private void OnDestroy()
        {
            UnsubscribeFromTarget();
        }

        private void ChangeNextLevel(int amount)
        {
            _itemLevelChanger.ChangeNextLevel(amount);
            RefreshUI();
        }

        private void RefreshUI()
        {
            OnCurrentLevelChanged(_target.CurrentLevel);
            OnNextLevelChanged(_itemLevelChanger.NextLevel);
            ClearRequiredItemIcons();
            UpdateRequiredItemIcons(_itemLevelChanger.GetRequiredItems());
        }

        private void ClearRequiredItemIcons()
        {
            foreach (var icon in _requiredItemIcons.Values)
            {
                icon.SetRequiredAmount(0);
            }
        }

        private void UpdateRequiredItemIcons(IReadOnlyDictionary<int, int> requiredItems)
        {
            foreach (var item in requiredItems)
            {
                if (_requiredItemIcons.TryGetValue(item.Key, out var icon))
                {
                    icon.SetRequiredAmount(item.Value);
                }
                else
                {
                    var newIcon = Instantiate(_requiredItemIconPrefab, _requiredItemIconsParent);
                    newIcon.Setup(item.Key, item.Value);
                    _requiredItemIcons.Add(item.Key, newIcon);
                }
            }
        }

        private void ApplyLevel()
        {
            _itemLevelChanger.ApplyLevel();
            RefreshUI();
        }

        private void SubscribeToTarget()
        {
            if (_target == null) return;

            _target.OnLevelChanged += OnCurrentLevelChanged;
        }

        private void UnsubscribeFromTarget()
        {
            if (_target == null) return;

            _target.OnLevelChanged -= OnCurrentLevelChanged;
        }

        private void OnCurrentLevelChanged(int level)
        {
            _currentLevelText.text = $"Lv.{level}";
            _currentStatusText.text = _targetStatusTable.GetStatusText(level);
        }

        private void OnNextLevelChanged(int level)
        {
            _nextLevelText.text = $"Lv.{level}";
            _nextStatusText.text = _targetStatusTable.GetStatusText(level);
        }
    }
}