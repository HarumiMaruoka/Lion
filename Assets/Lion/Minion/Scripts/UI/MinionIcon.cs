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

        private MinionData _minion;

        public MinionData Ally
        {
            get => _minion;
            set
            {
                if (_minion != null) _minion.OnActiveChanged -= OnActiveChanged;
                _minion = value;
                if (_minion != null) _minion.OnActiveChanged += OnActiveChanged;
                UpdateUI();
            }
        }

        public event Action<MinionData> OnSelected;

        private void Awake()
        {
            UpdateUI();
            GetComponent<Button>().onClick.AddListener(() => OnSelected?.Invoke(_minion));
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

                _actorView.sprite = _minion.IconSprite;
                _name.text = _minion.Name;
                _expLevel.text = _minion.ExpLevelManager.CurrentLevel.ToString();
                _itemLevel.text = _minion.ItemLevelManager.CurrentLevel.ToString();
                _haveCount.text = _minion.Count.ToString();
                _skillName.text = "not implemented"; /*_minion.SkillPrefab.Name;*/
                _lockedLabel.SetActive(!_minion.Unlocked);
                _activatedLabel.SetActive(_minion.IsActive);
                _detachLabel.SetActive(IsFormationMode && _minion.IsActive);
            }
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.SetActive(isActive);
        }
    }
}