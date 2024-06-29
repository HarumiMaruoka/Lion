using System;
using System.Collections;
using System.Collections.Generic;

namespace Lion.LevelManagement
{
    public class ItemLevelableContainer : IEnumerable<IItemLevelable>
    {
        public static ItemLevelableContainer Instance { get; private set; } = new ItemLevelableContainer();

        private List<IItemLevelable> ItemLevelables { get; } = new List<IItemLevelable>();

        public event Action<IItemLevelable> OnAdded;
        public event Action<IItemLevelable> OnRemoved;

        public void Add(IItemLevelable itemLevelable)
        {
            ItemLevelables.Add(itemLevelable);
            OnAdded?.Invoke(itemLevelable);
        }

        public void Remove(IItemLevelable itemLevelable)
        {
            ItemLevelables.Remove(itemLevelable);
            OnRemoved?.Invoke(itemLevelable);
        }

        public IEnumerator<IItemLevelable> GetEnumerator()
        {
            return ItemLevelables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}