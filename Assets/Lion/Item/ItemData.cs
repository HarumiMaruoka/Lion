using System;
using UnityEngine;

namespace Lion.Item
{
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Price { get; private set; }

        [SerializeField] private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnCountChanged?.Invoke(_count);
            }
        }

        public event Action<int> OnCountChanged;
    }
}