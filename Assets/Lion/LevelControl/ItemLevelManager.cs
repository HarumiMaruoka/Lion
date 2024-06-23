using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion
{
    public class ItemLevelManager<T> where T : struct, IStatus
    {
        public int Level { get; private set; }
        public int MaxLevel => LevelUpCostTable.Count;
        public Dictionary<int, List<LevelUpCost>> LevelUpCostTable { get; private set; }
        public T[] StatusTable { get; private set; }

        public ItemLevelManager(TextAsset levelUpCostTable, TextAsset statusTable)
        {
            Level = 1;

            var input = levelUpCostTable.LoadCsv(1);
            LevelUpCostTable = new Dictionary<int, List<LevelUpCost>>();

            for (int i = 0; i < input.Length; i++)
            {
                var level = int.Parse(input[i][0]);
                var itemID = int.Parse(input[i][1]);
                var amount = int.Parse(input[i][2]);

                if (!LevelUpCostTable.ContainsKey(level))
                {
                    LevelUpCostTable[level] = new List<LevelUpCost>();
                }

                LevelUpCostTable[level].Add(new LevelUpCost()
                {
                    ItemID = itemID,
                    Amount = amount,
                });
            }

            input = statusTable.LoadCsv(1);
            StatusTable = new T[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                T data = default;
                data.ItemCsvLoad(row);
                StatusTable[i] = data;
            }
        }

        public T GetStatus()
        {
            return StatusTable[Level - 1];
        }
    }

    public struct LevelUpCost
    {
        public int ItemID { get; set; }
        public int Amount { get; set; }
    }
}