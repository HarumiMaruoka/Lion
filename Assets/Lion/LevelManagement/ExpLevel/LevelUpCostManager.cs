using System;
using UnityEngine;

namespace Lion.LevelManagement.ExperienceLevel
{
    /// <summary>
    /// 経験値に応じたレベルアップコストを管理するクラス
    /// </summary>
    public class LevelUpCostManager
    {
        private int[] _expByLevel;

        public int MaxLevel { get; }

        public LevelUpCostManager(TextAsset table)
        {
            try
            {
                var csv = table.LoadCsv(1);
                MaxLevel = csv.Length;
                _expByLevel = new int[MaxLevel + 1]; // レベル1から始まるため、+1しています
                LoadLevelUpCost(csv);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void LoadLevelUpCost(string[][] csv)
        {
            foreach (var line in csv)
            {
                var level = int.Parse(line[0]);
                if (level < 1 || level > MaxLevel)
                {
                    Debug.LogError($"Level {level} is out of range.");
                    continue;
                }

                _expByLevel[level] = int.Parse(line[1]);
            }
        }

        public int GetExperienceForLevel(int level)
        {
            if (level < 1 || level > MaxLevel)
            {
                Debug.LogError($"Level {level} is out of range.");
                return 0;
            }

            return _expByLevel[level];
        }
    }
}