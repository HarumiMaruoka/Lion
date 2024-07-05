using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Minion
{
    public class MinionData : ScriptableObject, IItemLevelable
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite IconSprite { get; private set; }
        [field: SerializeField] public Sprite ActorSprite { get; private set; }
        [field: SerializeField] public MinionController Prefab { get; private set; }

        public ExpLevelManager ExpLevelManager { get; private set; }
        public ItemLevelManager ItemLevelManager { get; private set; }

        private int _count; // ŠŽ”B
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
                    ItemLevelableContainer.Instance.Add(this);
                    OnUnlockStatusChanged?.Invoke(true);
                }
                else if (_count > 0 && value == 0)
                {
                    ItemLevelableContainer.Instance.Remove(this);
                    OnActiveChanged?.Invoke(false);
                }

                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        public event Action<bool> OnActiveChanged;
        public bool IsActive => _instance != null;
        public MinionStatus Status => (MinionStatus)ExpLevelManager.GetCurrentStatus() + (MinionStatus)ItemLevelManager.GetCurrentStatus();

        private MinionController _instance;

        public void Initialize()
        {
            Count = 0;
            ExpLevelManager = ExpLevelManager.Create<MinionStatus>($"Minion_{ID}_ExpLevelStatusTable");
            ItemLevelManager = ItemLevelManager.Create<MinionStatus>($"Minion_{ID}_ItemLevelUpCostTable", $"Minion_{ID}_ItemLevelStatusTable");
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
    }
}