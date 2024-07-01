#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Lion.Enemy
{
    public class EnemySheetWindow : Lion.GameDataSheet.SheetWindowBase<EnemyData, EnemySheet, EnemySheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/EnemySheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(EnemySheetWindow)).Show();
        }
    }
}
#endif