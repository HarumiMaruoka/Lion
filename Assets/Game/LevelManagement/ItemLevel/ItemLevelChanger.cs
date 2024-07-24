using Lion.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement.ItemLevel
{
    public class ItemLevelChanger
    {
        private ItemLevelManager _itemLevelManager;
        private LevelUpCostTable _costManager;

        // キーはアイテムID, 値は要求数
        private readonly Dictionary<int, int> _itemRequirements = new Dictionary<int, int>();

        public event Action<int> OnNextLevelChanged;

        private int CurrentLevel => _itemLevelManager.CurrentLevel;
        private int MaxLevel => _costManager.MaxLevel;
        public int NextLevel { get; private set; }

        public void Setup(ItemLevelManager itemLevelManager, LevelUpCostTable costManager)
        {
            _itemLevelManager = itemLevelManager;
            _costManager = costManager;

            NextLevel = CurrentLevel;
            _itemRequirements.Clear();
        }

        public void ChangeNextLevel(int amount)
        {
            int oldLevel = NextLevel;
            NextLevel = Mathf.Clamp(NextLevel + amount, CurrentLevel, MaxLevel);

            if (oldLevel != NextLevel)
            {
                UpdateItemRequirements(oldLevel, NextLevel);
                OnNextLevelChanged?.Invoke(NextLevel);
            }
        }

        private void UpdateItemRequirements(int oldLevel, int newLevel)
        {
            var direction = Math.Sign(newLevel - oldLevel);
            var startLevel = direction > 0 ? oldLevel + 1 : newLevel + 1;
            var endLevel = direction > 0 ? newLevel : oldLevel;

            for (int level = startLevel; level <= endLevel; level++)
            {
                foreach (var cost in _costManager.GetLevelUpCost(level))
                {
                    if (_itemRequirements.ContainsKey(cost.itemID))
                    {
                        _itemRequirements[cost.itemID] += direction * cost.amount;
                    }
                    else
                    {
                        _itemRequirements[cost.itemID] = direction * cost.amount;
                    }
                }
            }
        }

        public bool CanApplyLevel()
        {
            foreach (var pair in _itemRequirements)
            {
                var itemID = pair.Key;
                var requiredAmount = pair.Value;
                var currentAmount = ItemManager.Instance.ItemSheet.GetItemData(itemID).Count;

                if (currentAmount < requiredAmount)
                {
                    return false;
                }
            }

            return true;
        }

        public void ApplyLevel()
        {
            if (NextLevel == CurrentLevel)
            {
                return;
            }

            if (!CanApplyLevel())
            {
                return;
            }

            foreach (var pair in _itemRequirements)
            {
                var itemID = pair.Key;
                var requiredAmount = pair.Value;

                ItemManager.Instance.ItemSheet.GetItemData(itemID).Count -= requiredAmount;
            }

            _itemLevelManager.CurrentLevel = NextLevel;
            Setup(_itemLevelManager, _costManager);
        }

        public IReadOnlyDictionary<int, int> GetRequiredItems()
        {
            return _itemRequirements;
        }
    }
}