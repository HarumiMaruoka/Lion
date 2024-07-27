#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Lion.Ally
{
    public class AllySheetWindow : Lion.GameDataSheet.SheetWindowBase<AllyData, AllySheet, AllySheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/AllySheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(AllySheetWindow)).Show();
        }
    }
}
#endif