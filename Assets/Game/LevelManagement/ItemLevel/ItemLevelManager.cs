using System;
using UnityEngine;

namespace Lion.LevelManagement.ItemLevel
{
    public class ItemLevelManager
    {
        private int _currentLevel;
        public LevelUpCostTable CostTable { get; }

        public ItemLevelManager(LevelUpCostTable costManager)
        {
            _currentLevel = 1;
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