using System;
using UnityEngine;

namespace Lion.Ally
{
    [CreateAssetMenu(
    fileName = "AllySheet",
    menuName = "Game Data Sheets/AllySheet")]
    public class AllySheet : Lion.GameDataSheet.SheetBase<AllyData> { }


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(AllySheet))]
    public class AllySheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                AllySheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}