using Lion.LevelManagement.ItemLevel;
using Lion.Weapon.Behaviour;
using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponInstance
    {
        private WeaponBehaviour _gameObject;

        public WeaponInstance(WeaponData data)
        {
            Data = data;
            LevelManager = new ItemLevelManager(data.LevelManager.CostTable);
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
        public ItemLevelManager LevelManager { get; }

        public bool IsActive => _gameObject != null;

        public int Level
        {
            get => LevelManager.CurrentLevel;
            set => LevelManager.CurrentLevel = value;
        }

        public WeaponStatus WeaponStatus => Data.LevelManager.GetStatus(Level);

        public event Action<bool> OnActiveChanged;

        public event Action<int> OnLevelChanged
        {
            add => LevelManager.OnLevelChanged += value;
            remove => LevelManager.OnLevelChanged -= value;
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