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

        public void Initialize()
        {
            Count = 0;
            ExpLevelManager = ExpLevelManager.Create<AllyStatus>($"Ally_{ID}_ExpLevelStatusTable");
            ItemLevelManager = ItemLevelManager.Create<AllyStatus>($"Ally_{ID}_ItemLevelUpCostTable", $"Ally_{ID}_ItemLevelStatusTable");
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

        private int _count; // èäéùêîÅB
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
                    OnUnlockStatusChanged?.Invoke(false);
                }

                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        public bool Unlocked => _count > 0;
        public ExpLevelManager ExpLevelManager { get; private set; }
        public ItemLevelManager ItemLevelManager { get; private set; }

        public AllyStatus Status => (AllyStatus)ExpLevelManager.GetCurrentStatus() + (AllyStatus)ItemLevelManager.GetCurrentStatus();
    }
}