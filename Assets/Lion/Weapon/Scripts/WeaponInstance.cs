using Lion.Weapon.Behaviour;
using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponInstance
    {
        public WeaponInstance(WeaponData data)
        {
            Data = data;
        }

        public WeaponData Data { get; private set; }

        public bool IsActive => _gameObject != null;
        public event Action<bool> OnActiveChanged;

        private WeaponBehaviour _gameObject;

        public void Activation()
        {
            _gameObject = GameObject.Instantiate(Data.Prefab);
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivation()
        {
            GameObject.Destroy(_gameObject);
            _gameObject = null;
            OnActiveChanged?.Invoke(false);
        }
    }
}