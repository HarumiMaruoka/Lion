using System;
using UnityEngine;

namespace Lion.Item
{
    public class ItemManager
    {
        public static ItemManager Instance { get; private set; } = new ItemManager();

        private ItemSheet _itemSheet;
        public ItemSheet ItemSheet => _itemSheet ??= Resources.Load<ItemSheet>("ItemSheet");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.ItemSheet.Initialize();
        }
    }
}