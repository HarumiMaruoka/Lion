using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Weapon
{
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
                $"PhysicalPower: {PhysicalPower:.0}\n" +
                $"MagicPower: {MagicPower:.0}\n" +
                $"Range: {Range:.0}\n" +
                $"Size: {Size:.0}\n" +
                $"Duration: {Duration:.0}\n" +
                $"AttackSpeed: {AttackSpeed:.0}\n" +
                $"Amount: {Amount:0}";
        }
    }
}