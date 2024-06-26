using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    public class LevelManager
    {
        public static LevelManager Instance { get; private set; } = new LevelManager();

        public ItemLevelUpableContainer ItemLevelUpableContainer { get; private set; } = new ItemLevelUpableContainer();
    }
}