using Lion.LevelManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyItemLevelUpManager : IItemLevelUpable
    {
        private AllyData _ally;

        public AllyItemLevelUpManager(AllyData ally)
        {
            _ally = ally;
        }

        public int Level => _ally.ItemStatusLevelManager.Level;

        public int MaxLevel => _ally.ItemStatusLevelManager.MaxLevel;

        public bool IsActive
        {
            get
            {
                Debug.Log("–¢ŽÀ‘•");
                return false;
            }
        }

        public Sprite ActorSprite => _ally.ActorSprite;

        public Sprite IconSprite => _ally.IconSprite;

        public event Action<int> OnLevelUp;
        public Action<bool> onActiveChanged;
        public event Action<bool> OnActiveChanged
        {
            add => onActiveChanged += value;
            remove => onActiveChanged -= value;
        }

        public List<LevelUpCost> GetLevelUpCosts(int level)
        {
            return _ally.ItemStatusLevelManager.LevelUpCostTable[level];
        }

        public object GetStatus()
        {
            return _ally.ItemStatusLevelManager.GetStatus();
        }

        public object GetStatus(int nextLevel)
        {
            return _ally.ItemStatusLevelManager.GetStatus(nextLevel);
        }

        public void LevelUp(int level)
        {
            _ally.ItemStatusLevelManager.ApplyLevel(level);
            OnLevelUp?.Invoke(Level);
        }
    }
}