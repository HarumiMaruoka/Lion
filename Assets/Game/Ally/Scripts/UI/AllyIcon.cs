using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Ally.UI
{
    [RequireComponent(typeof(Button))]
    public class AllyIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        [SerializeField] private Image _actorView;
        [SerializeField] private TMPro.TextMeshProUGUI _name;
        [SerializeField] private TMPro.TextMeshProUGUI _expLevel;
        [SerializeField] private TMPro.TextMeshProUGUI _itemLevel;
        [SerializeField] private TMPro.TextMeshProUGUI _haveCount;
        [SerializeField] private TMPro.TextMeshProUGUI _skillName;

        [SerializeField] private GameObject _lockedLabel;
        [SerializeField] private GameObject _activatedLabel;
        [SerializeField] private GameObject _removeLabel;

        public bool IsFormationMode { get; set; }

        private AllyData _ally;

        public AllyData Ally
        {
            get => _ally;
            set
            {
                UnsubscribeFromAllyEvents(_ally);
                _ally = value;
                SubscribeToAllyEvents(_ally);
                UpdateUI();
            }
        }

        public event Action<AllyData> OnSelected;

        private void Awake()
        {
            UpdateUI();
            GetComponent<Button>().onClick.AddListener(() => OnSelected?.Invoke(_ally));
        }

        private void OnDestroy()
        {
            UnsubscribeFromAllyEvents(_ally);
        }

        public void UpdateUI()
        {
            if (_ally == null)
            {
                _icon.color = Color.clear;
            }
            else
            {
                _icon.color = Color.white;

                _actorView.sprite = _ally.Icon;
                _name.text = _ally.Name;
                _expLevel.text = _ally.LevelManager.ExpLevelManager.CurrentLevel.ToString();
                _itemLevel.text = _ally.LevelManager.ItemLevelManager.CurrentLevel.ToString();
                _haveCount.text = _ally.Count.ToString();
                _skillName.text = "not implemented"; /*_ally.SkillPrefab.Name;*/
                _lockedLabel.SetActive(!_ally.Unlocked);
                _activatedLabel.SetActive(_ally.IsActive);
                _removeLabel.SetActive(IsFormationMode && _ally.IsActive);
            }
        }

        private void SubscribeToAllyEvents(AllyData ally)
        {
            if (ally == null) return;
            ally.OnActiveChanged += OnActiveChanged;
            ally.OnCountChanged += OnCountChanged;
            ally.OnUnlockStatusChanged += OnUnlockStatusChanged;
            ally.LevelManager.ExpLevelManager.OnLevelChanged += OnExpLevelChanged;
            ally.LevelManager.ItemLevelManager.OnLevelChanged += OnItemLevelChanged;
        }

        private void UnsubscribeFromAllyEvents(AllyData ally)
        {
            if (ally == null) return;
            ally.OnActiveChanged -= OnActiveChanged;
            ally.OnCountChanged -= OnCountChanged;
            ally.OnUnlockStatusChanged -= OnUnlockStatusChanged;
            ally.LevelManager.ExpLevelManager.OnLevelChanged -= OnExpLevelChanged;
            ally.LevelManager.ItemLevelManager.OnLevelChanged -= OnItemLevelChanged;
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.SetActive(isActive);
        }

        private void OnCountChanged(int count)
        {
            _haveCount.text = _ally.Count.ToString();
        }

        private void OnUnlockStatusChanged(bool isUnlocked)
        {
            _lockedLabel.SetActive(!isUnlocked);
        }

        private void OnExpLevelChanged(int level)
        {
            _expLevel.text = level.ToString();
        }

        private void OnItemLevelChanged(int level)
        {
            _itemLevel.text = level.ToString();
        }
    }
}