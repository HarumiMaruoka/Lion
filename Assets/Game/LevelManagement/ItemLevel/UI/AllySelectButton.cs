using Lion.Ally;
using Lion.Ally.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI
{
    [RequireComponent(typeof(Button))]
    public class AllySelectButton : MonoBehaviour
    {
        [SerializeField]
        private LevelUpWindow _levelUpWindow;
        [SerializeField]
        private AllyWindow _allyWindow;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TargetSelect);
        }

        private void TargetSelect()
        {
            _allyWindow.OnSelected += OnTargetSelected;
            _allyWindow.OnDisabled += OnAllyWindowDisabled;
            _allyWindow.Open();
        }

        private void OnTargetSelected(AllyData selected)
        {
            if (!selected.Unlocked) return;

            var icon = selected.Icon;
            var itemLevelManager = selected.LevelManager.ItemLevelManager;
            var statusTable = selected.LevelManager.ItemStatusManager;
            var costTable = selected.LevelManager.ItemLevelManager.CostTable;

            _levelUpWindow.Open(icon, itemLevelManager, statusTable, costTable);
        }

        private void OnAllyWindowDisabled()
        {
            _allyWindow.OnSelected -= OnTargetSelected;
            _allyWindow.OnDisabled -= OnAllyWindowDisabled;
        }
    }
}