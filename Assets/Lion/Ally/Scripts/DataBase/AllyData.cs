using Lion.Ally.Skill;
using Lion.LevelManagement;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyData : ScriptableObject, IItemLevelable
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite ActorSprite { get; private set; }
        [field: SerializeField] public Sprite IconSprite { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public SkillController SkillPrefab { get; private set; }

        private GameObject _instance;
        public event Action<bool> OnActiveChanged;
        public bool IsActive => _instance != null;

        public void Activate()
        {
            _instance = Instantiate(Prefab, PlayerController.Instance.transform.position, Quaternion.identity);
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivate()
        {
            Destroy(_instance);
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
        public bool Activated => _instance != null;

        private TextAsset LoadExpStatusAsset() => Resources.Load<TextAsset>($"Ally_{ID}_ExpLevelStatusTable");
        private TextAsset LoadLevelUpCostAsset() => Resources.Load<TextAsset>($"Ally_{ID}_ItemLevelUpCostTable");
        private TextAsset LoadLevelUpStatusAsset() => Resources.Load<TextAsset>($"Ally_{ID}_ItemLevelStatusTable");

        private ExpLevelManager<AllyStatus> CreateExpLevelManager() => new ExpLevelManager<AllyStatus>(LoadExpStatusAsset());
        private ItemLevelManager<AllyStatus> CreateItemStatusLevelManager() => new ItemLevelManager<AllyStatus>(LoadLevelUpCostAsset(), LoadLevelUpStatusAsset());


        private ExpLevelManager<AllyStatus> _expLevelManager;
        public ExpLevelManager<AllyStatus> ExpLevelManager => _expLevelManager ??= CreateExpLevelManager();

        private ItemLevelManager<AllyStatus> _itemLevelManager;
        public ItemLevelManager<AllyStatus> ItemLevelManager => _itemLevelManager ??= CreateItemStatusLevelManager();

        public AllyStatus Status => _expLevelManager.GetStatus() + _itemLevelManager.GetStatus();

        IItemLevelManager IItemLevelable.ItemLevelManager => ItemLevelManager;
    }
}