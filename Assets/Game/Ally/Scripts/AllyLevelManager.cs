using Lion.LevelManagement;
using Lion.LevelManagement.ExperienceLevel;
using Lion.LevelManagement.ItemLevel;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyLevelManager
    {
        public ItemLevelManager ItemLevelManager { get; }
        public LevelBasedStatusManager<AllyStatus> ItemStatusManager { get; }

        public ExperienceLevelManager ExpLevelManager { get; }
        public LevelBasedStatusManager<AllyStatus> ExpStatusManager { get; }

        public AllyStatus Status =>
            ItemStatusManager.GetStatus(ItemLevelManager.CurrentLevel) +
            ExpStatusManager.GetStatus(ExpLevelManager.CurrentLevel);

        public AllyLevelManager(AllyData ally)
        {
            var id = ally.ID;

            var asset = Resources.Load<TextAsset>($"Ally_{id}_ItemLevelStatusTable");
            ItemStatusManager = new LevelBasedStatusManager<AllyStatus>(asset);

            asset = Resources.Load<TextAsset>($"Ally_{id}_ItemLevelUpCostTable");
            ItemLevelManager = new ItemLevelManager(new LevelUpCostTable(asset, ItemStatusManager.MaxLevel));

            asset = Resources.Load<TextAsset>($"Ally_{id}_ExpLevelStatusTable");
            ExpStatusManager = new LevelBasedStatusManager<AllyStatus>(asset);

            asset = Resources.Load<TextAsset>($"Ally_{id}_ExpLevelUpCostTable");
            ExpLevelManager = new ExperienceLevelManager(new LevelUpCostManager(asset));
        }
    }
}