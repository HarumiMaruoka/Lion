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

    public struct WeaponStatus : IStatus
    {
        public int Level { get; private set; }
        public float PhysicalPower { get; private set; }
        public float MagicPower { get; private set; }
        public float Range { get; private set; }
        public float Size { get; private set; }
        public float Duration { get; private set; }
        public float AttackSpeed { get; private set; }
        public int Amount { get; private set; }

        public void LoadStatusFromCsv(string[] csv)
        {
            Level = int.Parse(csv[0]);
            PhysicalPower = float.Parse(csv[1]);
            MagicPower = float.Parse(csv[2]);
            Range = float.Parse(csv[3]);
            Size = float.Parse(csv[4]);
            Duration = float.Parse(csv[5]);
            AttackSpeed = float.Parse(csv[6]);
                Amount = (int)float.Parse(csv[7]);
        }

        public static WeaponStatus operator +(WeaponStatus a, WeaponStatus b)
        {
            return new WeaponStatus
            {
                PhysicalPower = a.PhysicalPower + b.PhysicalPower,
                MagicPower = a.MagicPower + b.MagicPower,
                Range = a.Range + b.Range,
                Size = a.Size + b.Size,
                Duration = a.Duration + b.Duration,
                AttackSpeed = a.AttackSpeed + b.AttackSpeed,
                Amount = a.Amount + b.Amount
            };
        }

        public override string ToString()
        {
            return
                $"PhysicalPower: {PhysicalPower}\n" +
                $"MagicPower: {MagicPower}\n" +
                $"Range: {Range}\n" +
                $"Size: {Size}\n" +
                $"Duration: {Duration}\n" +
                $"AttackSpeed: {AttackSpeed}\n" +
                $"Amount: {Amount}";
        }
    }
}