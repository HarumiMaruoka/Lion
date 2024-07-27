using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponParameter : IWeaponParameter
    {
        public WeaponInstance WeaponInstance { get; }
        public WeaponStatus WeaponStatus => WeaponInstance.Status;

        public WeaponParameter(WeaponInstance weaponInstance)
        {
            WeaponInstance = weaponInstance;
        }

        public IActor Actor { get; set; }

        public float PhysicalPower => WeaponStatus.PhysicalPower;
        public float MagicPower => WeaponStatus.MagicPower;
        public float Range => WeaponStatus.Range;
        public float Size => WeaponStatus.Size;
        public float Duration => WeaponStatus.Duration;
        public float AttackSpeed => WeaponStatus.AttackSpeed;
        public int Amount => WeaponStatus.Amount;
    }
}