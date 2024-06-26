using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement
{
    /// <summary>
    /// アイテムを消費してレベルアップする機能を提供するクラス。
    /// </summary>
    public interface IItemLevelUpable
    {
        int Level { get; }
        int MaxLevel { get; }

        bool IsActive { get; }

        Sprite ActorSprite { get; }
        Sprite IconSprite { get; }

        void LevelUp(int level);
        List<LevelUpCost> GetLevelUpCosts(int level);
        object GetStatus();
        object GetStatus(int nextLevel);

        event Action<int> OnLevelUp;
        event Action<bool> OnActiveChanged;
    }
}