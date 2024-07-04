using Lion.Ally.Skill;
using Lion.LevelManagement;
using Lion.Player;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace Lion.Ally
{
    public class AllyData : ScriptableObject, IItemLevelable
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite ActorSprite { get; private set; }
        [field: SerializeField] public Sprite IconSprite { get; private set; }
        [field: SerializeField] public AllyController Prefab { get; private set; }
        [field: SerializeField] public SkillController SkillPrefab { get; private set; }

        private AllyController _instance;
        public event Action<bool> OnActiveChanged;
        public bool IsActive => _instance != null;

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

        public bool Unlocked => _count > 0;

        private ExpLevelManager CreateExpLevelManager()
        {
            var instance = new ExpLevelManager();
            var expTable = Resources.Load<TextAsset>($"Ally_{ID}_ExpLevelStatusTable");
            instance.Initialize<AllyStatus>(expTable);
            return instance;
        }

        private ItemLevelManager CreateItemStatusLevelManager()
        {
            var instance = new ItemLevelManager();
            var costTable = Resources.Load<TextAsset>($"Ally_{ID}_ItemLevelUpCostTable");
            var statusTable = Resources.Load<TextAsset>($"Ally_{ID}_ItemLevelStatusTable");
            instance.Initialize<AllyStatus>(costTable, statusTable);
            return instance;
        }


        private ExpLevelManager _expLevelManager;
        public ExpLevelManager ExpLevelManager => _expLevelManager ??= CreateExpLevelManager();

        private ItemLevelManager _itemLevelManager;
        public ItemLevelManager ItemLevelManager => _itemLevelManager ??= CreateItemStatusLevelManager();

        public AllyStatus Status => (AllyStatus)_expLevelManager.GetCurrentStatus() + (AllyStatus)_itemLevelManager.GetCurrentStatus();
    }
}