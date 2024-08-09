using Lion.Enemy.Boss;
using System;
using UnityEngine;

namespace Lion.Mission
{
    public class MainMission : MonoBehaviour
    {
        public static MainMission Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogError("MainMission is already exists.");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        // 雑魚敵をキルした数
        private int _killCount = 0;
        public int KillCount
        {
            get => _killCount;
            set
            {
                _killCount = value;
                OnKillCountChanged?.Invoke(_killCount);
            }
        }
        public event Action<int> OnKillCountChanged;

        // 目標となる雑魚敵のキル数
        [SerializeField]
        private int _targetKillCount = 10;
        public int TargetKillCount => _targetKillCount;

        // ボスと戦闘することが可能かどうか
        public bool CanFightBoss => KillCount >= TargetKillCount;
    }
}