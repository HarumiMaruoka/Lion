using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Minion.UI
{
    public class MinionIcon : MonoBehaviour
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
        [SerializeField] private GameObject _detachLabel;

        public bool IsFormationMode { get; set; }
        public MinionData FormationModeMinion { get; set; }

        private MinionData _minion;

        public MinionData Minion
        {
            get => _minion;
            set
            {
                UnsubscribeFromMinionEvents(_minion);
                _minion = value;
                SubscribeToMinionEvents(_minion);
                UpdateUI();
            }
        }

        public event Action<MinionData> OnSelected;

        private void Awake()
        {
            UpdateUI();
            GetComponent<Button>().onClick.AddListener(() => OnSelected?.Invoke(_minion));
        }

        private void OnDestroy()
        {
            UnsubscribeFromMinionEvents(_minion);
        }

        public void UpdateUI()
        {
            if (_minion == null)
            {
                _icon.color = Color.clear;
            }
            else
            {
                _icon.color = Color.white;

                _actorView.sprite = _minion.Icon;
                _name.text = _minion.Name;
                _expLevel.text = _minion.LevelManager.ExpLevelManager.CurrentLevel.ToString();
                _itemLevel.text = _minion.LevelManager.ItemLevelManager.CurrentLevel.ToString();
                _haveCount.text = _minion.Count.ToString();
                _skillName.text = "not implemented"; /*_minion.SkillPrefab.Name;*/
                _lockedLabel.SetActive(!_minion.Unlocked);
                _activatedLabel.SetActive(_minion.IsActive);
                _detachLabel.SetActive(FormationModeMinion == _minion && IsFormationMode && _minion.IsActive);
            }
        }

        private void SubscribeToMinionEvents(MinionData minion)
        {
            if (minion == null) return;
            minion.OnActiveChanged += OnActiveChanged;
            minion.OnCountChanged += OnCountChanged;
            minion.OnUnlockStatusChanged += OnUnlockStatusChanged;
            minion.LevelManager.ExpLevelManager.OnLevelChanged += OnExpLevelChanged;
            minion.LevelManager.ItemLevelManager.OnLevelChanged += OnItemLevelChanged;
        }

        private void UnsubscribeFromMinionEvents(MinionData minion)
        {
            if (minion == null) return;
            minion.OnActiveChanged -= OnActiveChanged;
            minion.OnCountChanged -= OnCountChanged;
            minion.OnUnlockStatusChanged -= OnUnlockStatusChanged;
            minion.LevelManager.ExpLevelManager.OnLevelChanged -= OnExpLevelChanged;
            minion.LevelManager.ItemLevelManager.OnLevelChanged -= OnItemLevelChanged;
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.SetActive(isActive);
        }

        private void OnCountChanged(int count)
        {
            _haveCount.text = _minion.Count.ToString();
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