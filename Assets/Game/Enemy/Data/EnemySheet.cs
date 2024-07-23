using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Enemy
{
    [CreateAssetMenu(
        fileName = "EnemySheet",
        menuName = "Game Data Sheets/EnemySheet")]
    public class EnemySheet : Lion.GameDataSheet.SheetBase<EnemyData>
    {
        private Dictionary<int, EnemyData> _enemyDataByID = new Dictionary<int, EnemyData>();

        public void Initialize()
        {
            foreach (var enemyData in this)
            {
                if (!_enemyDataByID.TryAdd(enemyData.ID, enemyData))
                {
                    Debug.LogError($"EnemySheet: Duplicate ID: {enemyData.ID}");
                }
            }
        }

        public EnemyData GetEnemyData(int id)
        {
            if (_enemyDataByID.TryGetValue(id, out var enemyData)) return enemyData;
            Debug.LogError($"EnemySheet: ID not found: {id}");
            return null;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(EnemySheet))]
    public class EnemySheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                EnemySheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}