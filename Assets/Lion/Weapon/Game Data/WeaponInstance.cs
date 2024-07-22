using Lion.LevelManagement.ItemLevel;
using Lion.Weapon.Behaviour;
using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponInstance
    {
        private readonly ItemLevelManager _levelManager;
        private WeaponBehaviour _gameObject;

        public WeaponInstance(WeaponData data)
        {
            Data = data;
            _levelManager = new ItemLevelManager(data.LevelManager.CostTable);
        }

        public static WeaponInstance Create(int id)
        {
            if (WeaponManager.Instance.WeaponSheet.TryGetValue(id, out var data))
            {
                return new WeaponInstance(data);
            }
            Debug.LogError($"WeaponData not found: {id}");
            return null;
        }

        public WeaponData Data { get; }

        public bool IsActive => _gameObject != null;

        public int Level
        {
            get => _levelManager.CurrentLevel;
            set => _levelManager.CurrentLevel = value;
        }

        public WeaponStatus WeaponStatus => Data.LevelManager.GetStatus(Level);

        public event Action<bool> OnActiveChanged;

        public event Action<int> OnLevelChanged
        {
            add => _levelManager.OnLevelChanged += value;
            remove => _levelManager.OnLevelChanged -= value;
        }

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