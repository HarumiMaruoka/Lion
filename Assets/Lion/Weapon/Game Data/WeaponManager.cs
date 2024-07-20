using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponManager
    {
        public static WeaponManager Instance { get; private set; } = new WeaponManager();

        public WeaponSheet WeaponSheet { get; private set; }
        public WeaponInventory Inventory { get; private set; } = new WeaponInventory();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance.WeaponSheet = Resources.Load<WeaponSheet>("WeaponSheet");
            Instance.WeaponSheet.Initialize();
        }
    }
}