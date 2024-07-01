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
    public class ExpLevelManager : LevelManager
    {
        public int Exp { get; private set; }
        public int[] ExpTable { get; private set; }
        public event Action<int> OnExpChanged;

        public ExpLevelManager() { }

        public void Initialize<T>(TextAsset expTable) where T : IStatus, new()
        {
            CurrentLevel = 1;
            Exp = 0;

            var input = expTable.LoadCsv(1);

            ExpTable = new int[input.Length];
            StatusTable = new IStatus[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                ExpTable[i] = int.Parse(row[1]);

                T data = new();
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