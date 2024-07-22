using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement.ItemLevel
{
    public class ItemLevelChanger
    {
        private readonly ItemLevelManager _itemLevelManager;
        private readonly LevelUpCostTable _costManager;

        // キーはアイテムID, 値は要求数
        private readonly Dictionary<int, int> _itemRequirements = new Dictionary<int, int>();

        public event Action<int> OnNextLevelChanged;

        public ItemLevelChanger(ItemLevelManager itemLevelManager, LevelUpCostTable costManager)
        {
            _itemLevelManager = itemLevelManager;
            _costManager = costManager;
        }

        private int CurrentLevel => _itemLevelManager.CurrentLevel;
        private int MaxLevel => _costManager.MaxLevel;
        public int NextLevel { get; private set; }

        public void Reset()
        {
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
            Debug.Log("ここでアイテムを持っているかチェックする処理を書く");
            //foreach (var pair in _itemRequirements)
            //{
            //    // ここでアイテムを持っているかチェックする処理を書く
            //    Debug.Log($"ItemID: {pair.Key}, Amount: {pair.Value}");
            //}

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

            Debug.Log("ここでアイテムを減らす処理を書く");
            //foreach (var pair in _itemRequirements)
            //{
            //    // ここでアイテムを減らす処理を書く
            //    Debug.Log($"ItemID: {pair.Key}, Amount: {pair.Value}");
            //}

            _itemLevelManager.CurrentLevel = NextLevel;
            Reset();
        }
    }
}