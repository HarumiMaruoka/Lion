using Lion.Weapon;
using System;
using UnityEngine;

namespace Lion.Formation
{
    public interface IWeaponEquippable
    {
        void Equip(WeaponInstance weapon, int index);
        WeaponInstance Equipped(int index);
    }
}