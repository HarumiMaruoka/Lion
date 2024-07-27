using Lion.Weapon;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    public class WeaponAllGetButton : MonoBehaviour
    {
        [SerializeField]
        private int _count;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(GetAllWeapons);
        }

        private void GetAllWeapons()
        {
            var weaponSheet = WeaponManager.Instance.WeaponSheet;
            foreach (var weaponData in weaponSheet)
            {
                for (int i = 0; i < _count; i++)
                {
                    var weapon = WeaponInstance.Create(weaponData.ID);
                    WeaponManager.Instance.Inventory.Add(weapon);
                }
            }
        }
    }
}