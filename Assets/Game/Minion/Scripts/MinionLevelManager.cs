using Lion.LevelManagement;
using Lion.LevelManagement.ExperienceLevel;
using Lion.LevelManagement.ItemLevel;
using System;
using UnityEngine;

namespace Lion.Minion
{
    public class MinionLevelManager
    {
        public ItemLevelManager ItemLevelManager { get; }
        public LevelBasedStatusManager<MinionStatus> ItemStatusManager { get; }

        public ExperienceLevelManager ExpLevelManager { get; }
        public LevelBasedStatusManager<MinionStatus> ExpStatusManager { get; }

        public MinionStatus Status =>
            ItemStatusManager.GetStatus(ItemLevelManager.CurrentLevel) +
            ExpStatusManager.GetStatus(ExpLevelManager.CurrentLevel);

        public MinionLevelManager(MinionData minion)
        {
            var id = minion.ID;

            var asset = Resources.Load<TextAsset>($"Minion_{id}_ItemLevelStatusTable");
            ItemStatusManager = new LevelBasedStatusManager<MinionStatus>(asset);

            asset = Resources.Load<TextAsset>($"Minion_{id}_ItemLevelUpCostTable");
            ItemLevelManager = new ItemLevelManager(new LevelUpCostTable(asset, ItemStatusManager.MaxLevel));

            asset = Resources.Load<TextAsset>($"Minion_{id}_ExpLevelStatusTable");
            ExpStatusManager = new LevelBasedStatusManager<MinionStatus>(asset);

            asset = Resources.Load<TextAsset>($"Minion_{id}_ExpLevelUpCostTable");
            ExpLevelManager = new ExperienceLevelManager(new LevelUpCostManager(asset));
        }
    }
}