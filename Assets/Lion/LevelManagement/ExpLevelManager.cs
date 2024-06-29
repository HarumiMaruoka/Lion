using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    /// <summary>
    /// 経験値の取得によってレベルアップする機能を提供するクラス。
    /// </summary>
    /// <typeparam name="T">
    /// ステータスを表す構造体。IStatusを実装している必要がある。
    /// </typeparam>
    public class ExpLevelManager<T> : LevelManager<T> where T : IStatus
    {
        public int Exp { get; private set; }
        public int[] ExpTable { get; private set; }
        public event Action<int> OnExpChanged;

        public ExpLevelManager(TextAsset expTable)
        {
            CurrentLevel = 1;
            Exp = 0;

            var input = expTable.LoadCsv(1);

            ExpTable = new int[input.Length];
            StatusTable = new T[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                ExpTable[i] = int.Parse(row[1]);

                T data = default; // ここでエラーが発生する場合は、T型が構造体か確認してください。諸事情でwhere T : structが使えないので。
                data.LoadExpSheet(row);
                StatusTable[i] = data;
            }
        }

        public void AddExp(int exp)
        {
            Exp += exp;

            if (CurrentLevel >= MaxLevel) return;

            while (Exp >= ExpTable[CurrentLevel])
            {
                CurrentLevel++;
                if (CurrentLevel >= MaxLevel) break;
            }

            OnExpChanged?.Invoke(exp);
        }

        public void SetExp(int exp)
        {
            Exp = exp;

            if (CurrentLevel >= MaxLevel) return;

            while (Exp >= ExpTable[CurrentLevel])
            {
                CurrentLevel++;
                if (CurrentLevel >= MaxLevel) break;
            }

            OnExpChanged?.Invoke(exp);
        }

        public int GetCurrentLevelExp()
        {
            return ExpTable[CurrentLevel - 1];
        }

        public int GetNextLevelExp()
        {
            if (CurrentLevel >= MaxLevel) return 0;

            return ExpTable[CurrentLevel];
        }
    }
}