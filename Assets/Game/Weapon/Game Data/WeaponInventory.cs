using Lion.Save;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponInventory : IEnumerable<WeaponInstance>, ISavable
    {
        private List<WeaponInstance> _weapons = new List<WeaponInstance>();
        public List<WeaponInstance> Weapons => _weapons;

        public event Action<WeaponInstance> OnAdded;
        public event Action<WeaponInstance> OnRemoved;

        public WeaponInventory()
        {
            SaveManager.Instance.Register(this);
        }

        public void Add(WeaponInstance weapon)
        {
            _weapons.Add(weapon);
            OnAdded?.Invoke(weapon);
        }

        public void Remove(WeaponInstance weapon)
        {
            _weapons.Remove(weapon);
            OnRemoved?.Invoke(weapon);
        }

        public IEnumerator<WeaponInstance> GetEnumerator()
        {
            return _weapons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _weapons.GetEnumerator();
        }

        public void Save()
        {
            var count = PlayerPrefs.GetInt("WeaponInventoryCount", 0);
            for (int i = 0; i < _weapons.Count; i++)
            {
                var weapon = _weapons[i];
                PlayerPrefs.SetInt($"WeaponInventory{i}_InventoryIndex", i);
                PlayerPrefs.SetInt($"WeaponInventory{i}_ID", weapon.Data.ID);
                PlayerPrefs.SetInt($"WeaponInventory{i}_Level", weapon.Level);
            }
            for (int i = _weapons.Count; i < count; i++)
            {
                PlayerPrefs.DeleteKey($"WeaponInventory{i}_InventoryIndex");
                PlayerPrefs.DeleteKey($"WeaponInventory{i}_ID");
                PlayerPrefs.DeleteKey($"WeaponInventory{i}_Level");
            }
            PlayerPrefs.SetInt("WeaponInventoryCount", _weapons.Count);
        }

        public void Load()
        {
            _weapons.Clear();
            var count = PlayerPrefs.GetInt("WeaponInventoryCount", 0);
            for (int i = 0; i < count; i++)
            {
                var id = PlayerPrefs.GetInt($"WeaponInventory{i}_ID");
                var level = PlayerPrefs.GetInt($"WeaponInventory{i}_Level");
                var weapon = WeaponInstance.Create(id, level);
                _weapons.Add(weapon);
            }
        }
    }
}