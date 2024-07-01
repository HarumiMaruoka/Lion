using Lion.LevelManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerItemLevelManager : MonoBehaviour, IItemLevelable
    {
        [SerializeField]
        private Sprite _actorSprite;
        [SerializeField]
        private Sprite _iconSprite;

        public bool IsActive => true;
        public event Action<bool> OnActiveChanged;

        public Sprite ActorSprite => _actorSprite;
        public Sprite IconSprite => _iconSprite;

        private ItemLevelManager _levelManager;
        public ItemLevelManager ItemLevelManager => _levelManager ??= PlayerManager.Instance.ItemLevelManager;

        private void OnEnable()
        {
            OnActiveChanged?.Invoke(true);
            ItemLevelableContainer.Instance.Add(this);
        }

        private void OnDisable()
        {
            OnActiveChanged?.Invoke(false);
            ItemLevelableContainer.Instance.Remove(this);
        }
    }
}