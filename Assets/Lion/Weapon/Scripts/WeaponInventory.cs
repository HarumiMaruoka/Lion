using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponInventory : IEnumerable<WeaponInstance>
    {
        private HashSet<WeaponInstance> _weapons = new HashSet<WeaponInstance>();
        public IEnumerable<WeaponInstance> Weapons => _weapons;

        public event Action<WeaponInstance> OnAdded;
        public event Action<WeaponInstance> OnRemoved;

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
    }
}