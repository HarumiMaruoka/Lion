using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public struct AllyStatus : IStatus
    {
        public float HP;
        public float MP;
        public float AttackPower;
        public float Defense;
        public float Speed;
        public float Range;
        public float Luck;
        public int AvailableMinionsCount; // 使役可能なミニオンの数

        public float BattlePower => HP + AttackPower + Defense + Speed + Range + Luck;
        public float MoveSpeed => 3f + Speed * 0.03f;

        public void LoadExpSheet(string[] row)
        {
            HP = float.Parse(row[2]);
            MP = float.Parse(row[3]);
            AttackPower = float.Parse(row[4]);
            Defense = float.Parse(row[5]);
            Speed = float.Parse(row[6]);
            Range = float.Parse(row[7]);
            Luck = float.Parse(row[8]);
            AvailableMinionsCount = int.Parse(row[9]);
        }

        public void LoadItemSheet(string[] row)
        {
            HP = float.Parse(row[1]);
            MP = float.Parse(row[2]);
            AttackPower = float.Parse(row[3]);
            Defense = float.Parse(row[4]);
            Speed = float.Parse(row[5]);
            Range = float.Parse(row[6]);
            Luck = float.Parse(row[7]);
            AvailableMinionsCount = int.Parse(row[8]);
        }

        public static AllyStatus operator +(AllyStatus a, AllyStatus b)
        {
            return new AllyStatus()
            {
                HP = a.HP + b.HP,
                MP = a.MP + b.MP,
                AttackPower = a.AttackPower + b.AttackPower,
                Defense = a.Defense + b.Defense,
                Speed = a.Speed + b.Speed,
                Range = a.Range + b.Range,
                Luck = a.Luck + b.Luck,
                AvailableMinionsCount = a.AvailableMinionsCount + b.AvailableMinionsCount
            };
        }

        public override string ToString()
        {
            return
                $"HP: {HP}\n" +
                $"MP: {MP}\n" +
                $"Attack: {AttackPower}\n" +
                $"Defense: {Defense}\n" +
                $"Speed: {Speed}\n" +
                $"Range: {Range}\n" +
                $"Luck: {Luck}\n" +
                $"Minion Count: {AvailableMinionsCount}";
        }
    }
}
