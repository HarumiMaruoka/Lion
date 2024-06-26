using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; private set; } = new PlayerManager();

        public Status Status => ExpLevelManager.GetStatus() + ItemLevelManager.GetStatus();

        private ExpLevelManager<Status> _expLevelManager;
        public ExpLevelManager<Status> ExpLevelManager => GetOrCreateExpLevelManager();

        private ItemStatusLevelManager<Status> _itemLevelManager;
        public ItemStatusLevelManager<Status> ItemLevelManager => GetOrCreateItemLevelManager();

        private ExpLevelManager<Status> GetOrCreateExpLevelManager()
        {
            if (_expLevelManager == null)
            {
                var expLevelStatusTable = Resources.Load<TextAsset>("PlayerData_ExpLevelStatusTable");
                _expLevelManager = new ExpLevelManager<Status>(expLevelStatusTable);
            }
            return _expLevelManager;
        }

        private ItemStatusLevelManager<Status> GetOrCreateItemLevelManager()
        {
            if (_itemLevelManager == null)
            {
                var itemLevelUpCostTable = Resources.Load<TextAsset>("PlayerData_ItemLevelUpCostTable");
                var itemLevelStatusTable = Resources.Load<TextAsset>("PlayerData_ItemLevelStatusTable");
                _itemLevelManager = new ItemStatusLevelManager<Status>(itemLevelUpCostTable, itemLevelStatusTable);
            }
            return _itemLevelManager;
        }
    }
}