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

        private int _count; // èäéùêîÅB
        public event Action<int> OnCountChanged;
        public int Count
        {
            get => _count;
            set
            {
                if (_count == 0 && value > 0)
                    ItemLevelableContainer.Instance.Add(this);
                else if (_count > 0 && value == 0)
                    ItemLevelableContainer.Instance.Remove(this);

                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        private ExpLevelManager _expLevelManager;
        public ExpLevelManager ExpLevelManager => _expLevelManager ??= CreateExpLevelManager();

        private ItemLevelManager _itemLevelManager;
        public ItemLevelManager ItemLevelManager => _itemLevelManager ??= CreateItemLevelManager();

        public event Action<bool> OnActiveChanged;
        public bool IsActive => _instance != null;
        public bool Unlocked => Count > 0;
        public MinionStatus Status => (MinionStatus)ExpLevelManager.GetCurrentStatus() + (MinionStatus)ItemLevelManager.GetCurrentStatus();

        private MinionController _instance;

        public void Initialize()
        {
            Count = 0;
        }

        public void Activate()
        {
            _instance = GameObject.Instantiate(Prefab);
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivate()
        {
            GameObject.Destroy(_instance);
            _instance = null;
            OnActiveChanged?.Invoke(false);
        }

        private ExpLevelManager CreateExpLevelManager()
        {
            var expTable = Resources.Load<TextAsset>($"Minion_{ID}_ExpLevelStatusTable");
            var manager = new ExpLevelManager();
            manager.Initialize<MinionStatus>(expTable);
            return manager;
        }

        private ItemLevelManager CreateItemLevelManager()
        {
            var costTable = Resources.Load<TextAsset>($"Minion_{ID}_ItemLevelUpCostTable");
            var statusTable = Resources.Load<TextAsset>($"Minion_{ID}_ItemLevelStatusTable");
            var manager = new ItemLevelManager();
            manager.Initialize<MinionStatus>(costTable, statusTable);
            return manager;
        }
    }
}