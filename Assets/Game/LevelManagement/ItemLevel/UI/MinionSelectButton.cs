using Lion.Minion;
using Lion.Minion.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI
{
    [RequireComponent(typeof(Button))]
    public class MinionSelectButton : MonoBehaviour
    {
        [SerializeField]
        private LevelUpWindow _levelUpWindow;
        [SerializeField]
        private MinionWindow _minionWindow;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TargetSelect);
        }

        private void TargetSelect()
        {
            _minionWindow.OnSelected += OnTargetSelected;
            _minionWindow.OnDisabled += OnAllyWindowDisabled;
            _minionWindow.Open();
        }

        private void OnTargetSelected(MinionData selected)
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
            _minionWindow.OnSelected -= OnTargetSelected;
            _minionWindow.OnDisabled -= OnAllyWindowDisabled;
        }
    }
}