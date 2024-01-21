using System;
using UnityEngine;

namespace EquippedWeaponUI
{
    public interface IDraggable
    {
        WeaponBase Weapon { get; set; }
        int Index { get; }
        EquippedWeaponType EquippedWeaponType { get; }
    }
}

[Serializable]
public enum EquippedWeaponType
{
    Equipped,
    Inventory,
}