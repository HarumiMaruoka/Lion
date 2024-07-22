using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Minion
{
    public struct MinionStatus : IStatus
    {
        public float HP;
        public float MP;
        public float Attack;
        public float Defense;
        public float Speed;
        public float Range;
        public float Luck;

        public float BattlePower => HP + Attack + Defense + Speed + Range + Luck;
        public float MoveSpeed => 3f + Speed * 0.03f;

        public int Level { get; private set; }

        public static MinionStatus operator +(MinionStatus a, MinionStatus b)
        {
            return new MinionStatus()
            {
                HP = a.HP + b.HP,
                MP = a.MP + b.MP,
                Attack = a.Attack + b.Attack,
                Defense = a.Defense + b.Defense,
                Speed = a.Speed + b.Speed,
                Range = a.Range + b.Range,
                Luck = a.Luck + b.Luck,
            };
        }

        public override string ToString()
        {
            return
                $"HP: {HP}\n" +
                $"MP: {MP}\n" +
                $"Attack: {Attack}\n" +
                $"Defense: {Defense}\n" +
                $"Speed: {Speed}\n" +
                $"Range: {Range}\n" +
                $"Luck: {Luck}";
        }

        public void LoadStatusFromCsv(string[] csv)
        {
            Level = int.Parse(csv[0]);
            HP = float.Parse(csv[1]);
            MP = float.Parse(csv[2]);
            Attack = float.Parse(csv[3]);
            Defense = float.Parse(csv[4]);
            Speed = float.Parse(csv[5]);
            Range = float.Parse(csv[6]);
            Luck = float.Parse(csv[7]);
        }
    }
}