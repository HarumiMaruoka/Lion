using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AllyData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite IconSprite { get; private set; }

        [field: SerializeField] public TextAsset ExpStatusTable { get; private set; }
        [field: SerializeField] public TextAsset ItemLevelUpCostTable { get; private set; }
        [field: SerializeField] public TextAsset ItemLevelUpStatusTable { get; private set; }

        private int _count; // Š”B
        public event Action<int> OnCountChanged;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnCountChanged?.Invoke(value);
            }
        }

        public bool UnLocked => _count > 0;
    }
}