using System;
using System.Collections;
using System.Collections.Generic;

namespace Lion.LevelManagement
{
    public class ItemLevelUpableContainer : IEnumerable<IItemLevelUpable>
    {
        private HashSet<IItemLevelUpable> itemLevelUpables = new HashSet<IItemLevelUpable>();

        public event Action<IItemLevelUpable> OnAdded;
        public event Action<IItemLevelUpable> OnRemoved;

        public void Register(IItemLevelUpable itemLevelUpable)
        {
            itemLevelUpables.Add(itemLevelUpable);
            OnAdded?.Invoke(itemLevelUpable);
        }

        public void Unregister(IItemLevelUpable itemLevelUpable)
        {
            itemLevelUpables.Remove(itemLevelUpable);
            OnRemoved?.Invoke(itemLevelUpable);
        }

        public IEnumerator<IItemLevelUpable> GetEnumerator()
        {
            return itemLevelUpables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}