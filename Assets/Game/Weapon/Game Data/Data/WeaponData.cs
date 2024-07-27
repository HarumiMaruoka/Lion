using Lion.LevelManagement;
using Lion.Weapon.Behaviour;
using System;
using UnityEngine;

namespace Lion.Weapon
{
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public WeaponBehaviour Prefab { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public WeaponLevelManager LevelManager { get; private set; }

        public void Initialize()
        {
            LevelManager = new WeaponLevelManager(this);
        }
    }
}