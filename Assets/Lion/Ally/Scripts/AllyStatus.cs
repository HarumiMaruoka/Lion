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

        public int Level { get; private set; }

        public void LoadStatusFromCsv(string[] csv)
        {
            Level = int.Parse(csv[0]);
            HP = float.Parse(csv[1]);
            MP = float.Parse(csv[2]);
            AttackPower = float.Parse(csv[3]);
            Defense = float.Parse(csv[4]);
            Speed = float.Parse(csv[5]);
            Range = float.Parse(csv[6]);
            Luck = float.Parse(csv[7]);
            AvailableMinionsCount = int.Parse(csv[8]);
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
