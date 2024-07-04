#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Lion.Minion
{
    public class MinionSheetWindow : Lion.GameDataSheet.SheetWindowBase<MinionData, MinionSheet, MinionSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/MinionSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(MinionSheetWindow)).Show();
        }
    }
}
#endif