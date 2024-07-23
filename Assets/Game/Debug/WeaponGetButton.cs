using Lion.Weapon;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    [RequireComponent(typeof(Button))]
    public class WeaponGetButton : MonoBehaviour
    {
        [SerializeField]
        private int _weaponID;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(GetWeapon);
        }

        private void GetWeapon()
        {
            var weapon = WeaponInstance.Create(_weaponID);
            WeaponManager.Instance.Inventory.Add(weapon);
        }
    }
}