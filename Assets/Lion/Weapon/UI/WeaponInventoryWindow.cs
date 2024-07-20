using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.UI
{
    public class WeaponInventoryWindow : MonoBehaviour
    {
        [SerializeField] private WeaponIcon _iconPrefab;
        [SerializeField] private Transform _content;

        private Dictionary<WeaponInstance, WeaponIcon> _weaponIcons = new Dictionary<WeaponInstance, WeaponIcon>();

        private void Start()
        {
            foreach (var weapon in WeaponManager.Instance.Inventory)
            {
                OnAdded(weapon);
            }
            WeaponManager.Instance.Inventory.OnAdded += OnAdded;
            WeaponManager.Instance.Inventory.OnRemoved += OnRemoved;
        }

        private void OnDestroy()
        {
            WeaponManager.Instance.Inventory.OnAdded -= OnAdded;
            WeaponManager.Instance.Inventory.OnRemoved -= OnRemoved;
        }

        private void OnAdded(WeaponInstance weapon)
        {
            var icon = Instantiate(_iconPrefab, _content);
            icon.SetWeapon(weapon);
            _weaponIcons.Add(weapon, icon);
        }

        private void OnRemoved(WeaponInstance weapon)
        {
            if (_weaponIcons.TryGetValue(weapon, out var icon))
            {
                Destroy(icon.gameObject);
                _weaponIcons.Remove(weapon);
            }
        }
    }
}