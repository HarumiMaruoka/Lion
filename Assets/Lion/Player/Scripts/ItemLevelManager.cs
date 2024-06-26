using Lion.LevelManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Player
{
    public class ItemLevelManager : MonoBehaviour, IItemLevelUpable
    {
        [SerializeField]
        private Sprite _actorSprite;
        [SerializeField]
        private Sprite _iconSprite;

        public int Level => PlayerManager.Instance.ItemLevelManager.Level;

        public int MaxLevel => PlayerManager.Instance.ItemLevelManager.MaxLevel;

        public bool IsActive => true;

        public Sprite ActorSprite => _actorSprite;

        public Sprite IconSprite => _iconSprite;

        public event Action<int> OnLevelUp;
        public event Action<bool> OnActiveChanged;

        private void Start()
        {
            LevelManager.Instance.ItemLevelUpableContainer.Register(this);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.ItemLevelUpableContainer.Unregister(this);
        }

        private void OnEnable()
        {
            OnActiveChanged?.Invoke(true);
        }

        private void OnDisable()
        {
            OnActiveChanged?.Invoke(false);
        }

        public List<LevelUpCost> GetLevelUpCosts(int level)
        {
            return PlayerManager.Instance.ItemLevelManager.LevelUpCostTable[level];
        }

        public void LevelUp(int level)
        {
            PlayerManager.Instance.ItemLevelManager.ApplyLevel(level);
            OnLevelUp?.Invoke(Level);
        }

        public object GetStatus()
        {
            return PlayerManager.Instance.ItemLevelManager.GetStatus();
        }

        public object GetStatus(int nextLevel)
        {
            return PlayerManager.Instance.ItemLevelManager.GetStatus(nextLevel);
        }
    }
}