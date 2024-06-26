#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Lion.Item
{
    public class ItemSheetWindow : Lion.GameDataSheet.SheetWindowBase<ItemData, ItemSheet, ItemSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/ItemSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(ItemSheetWindow)).Show();
        }
    }
}
#endif