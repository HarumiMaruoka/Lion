using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon
{
    [CreateAssetMenu(
        fileName = "WeaponSheet",
        menuName = "Game Data Sheets/WeaponSheet")]
    public class WeaponSheet : Lion.GameDataSheet.SheetBase<WeaponData>
    {
        private Dictionary<int, WeaponData> _weaponDataByID = new Dictionary<int, WeaponData>();

        public void Initialize()
        {
            foreach (var data in this)
            {
                if (!_weaponDataByID.TryAdd(data.ID, data))
                {
                    Debug.LogError($"Duplicate ID: {data.ID}");
                }
                data.Initialize();
            }
        }
    }


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(WeaponSheet))]
    public class WeaponSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                WeaponSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}