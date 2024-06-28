using System;
using UnityEngine;

namespace Lion.Item
{
    public class ItemManager
    {
        public static ItemManager Instance { get; private set; } = new ItemManager();

        public ItemSheet ItemSheet { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.ItemSheet = ScriptableObject.Instantiate(Resources.Load<ItemSheet>("ItemSheet"));
            Instance.ItemSheet.Initialize();
        }
    }
}