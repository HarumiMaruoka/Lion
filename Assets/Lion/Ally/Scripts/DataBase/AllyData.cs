using Lion.Ally.Skill;
using Lion.LevelManagement;
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
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public SkillController SkillPrefab { get; private set; }

        private AllyItemLevelUpManager _itemLevelUpManager;
        public AllyItemLevelUpManager ItemLevelUpManager => _itemLevelUpManager ??= new AllyItemLevelUpManager(this);

        private GameObject _instance;

        public void Activate()
        {
            _instance = Instantiate(Prefab);
            _itemLevelUpManager.onActiveChanged?.Invoke(true);
        }

        public void Deactivate()
        {
            Destroy(_instance);
            _instance = null;
            _itemLevelUpManager.onActiveChanged?.Invoke(false);
        }

        private int _count; // èäéùêîÅB
        public event Action<int> OnCountChanged;
        public int Count
        {
            get => _count;
            set
            {
                var old = _count;
                _count = value;
                OnCountChanged?.Invoke(value);

                if (old == 0 && value > 0)
                {
                    LevelManager.Instance.ItemLevelUpableContainer.Register(ItemLevelUpManager);
                }
                else if (old > 0 && value == 0)
                {
                    LevelManager.Instance.ItemLevelUpableContainer.Unregister(ItemLevelUpManager);
                }
            }
        }

        public bool Unlocked => _count > 0;
        public bool Activated => _instance != null;

        private TextAsset LoadExpStatusAsset() => Resources.Load<TextAsset>($"Ally_{ID}_ExpLevelStatusTable");
        private TextAsset LoadLevelUpCostAsset() => Resources.Load<TextAsset>($"Ally_{ID}_ItemLevelUpCostTable");
        private TextAsset LoadLevelUpStatusAsset() => Resources.Load<TextAsset>($"Ally_{ID}_ItemLevelStatusTable");

        private ExpLevelManager<AllyStatus> CreateExpLevelManager() => new ExpLevelManager<AllyStatus>(LoadExpStatusAsset());
        private ItemStatusLevelManager<AllyStatus> CreateItemStatusLevelManager() => new ItemStatusLevelManager<AllyStatus>(LoadLevelUpCostAsset(), LoadLevelUpStatusAsset());

        private ExpLevelManager<AllyStatus> _expLevelManager;
        public ExpLevelManager<AllyStatus> ExpLevelManager => _expLevelManager ??= CreateExpLevelManager();

        private ItemStatusLevelManager<AllyStatus> _itemStatusLevelManager;
        public ItemStatusLevelManager<AllyStatus> ItemStatusLevelManager => _itemStatusLevelManager ??= CreateItemStatusLevelManager();

        public AllyStatus Status => _expLevelManager.GetStatus() + _itemStatusLevelManager.GetStatus();
    }
}