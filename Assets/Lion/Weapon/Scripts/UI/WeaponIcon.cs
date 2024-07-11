using System;
using UnityEngine;

namespace Lion.Weapon.UI
{
    public class WeaponIcon : MonoBehaviour
    {
        private WeaponInstance _weapon;

        public void SetWeapon(WeaponInstance weapon)
        {
            _weapon = weapon;
        }
    }
}