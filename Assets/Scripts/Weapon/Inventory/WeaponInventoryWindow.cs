using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryWindow : MonoBehaviour
{
    [SerializeField]
    private WeaponInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private HashSet<WeaponInventoryWindowElement> _actives = new HashSet<WeaponInventoryWindowElement>();
    private Stack<WeaponInventoryWindowElement> _inactives = new Stack<WeaponInventoryWindowElement>();

    public event Action<WeaponBase> OnSelectedWeapon;

    private void OnEnable()
    {
        Show();
    }

    private void OnDisable()
    {
        Hide();
    }

    public void Show()
    {
        var weaponCollection = WeaponInventory.Current.WeaponCollection;
        foreach (var item in weaponCollection)
        {
            WeaponInventoryWindowElement elem;
            if (_inactives.Count != 0)
            {
                elem = _inactives.Pop();
            }
            else
            {
                elem = Instantiate(_elementPrefab, _elementParent);
            }

            elem.Weapon = item;

            elem.gameObject.SetActive(true);
            elem.OnSelected += OnSelectedWeapon;
            _actives.Add(elem);
        }
    }

    public void Hide()
    {
        foreach (var active in _actives)
        {
            active.gameObject.SetActive(false);
            active.OnSelected -= OnSelectedWeapon;
            _inactives.Push(active);
        }
        _actives.Clear();
    }
}