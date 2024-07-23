using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    public class LevelBasedStatusManager<T> : ILevelBasedStatusManager where T : IStatus, new()
    {
        private readonly T[] _statusByLevel;
        public int MaxLevel { get; }

        public LevelBasedStatusManager(TextAsset input, int ignoreRowCount = 1)
        {
            var csv = input.LoadCsv(ignoreRowCount);
            MaxLevel = csv.Length;
            _statusByLevel = new T[MaxLevel + 1];
            LoadStatus(csv);
        }

        private void LoadStatus(string[][] csv)
        {
            foreach (var line in csv)
            {
                var status = new T();
                status.LoadStatusFromCsv(line);
                if (status.Level <= MaxLevel)
                {
                    _statusByLevel[status.Level] = status;
                }
                else
                {
                    Debug.LogError($"Level {status.Level} exceeds the maximum level of {MaxLevel}.");
                }
            }
        }

        public T GetStatus(int level)
        {
            if (level <= MaxLevel && level > 0)
            {
                return _statusByLevel[level];
            }
            else
            {
                Debug.LogError("Level " + level + " is out of range.");
                return default(T);
            }
        }

        public string GetStatusText(int level)
        {
            return GetStatus(level).ToString();
        }
    }
}