using Lion.LevelManagement;
using Lion.LevelManagement.ItemLevel;
using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponLevelManager
    {
        public LevelBasedStatusManager<WeaponStatus> StatusTable { get; }
        public LevelUpCostTable CostTable { get; }

        public WeaponLevelManager(WeaponData weaponData)
        {
            var id = weaponData.ID;

            TextAsset asset;
            try
            {
                asset = Resources.Load<TextAsset>($"Weapon_{id}_ItemLevelStatusTable");
                StatusTable = new LevelBasedStatusManager<WeaponStatus>(asset);
            }
            catch (Exception)
            {
                Debug.LogError($"Weapon_{id}_ItemLevelStatusTable is not found.");
            }

            asset = Resources.Load<TextAsset>($"Weapon_{id}_ItemLevelUpCostTable");
            CostTable = new LevelUpCostTable(asset, StatusTable.MaxLevel);
        }

        public WeaponStatus GetStatus(int level)
        {
            return StatusTable.GetStatus(level);
        }
    }
}