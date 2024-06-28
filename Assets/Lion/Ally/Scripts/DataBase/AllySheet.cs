using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Ally
{
    [CreateAssetMenu(
    fileName = "AllySheet",
    menuName = "Game Data Sheets/AllySheet")]
    public class AllySheet : Lion.GameDataSheet.SheetBase<AllyData>
    {
        private Dictionary<int, AllyData> _allyDataByID = new Dictionary<int, AllyData>();

        public void Initialize()
        {
            foreach (var data in this)
            {
                _allyDataByID.Add(data.ID, data);
            }
        }
    }


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