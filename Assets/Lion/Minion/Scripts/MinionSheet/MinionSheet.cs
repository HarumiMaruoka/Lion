using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Minion
{
    [CreateAssetMenu(
        fileName = "MinionSheet",
        menuName = "Game Data Sheets/MinionSheet")]
    public class MinionSheet : Lion.GameDataSheet.SheetBase<MinionData>
    {
        private Dictionary<int, MinionData> _minionDataByID = new Dictionary<int, MinionData>();

        public void Initialize()
        {
            foreach (var data in this)
            {
                if (_minionDataByID.ContainsKey(data.ID))
                {
                    Debug.LogError($"MinionSheet: Duplicated id {data.ID}");
                    continue;
                }

                _minionDataByID.Add(data.ID, data);
                data.Initialize();
            }
        }

        public MinionData GetMinionData(int id)
        {
            if (_minionDataByID.ContainsKey(id))
            {
                return _minionDataByID[id];
            }

            Debug.LogError($"MinionSheet: Not found id {id}");
            return null;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MinionSheet))]
    public class MinionSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                MinionSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}