using System;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeaponInventoryWindow : MonoBehaviour
{
    [SerializeField]
    private EquippedWeaponInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private Dictionary<WeaponBase, EquippedWeaponInventoryWindowElement> _activeElements = new Dictionary<WeaponBase, EquippedWeaponInventoryWindowElement>();
    private Stack<EquippedWeaponInventoryWindowElement> _inactiveElements = new Stack<EquippedWeaponInventoryWindowElement>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < WeaponInventory.Instance.Capacity; i++)
        {
            CreateElement();
        }
        WeaponInventory.Instance.OnAddedWeapon += OnAddedWeapon;
        WeaponInventory.Instance.OnRemovedWeapon += OnRemovedWeapon;
    }

    private EquippedWeaponInventoryWindowElement CreateElement()
    {
        var instance = Instantiate(_elementPrefab, _elementParent);
        instance.WeaponData = null;
        _inactiveElements.Push(instance);
        return instance;
    }

    private void OnAddedWeapon(WeaponBase weapon)
    {
        if (_inactiveElements.Count == 0)
        {
            Debug.Log("—˜—p‰Â”\‚ÈElement‚ª‚ ‚è‚Ü‚¹‚ñB");
            return;
        }
        var element = _inactiveElements.Pop();
        element.WeaponData = weapon.Data;
        _activeElements.Add(weapon, element);
    }

    private void OnRemovedWeapon(WeaponBase weapon)
    {
        var element = _activeElements[weapon];
        _activeElements.Remove(weapon);
        element.WeaponData = null;
        _inactiveElements.Push(element);
    }
}