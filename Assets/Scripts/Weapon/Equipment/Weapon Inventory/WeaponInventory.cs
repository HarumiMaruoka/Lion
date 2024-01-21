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

        Initialize();
    }

    private void OnDestroy()
    {
        _current = null;
    }

    public int EquippableCount => 4;
    public int InventoryCapacity => 6;

    private WeaponBase[] _equipped;
    private WeaponBase[] _weaponCollection;
    private Action<WeaponBase>[] _onEquippedChanged;
    private Action<WeaponBase>[] _onWeaponCollectionChanged;

    public WeaponBase[] Equipped => _equipped;
    public WeaponBase[] WeaponCollection => _weaponCollection;
    public Action<WeaponBase>[] OnEquippedChanged => _onEquippedChanged;
    public Action<WeaponBase>[] OnWeaponCollectionChanged => _onWeaponCollectionChanged;

    public void Initialize()
    {
        _equipped = new WeaponBase[EquippableCount];
        _weaponCollection = new WeaponBase[InventoryCapacity];

        _onEquippedChanged = new Action<WeaponBase>[EquippableCount];
        _onWeaponCollectionChanged = new Action<WeaponBase>[InventoryCapacity];
    }

    public void AddWeapon(WeaponBase weapon)
    {
        for (int i = 0; i < _equipped.Length; i++)
        {
            if (_equipped[i] == null)
            {
                _equipped[i] = weapon;
                _onEquippedChanged[i].Invoke(weapon);
                return;
            }
        }
        for (int i = 0; i < _weaponCollection.Length; i++)
        {
            if (_weaponCollection[i] == null)
            {
                _weaponCollection[i] = weapon;
                _onWeaponCollectionChanged[i]?.Invoke(weapon);
                return;
            }
        }
        Debug.Log("•Ší‚ðŠi”[‚Å‚«‚éêŠ‚ª‚ ‚è‚Ü‚¹‚ñI");
        GameObject.Destroy(weapon.gameObject);
    }

    public void RemoveEquipped(int removeEquippedIndex)
    {
        var equipped = _equipped[removeEquippedIndex];
        _equipped[removeEquippedIndex] = null;
        _onEquippedChanged[removeEquippedIndex].Invoke(null);

        GameObject.Destroy(equipped.gameObject);
    }

    public void RemoveCollection(int collectionIndex)
    {
        var weapon = _weaponCollection[collectionIndex];
        _weaponCollection[collectionIndex] = null;
        _onWeaponCollectionChanged[collectionIndex]?.Invoke(null);

        GameObject.Destroy(weapon.gameObject);
    }


    public void SwapEquipped(int index1, int index2)
    {
        var weapon1 = _equipped[index1];
        var weapon2 = _equipped[index2];

        _equipped[index1] = weapon2;
        _equipped[index2] = weapon1;

        _onEquippedChanged[index1].Invoke(weapon2);
        _onEquippedChanged[index2].Invoke(weapon1);
    }

    public void SwapInInventory(int index1, int index2)
    {
        var weapon1 = _weaponCollection[index1];
        var weapon2 = _weaponCollection[index2];

        _weaponCollection[index1] = weapon2;
        _weaponCollection[index2] = weapon1;

        _onWeaponCollectionChanged[index1].Invoke(weapon2);
        _onWeaponCollectionChanged[index2].Invoke(weapon1);
    }

    public void SwapEquippedAndInventory(int equippedIndex, int inventoryIndex)
    {
        var equipped = _equipped[equippedIndex];
        var inventory = _weaponCollection[inventoryIndex];

        _equipped[equippedIndex] = inventory;
        _weaponCollection[inventoryIndex] = equipped;

        _onEquippedChanged[equippedIndex].Invoke(inventory);
        _onWeaponCollectionChanged[inventoryIndex].Invoke(equipped);
    }
}