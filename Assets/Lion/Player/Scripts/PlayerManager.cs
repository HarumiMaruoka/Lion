
using Lion.Formation;
using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; private set; } = new PlayerManager();

        private PlayerManager()
        {
            ExpLevelManager = ExpLevelManager.Create<Status>("PlayerData_ExpLevelStatusTable");
            ItemLevelManager = ItemLevelManager.Create<Status>("PlayerData_ItemLevelUpCostTable", "PlayerData_ItemLevelStatusTable");
            HealthManager = new HealthManager(100);
        }

        public Status Status => (Status)ExpLevelManager.GetCurrentStatus() + (Status)ItemLevelManager.GetCurrentStatus();
        public float BattlePower => Status.BattlePower + FormationManager.Instance.BattlePower;

        public ExpLevelManager ExpLevelManager { get; private set; }
        public ItemLevelManager ItemLevelManager { get; private set; }
        public HealthManager HealthManager { get; private set; }
    }
}