using Lion.Formation;
using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();

        public Sprite Icon { get; private set; }
        public PlayerStatus Status => LevelManager.Status;
        public float BattlePower => Status.BattlePower + FormationManager.Instance.BattlePower;

        public PlayerLevelManager LevelManager { get; } = new PlayerLevelManager();
        public HPManager HPManager = new HPManager();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.HPManager.Heal(Instance.Status.HP);
            Instance.Icon = Resources.Load<Sprite>("PlayerIcon");
        }
    }
}