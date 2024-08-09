using System;
using UnityEngine;

namespace Lion.LevelManagement.ItemLevel
{
    public class ItemLevelManager
    {
        private int _currentLevel;
        public LevelUpCostTable CostTable { get; }

        public ItemLevelManager(LevelUpCostTable costManager, int initialLevel = 1)
        {
            _currentLevel = Mathf.Clamp(initialLevel, 1, costManager.MaxLevel);
            CostTable = costManager;
        }

        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                OnLevelChanged?.Invoke(_currentLevel);
            }
        }

        public event Action<int> OnLevelChanged;

        public int MaxLevel => CostTable.MaxLevel;
    }
}