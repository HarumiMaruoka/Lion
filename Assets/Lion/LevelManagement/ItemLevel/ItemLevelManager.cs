using System;
using UnityEngine;

namespace Lion.LevelManagement.ItemLevel
{
    public class ItemLevelManager
    {
        private int _currentLevel;
        private LevelUpCostTable _costManager;

        public ItemLevelManager(LevelUpCostTable costManager)
        {
            _currentLevel = 1;
            _costManager = costManager;
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

        public int MaxLevel => _costManager.MaxLevel;
    }
}