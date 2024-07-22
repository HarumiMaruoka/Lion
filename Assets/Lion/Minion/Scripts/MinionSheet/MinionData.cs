using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Minion
{
    public class MinionData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite IconSprite { get; private set; }
        [field: SerializeField] public Sprite ActorSprite { get; private set; }
        [field: SerializeField] public MinionController Prefab { get; private set; }

        private int _count; // 所持数。
        public bool Unlocked => Count > 0;
        public event Action<int> OnCountChanged;
        public event Action<bool> OnUnlockStatusChanged;
        public int Count
        {
            get => _count;
            set
            {
                if (_count == 0 && value > 0)
                {
                    OnUnlockStatusChanged?.Invoke(true);
                }
                else if (_count > 0 && value == 0)
                {
                    OnActiveChanged?.Invoke(false);
                }

                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        public event Action<bool> OnActiveChanged;
        public bool IsActive => _instance != null;

        private MinionController _instance;

        public MinionLevelManager LevelManager { get; private set; }

        public void Initialize()
        {
            Count = 0;
            LevelManager = new MinionLevelManager(this);
        }

        public void Activate()
        {
            _instance = GameObject.Instantiate(Prefab);
            _instance.MinionData = this;
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivate()
        {
            GameObject.Destroy(_instance.gameObject);
            _instance = null;
            OnActiveChanged?.Invoke(false);
        }

        public MinionStatus Status => LevelManager.Status;
    }
}