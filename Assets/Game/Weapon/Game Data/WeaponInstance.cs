using Lion.LevelManagement.ItemLevel;
using Lion.Weapon.Behaviour;
using System;
using System.Net.WebSockets;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponInstance
    {
        private WeaponBehaviour _gameObject;
        public WeaponParameter Parameter { get; }

        public WeaponInstance(WeaponData data)
        {
            Data = data;
            Parameter = new WeaponParameter(this);
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

        public WeaponStatus Status => Data.LevelManager.GetStatus(Level);

        public event Action<bool> OnActiveChanged;

        public event Action<int> OnLevelChanged
        {
            add => LevelManager.OnLevelChanged += value;
            remove => LevelManager.OnLevelChanged -= value;
        }

        public void Activation(IActor owner)
        {
            _gameObject = GameObject.Instantiate(Data.Prefab, owner.transform.position, Quaternion.identity);
            _gameObject.transform.SetParent(owner.transform);
            Parameter.Actor = owner;
            _gameObject.Initialize(this);
            OnActiveChanged?.Invoke(true);
        }

        public void Deactivation()
        {
            Parameter.Actor = null;
            GameObject.Destroy(_gameObject);
            _gameObject = null;
            OnActiveChanged?.Invoke(false);
        }
    }
}