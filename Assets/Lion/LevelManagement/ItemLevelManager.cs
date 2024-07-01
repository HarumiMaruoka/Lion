using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement
{
    /// <summary>
    /// アイテムを消費してレベルアップする機能を提供するクラス。
    /// </summary>
    /// <typeparam name="T"> 
    /// ステータスを表す構造体。IStatusを実装している必要がある。
    /// </typeparam>
    public class ItemLevelManager : LevelManager
    {
        /// <summary>
        /// Keyはレベル、Valueはレベルアップに必要なアイテムとその個数を表す構造体のリスト。
        /// </summary>
        public Dictionary<int, List<LevelUpCost>> LevelUpCostTable { get; private set; }

        public ItemLevelManager() { }

        public void Initialize<T>(TextAsset levelUpCostTable, TextAsset statusTable) where T : IStatus, new()
        {
            CurrentLevel = 1;

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
            StatusTable = new IStatus[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                T data = new T();
                data.LoadItemSheet(row);
                StatusTable[i] = data;
            }
        }

        public List<LevelUpCost> GetLevelUpCosts(int level)
        {
            return LevelUpCostTable[level];
        }
    }

    public struct LevelUpCost
    {
        public int ItemID { get; set; }
        public int Amount { get; set; }
    }
}