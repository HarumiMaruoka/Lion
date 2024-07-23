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

        // ŽG‹›“G‚ðƒLƒ‹‚µ‚½”
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

        // –Ú•W‚Æ‚È‚éŽG‹›“G‚ÌƒLƒ‹”
        [SerializeField]
        private int _targetKillCount = 10;
        public int TargetKillCount => _targetKillCount;
    }
}