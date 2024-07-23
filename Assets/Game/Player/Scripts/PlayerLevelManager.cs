using Lion.LevelManagement;
using Lion.LevelManagement.ExperienceLevel;
using Lion.LevelManagement.ItemLevel;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerLevelManager
    {
        public ItemLevelManager ItemLevelManager { get; }
        public LevelBasedStatusManager<PlayerStatus> ItemStatusManager { get; }

        public ExperienceLevelManager ExpLevelManager { get; }
        public LevelBasedStatusManager<PlayerStatus> ExpStatusManager { get; }

        public PlayerStatus Status =>
            ItemStatusManager.GetStatus(ItemLevelManager.CurrentLevel) +
            ExpStatusManager.GetStatus(ExpLevelManager.CurrentLevel);

        public PlayerLevelManager()
        {
            var asset = Resources.Load<TextAsset>("PlayerData_ItemLevelStatusTable");
            ItemStatusManager = new LevelBasedStatusManager<PlayerStatus>(asset);

             asset = Resources.Load<TextAsset>("PlayerData_ItemLevelUpCostTable");
            ItemLevelManager = new ItemLevelManager(new LevelUpCostTable(asset, ItemStatusManager.MaxLevel));

            asset = Resources.Load<TextAsset>("PlayerData_ExpLevelStatusTable");
            ExpStatusManager = new LevelBasedStatusManager<PlayerStatus>(asset);

            asset = Resources.Load<TextAsset>("PlayerData_ExpLevelUpCostTable");
            ExpLevelManager = new ExperienceLevelManager(new LevelUpCostManager(asset));
        }
    }
}