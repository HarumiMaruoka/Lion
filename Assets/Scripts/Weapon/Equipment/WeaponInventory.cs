using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory
{
    private static WeaponInventory _instance;
    public static WeaponInventory Instance => _instance ??= new WeaponInventory();
    private WeaponInventory() { }

    public int Capacity => 100;

    private List<WeaponBase> _weaponCollection = new List<WeaponBase>();
    public IReadOnlyList<WeaponBase> WeaponCollection => _weaponCollection;

    public event Action<WeaponBase> OnAddedWeapon;
    public event Action<WeaponBase> OnRemovedWeapon;

    public void AddWeapon(WeaponBase weapon)
    {
        if (_weaponCollection.Count >= Capacity)
        {
            Debug.Log("‚±‚êˆÈãWeapon‚ğŠ—L‚Å‚«‚Ü‚¹‚ñB");
            return;
        }
        _weaponCollection.Add(weapon);
        OnAddedWeapon?.Invoke(weapon);
    }

    public void RemoveWeapon(WeaponBase weapon)
    {
        _weaponCollection.Remove(weapon);
        OnRemovedWeapon?.Invoke(weapon);
    }
}