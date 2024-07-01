
using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; private set; } = new PlayerManager();

        public Status Status => (Status)ExpLevelManager.GetCurrentStatus() + (Status)ItemLevelManager.GetCurrentStatus();

        private ExpLevelManager _expLevelManager;
        public ExpLevelManager ExpLevelManager => GetOrCreateExpLevelManager();

        private ItemLevelManager _itemLevelManager;
        public ItemLevelManager ItemLevelManager => GetOrCreateItemLevelManager();

        private ExpLevelManager GetOrCreateExpLevelManager()
        {
            if (_expLevelManager == null)
            {
                var expLevelStatusTable = Resources.Load<TextAsset>("PlayerData_ExpLevelStatusTable");
                _expLevelManager = new ExpLevelManager();
                _expLevelManager.Initialize<Status>(expLevelStatusTable);
            }
            return _expLevelManager;
        }

        private ItemLevelManager GetOrCreateItemLevelManager()
        {
            if (_itemLevelManager == null)
            {
                var itemLevelUpCostTable = Resources.Load<TextAsset>("PlayerData_ItemLevelUpCostTable");
                var itemLevelStatusTable = Resources.Load<TextAsset>("PlayerData_ItemLevelStatusTable");
                _itemLevelManager = new ItemLevelManager();
                _itemLevelManager.Initialize<Status>(itemLevelUpCostTable, itemLevelStatusTable);
            }
            return _itemLevelManager;
        }
    }
}