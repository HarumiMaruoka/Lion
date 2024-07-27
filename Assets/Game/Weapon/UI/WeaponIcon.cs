using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Weapon.UI
{
    [RequireComponent(typeof(Button))]
    public class WeaponIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _itemView;
        [SerializeField]
        private TMPro.TextMeshProUGUI _levelView;
        [SerializeField]
        private GameObject _activatedLabel;

        [field: SerializeField]
        public GameObject RemoveLabel { get; private set; }

        private WeaponInstance _weapon;

        public event Action<WeaponInstance> OnSelected;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Select);
        }

        private void Select()
        {
            OnSelected?.Invoke(_weapon);
        }

        public void SetWeapon(WeaponInstance weapon)
        {
            _weapon = weapon;
            _itemView.sprite = weapon.Data.Icon;
            _activatedLabel.SetActive(weapon.IsActive);

            UpdateLevel(default);

            weapon.OnLevelChanged += UpdateLevel;
            weapon.OnActiveChanged += OnActiveChanged;
        }

        private void OnDestroy()
        {
            _weapon.OnLevelChanged -= UpdateLevel;
            _weapon.OnActiveChanged -= OnActiveChanged;
        }

        private void UpdateLevel(int _)
        {
            var level = _weapon.LevelManager.CurrentLevel;
            var maxLevel = _weapon.LevelManager.MaxLevel;

            _levelView.text = $"Lv.{level}/{maxLevel}";
        }

        private void OnActiveChanged(bool isActive)
        {
            _activatedLabel.SetActive(isActive);
        }
    }
}