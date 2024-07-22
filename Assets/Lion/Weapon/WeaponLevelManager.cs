using Lion.LevelManagement;
using Lion.LevelManagement.ItemLevel;
using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponLevelManager
    {
        private LevelBasedStatusManager<WeaponStatus> _itemStatus;
        public readonly LevelUpCostTable CostTable;

        public WeaponLevelManager(WeaponData weaponData)
        {
            var id = weaponData.ID;

            TextAsset asset;
            try
            {
                asset = Resources.Load<TextAsset>($"Weapon_{id}_ItemLevelStatusTable");
                _itemStatus = new LevelBasedStatusManager<WeaponStatus>(asset);
            }
            catch (Exception)
            {
                Debug.LogError($"Weapon_{id}_ItemLevelStatusTable is not found.");
            }

            asset = Resources.Load<TextAsset>($"Weapon_{id}_ItemLevelUpCostTable");
            CostTable = new LevelUpCostTable(asset);
        }

        public WeaponStatus GetStatus(int level)
        {
            return _itemStatus.GetStatus(level);
        }
    }
}