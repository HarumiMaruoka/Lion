using Lion.Ally.Skill;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite ActorSprite { get; private set; }
        [field: SerializeField] public Sprite IconSprite { get; private set; }
        [field: SerializeField] public AllyController Prefab { get; private set; }
        [field: SerializeField] public SkillController SkillPrefab { get; private set; }

        private AllyController _instance;
        private int _count; // 所持数

        public event Action<bool> OnActiveChanged;
        public event Action<int> OnCountChanged;
        public event Action<bool> OnUnlockStatusChanged;

        public AllyLevelManager LevelManager { get; private set; }
        public bool IsActive => _instance != null;
        public bool Unlocked => _count > 0;
        public AllyStatus Status => LevelManager.Status;

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
                    OnUnlockStatusChanged?.Invoke(false);
                }

                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        public void Initialize()
        {
            Count = 0;
            LevelManager = new AllyLevelManager(this);
        }

        public void Activate()
        {
            _instance = Instantiate(Prefab, PlayerController.Instance.transform.position, Quaternion.identity);
            _instance.AllyData = this;
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivate()
        {
            Destroy(_instance.gameObject);
            _instance = null;
            OnActiveChanged?.Invoke(false);
        }
    }
}