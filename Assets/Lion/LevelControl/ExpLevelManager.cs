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
    public class ExpLevelManager<T> where T : struct, IStatus
    {
        public int Level { get; private set; }
        public int MaxLevel => ExpTable.Length;
        public int Exp { get; private set; }
        public int[] ExpTable { get; private set; }
        public T[] StatusTable { get; private set; }

        public event Action<int> OnExpChanged;

        public ExpLevelManager(TextAsset expTable)
        {
            Level = 1;
            Exp = 0;

            var input = expTable.LoadCsv(1);

            ExpTable = new int[input.Length];
            StatusTable = new T[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                ExpTable[i] = int.Parse(row[1]);

                T data = default;
                data.ExpCsvLoad(row);
                StatusTable[i] = data;
            }
        }

        public void AddExp(int exp)
        {
            Exp += exp;

            if (Level >= MaxLevel) return;

            while (Exp >= ExpTable[Level])
            {
                Level++;
                if (Level >= MaxLevel) break;
            }

            OnExpChanged?.Invoke(exp);
        }

        public void SetExp(int exp)
        {
            Exp = exp;

            if (Level >= MaxLevel) return;

            while (Exp >= ExpTable[Level])
            {
                Level++;
                if (Level >= MaxLevel) break;
            }

            OnExpChanged?.Invoke(exp);
        }

        public T GetStatus()
        {
            return StatusTable[Level - 1];
        }

        public int GetLevelExp()
        {
            return ExpTable[Level - 1];
        }

        public int GetNextLevelExp()
        {
            if (Level >= MaxLevel) return 0;

            return ExpTable[Level];
        }
    }
}