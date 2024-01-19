using System;
using UnityEngine;

public class EquippedWeaponManager : MonoBehaviour
{
    private static EquippedWeaponManager _current;
    public static EquippedWeaponManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
        Initialize();
    }
    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private int _equippableWeaponCount;

    private WeaponBase[] _equippedWeapons;
    public Action<WeaponBase>[] OnEquippedWeapon;

    private void Initialize()
    {
        _equippedWeapons = new WeaponBase[_equippableWeaponCount];
        OnEquippedWeapon = new Action<WeaponBase>[_equippableWeaponCount];
    }

    private WeaponBase EquipWeapon(int index, WeaponBase equip)
    {
        var old = _equippedWeapons[index];
        _equippedWeapons[index] = equip;
        OnEquippedWeapon[index]?.Invoke(equip);
        return old;
    }
}