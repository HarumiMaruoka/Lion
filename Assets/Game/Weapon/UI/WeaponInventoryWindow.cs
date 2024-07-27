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

        private Action<WeaponInstance> onSelectedBuffer;

        public WeaponIcon GetWeaponIcon(WeaponInstance weapon) => _weaponIcons[weapon];

        public event Action OnDisabled;
        public event Action<WeaponInstance> OnSelected
        {
            add
            {
                onSelectedBuffer += value;
                foreach (var icon in _weaponIcons.Values) icon.OnSelected += value;
            }
            remove
            {
                onSelectedBuffer -= value;
                foreach (var icon in _weaponIcons.Values) icon.OnSelected -= value;
            }
        }

        private void Start()
        {
            foreach (var weapon in WeaponManager.Instance.Inventory)
            {
                OnAdded(weapon);
            }
            WeaponManager.Instance.Inventory.OnAdded += OnAdded;
            WeaponManager.Instance.Inventory.OnRemoved += OnRemoved;
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }

        private void OnDestroy()
        {
            WeaponManager.Instance.Inventory.OnAdded -= OnAdded;
            WeaponManager.Instance.Inventory.OnRemoved -= OnRemoved;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        private void OnAdded(WeaponInstance weapon)
        {
            var icon = Instantiate(_iconPrefab, _content);
            icon.SetWeapon(weapon);
            icon.OnSelected += onSelectedBuffer;
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