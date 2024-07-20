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

        public ItemLevelManager LevelManager { get; private set; }

        public void Initialize()
        {
            var levelUpCost = $"Weapon_{ID}_ItemLevelUpCostTable";
            var status = $"Weapon_{ID}_ItemLevelStatusTable";
            LevelManager = ItemLevelManager.Create<WeaponStatus>(levelUpCost, status);
        }
    }

    public struct WeaponStatus : IStatus
    {
        public float PhysicalPower { get; private set; }
        public float MagicPower { get; private set; }
        public float Range { get; private set; }
        public float Size { get; private set; }
        public float Duration { get; private set; }
        public float AttackSpeed { get; private set; }

        public void LoadExpSheet(string[] row)
        {
            // 不要なので実装しない
        }

        public void LoadItemSheet(string[] row)
        {
            PhysicalPower = float.Parse(row[1]);
            MagicPower = float.Parse(row[2]);
            Range = float.Parse(row[3]);
            Size = float.Parse(row[4]);
            Duration = float.Parse(row[5]);
            AttackSpeed = float.Parse(row[6]);
        }
    }
}