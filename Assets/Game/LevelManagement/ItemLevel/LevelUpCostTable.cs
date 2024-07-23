using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement.ItemLevel
{
    /// <summary>
    /// アイテムを消費してレベルアップする際のコストを管理するクラス
    /// </summary>
    public class LevelUpCostTable
    {
        private readonly List<LevelUpCost>[] _costByLevel;

        public int MaxLevel { get; }

        public LevelUpCostTable(TextAsset input, int maxLevel)
        {
            var csv = input.LoadCsv(1);
            MaxLevel = maxLevel;
            _costByLevel = new List<LevelUpCost>[MaxLevel + 1];
            LoadLevelUpCost(csv);
        }

        private void LoadLevelUpCost(string[][] csv)
        {
            for (int i = 1; i <= MaxLevel; i++)
            {
                _costByLevel[i] = new List<LevelUpCost>();
            }

            foreach (var line in csv)
            {
                var level = int.Parse(line[0]);
                if (level < 1 || level > MaxLevel)
                {
                    Debug.LogError($"Level {level} is out of range.");
                    continue;
                }

                var cost = new LevelUpCost
                {
                    itemID = int.Parse(line[1]),
                    amount = int.Parse(line[2])
                };

                _costByLevel[level].Add(cost);
            }
        }

        public List<LevelUpCost> GetLevelUpCost(int level)
        {
            if (level <= MaxLevel && level > 0)
            {
                return _costByLevel[level];
            }
            else
            {
                Debug.LogError("Level " + level + " is out of range.");
                return null;
            }
        }
    }

    public struct LevelUpCost
    {
        public int itemID;
        public int amount;
    }
}