using Lion.Weapon;
using Lion.Weapon.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI
{
    [RequireComponent(typeof(Button))]
    public class WeaponSelectButton : MonoBehaviour
    {
        [SerializeField]
        private LevelUpWindow _levelUpWindow;
        [SerializeField]
        private WeaponInventoryWindow _weaponInventoryWindow;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TargetSelect);
        }

        private void TargetSelect()
        {
            _weaponInventoryWindow.OnSelected += OnTargetSelected;
            _weaponInventoryWindow.OnDisabled += OnAllyWindowDisabled;
            _weaponInventoryWindow.Open();
        }

        private void OnTargetSelected(WeaponInstance selected)
        {
            var icon = selected.Data.Icon;
            var itemLevelManager = selected.LevelManager;
            var statusTable = selected.Data.LevelManager.StatusTable;
            var costTable = selected.Data.LevelManager.CostTable;

            _levelUpWindow.Open(icon, itemLevelManager, statusTable, costTable);
        }

        private void OnAllyWindowDisabled()
        {
            _weaponInventoryWindow.OnSelected -= OnTargetSelected;
            _weaponInventoryWindow.OnDisabled -= OnAllyWindowDisabled;
        }
    }
}