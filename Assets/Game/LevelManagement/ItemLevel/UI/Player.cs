using Lion.Player;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LevelManagement.UI
{
    [RequireComponent(typeof(Button))]
    public class Player : LevelUpWindowOpenButton
    {
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OpenLevelUpWindow);
        }

        private void OpenLevelUpWindow()
        {
            var icon = PlayerManager.Instance.Icon;
            var itemLevelManager = PlayerManager.Instance.LevelManager.ItemLevelManager;
            var statusTable = PlayerManager.Instance.LevelManager.ItemStatusManager;
            var costTable = PlayerManager.Instance.LevelManager.ItemLevelManager.CostTable;

            _levelUpWindow.Open(icon, itemLevelManager, statusTable, costTable);
        }
    }
}