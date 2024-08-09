using System;
using UnityEngine;

namespace Lion.LevelManagement.ExperienceLevel
{
    public class ExperienceLevelManager
    {
        private readonly LevelUpCostManager _costManager;

        public ExperienceLevelManager(LevelUpCostManager costManager)
        {
            CurrentLevel = 1;
            CurrentExp = 0;
            _costManager = costManager;
        }

        public int CurrentLevel { get; private set; }
        public int CurrentExp { get; private set; }

        public int CurrentLevelExp => _costManager.GetExperienceForLevel(CurrentLevel);
        public int NextLevelExp => _costManager.GetExperienceForLevel(CurrentLevel + 1);

        public event Action<int> OnExpChanged;
        public event Action<int> OnLevelChanged;

        public void AddExp(int exp)
        {
            CurrentExp += exp;
            OnExpChanged?.Invoke(CurrentExp);

            // レベルアップ処理
            // 現在の経験値が次のレベルに必要な経験値以上になるまで繰り返す
            while (CurrentLevel < _costManager.MaxLevel)
            {
                var nextLevelExp = _costManager.GetExperienceForLevel(CurrentLevel + 1);
                if (CurrentExp >= nextLevelExp)
                {
                    CurrentLevel++;
                    OnLevelChanged?.Invoke(CurrentLevel);
                }
                else
                {
                    break;
                }
            }
        }

        public void Clear()
        {
            CurrentLevel = 1;
            CurrentExp = 0;
            OnExpChanged?.Invoke(CurrentExp);
            OnLevelChanged?.Invoke(CurrentLevel);
        }
    }
}