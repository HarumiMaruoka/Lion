using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement
{
    public interface IItemLevelManager
    {
        int CurrentLevel { get; }
        int MaxLevel { get; }
        void ApplyLevel(int level);
        event Action<int> OnLevelChanged;
        List<LevelUpCost> GetLevelUpCosts(int level);
        string GetCurrentStatusText();
        string GetStatusText(int level);
    }
}