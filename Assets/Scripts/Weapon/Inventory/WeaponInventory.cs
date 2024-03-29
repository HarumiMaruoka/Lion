using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    private static WeaponInventory _current;
    public static WeaponInventory Current => _current;
    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private Transform _weaponParent;

    public Transform WeaponParent => _weaponParent;

    public int InventoryCapacity => 30;

    private List<WeaponBase> _weaponCollection = new List<WeaponBase>();
    public IReadOnlyList<WeaponBase> WeaponCollection => _weaponCollection;

    public void AddWeapon(WeaponBase weapon)
    {
        if (_weaponCollection.Count >= InventoryCapacity)
        {
            // Debug.Log("容量がいっぱいです。武器をこれ以上格納できません！");
            GameObject.Destroy(weapon.gameObject);
            return;
        }

        weapon.Inactivate(); // インベントリの武器は非アクティブにする。
        _weaponCollection.Add(weapon);
    }

    public void RemoveWeapon(WeaponBase remove)
    {
        _weaponCollection.Remove(remove);
        // GameObject.Destroy(remove.gameObject);
    }
}