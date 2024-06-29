using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Item
{
    [CreateAssetMenu(
        fileName = "ItemSheet",
        menuName = "Game Data Sheets/ItemSheet")]
    public class ItemSheet : Lion.GameDataSheet.SheetBase<ItemData>
    {
        private Dictionary<int, ItemData> _itemDataById = new Dictionary<int, ItemData>();

        public void Initialize()
        {
            foreach (var itemData in this)
            {
                itemData.Count = 0;
                if (!_itemDataById.TryAdd(itemData.ID, itemData))
                {
                    Debug.LogError($"ItemSheet: Duplicate ID: {itemData.ID}");
                }
            }
        }

        public ItemData GetItemData(int id)
        {
            if (_itemDataById.TryGetValue(id, out var itemData)) return itemData;

            Debug.LogError($"ItemSheet: ID not found: {id}");
            return null;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ItemSheet))]
    public class ItemSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                ItemSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}