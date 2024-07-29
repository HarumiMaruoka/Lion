using System;
using UnityEngine;

namespace Lion.Stage
{
    public class StageManager
    {
        public static StageManager Instance { get; private set; } = new StageManager();

        private bool _isBattleScene = false;
        public event Action<bool> OnBattleSceneChanged;
        public bool IsBattleScene
        {
            get => _isBattleScene;
            set
            {
                _isBattleScene = value;
                OnBattleSceneChanged?.Invoke(_isBattleScene);
            }
        }
    }
}