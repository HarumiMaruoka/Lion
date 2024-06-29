using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement
{
    public abstract class LevelManager<T> where T : IStatus
    {
        public T[] StatusTable { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public int MaxLevel => StatusTable.Length;
        public void ApplyLevel(int level) { CurrentLevel = level; OnLevelChanged?.Invoke(level); }
        public T GetStatus() { return StatusTable[CurrentLevel - 1]; }
        public T GetStatus(int nextLevel) { return StatusTable[nextLevel - 1]; }
        public event Action<int> OnLevelChanged;
    }
}