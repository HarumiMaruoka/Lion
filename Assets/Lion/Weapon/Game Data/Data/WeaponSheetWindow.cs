#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponSheetWindow : Lion.GameDataSheet.SheetWindowBase<WeaponData, WeaponSheet, WeaponSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/WeaponSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(WeaponSheetWindow)).Show();
        }
    }
}
#endif