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

        private AllyData _ally;

        public AllyData Ally
        {
            get => _ally;
            set
            {
                if (_ally != null) _ally.OnActiveChanged -= OnActiveChanged;
                _ally = value;
                if (_ally != null) _ally.OnActiveChanged += OnActiveChanged;
                UpdateUI();
            }
        }

        public event Action<AllyData> OnSelected;

        private void Awake()
        {
            UpdateUI();
            GetComponent<Button>().onClick.AddListener(() => OnSelected?.Invoke(_ally));
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

                _actorView.sprite = _ally.IconSprite;
                _name.text = _ally.Name;
                _expLevel.text = _ally.ExpLevelManager.CurrentLevel.ToString();
                _itemLevel.text = _ally.ItemLevelManager.CurrentLevel.ToString();
                _haveCount.text = _ally.Count.ToString();
                _skillName.text = "not implemented"; /*_ally.SkillPrefab.Name;*/
                _lockedLabel.SetActive(!_ally.Unlocked);
                _activatedLabel.SetActive(_ally.Activated);
            }
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.SetActive(isActive);
        }
    }
}