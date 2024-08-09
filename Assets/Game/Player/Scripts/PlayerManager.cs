using Lion.Formation;
using Lion.LevelManagement;
using Lion.Save;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerManager : ISavable
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
            SaveManager.Instance.Register(Instance);
        }

        public void Save()
        {
            // レベル、ライフをセーブする。
            var itemLevel = LevelManager.ItemLevelManager.CurrentLevel;
            var exp = LevelManager.ExpLevelManager.CurrentExp;
            var hp = HPManager.CurrentHP;

            PlayerPrefs.SetInt("Player_ItemLevel", itemLevel);
            PlayerPrefs.SetInt("Player_ExpLevel", exp);
            PlayerPrefs.SetFloat("Player_HP", hp);
        }

        public void Load()
        {
            // レベル、ライフをロードする。
            var itemLevel = PlayerPrefs.GetInt("Player_ItemLevel", 1);
            var exp = PlayerPrefs.GetInt("Player_ExpLevel", 0);
            var hp = PlayerPrefs.GetFloat("Player_HP", 100);

            LevelManager.ItemLevelManager.CurrentLevel = itemLevel;
            LevelManager.ExpLevelManager.Clear();
            LevelManager.ExpLevelManager.AddExp(exp);
            HPManager.Heal(hp);
        }
    }
}