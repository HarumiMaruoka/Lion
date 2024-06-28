using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public struct AllyStatus : IStatus
    {
        public float HP;
        public float MP;
        public float Attack;
        public float Defense;
        public float Speed;
        public float Range;
        public float Luck;

        public float MoveSpeed => 3f + Speed * 0.03f;

        public void ExpCsvLoad(string[] row)
        {
            HP = float.Parse(row[2]);
            MP = float.Parse(row[3]);
            Attack = float.Parse(row[4]);
            Defense = float.Parse(row[5]);
            Speed = float.Parse(row[6]);
            Range = float.Parse(row[7]);
            Luck = float.Parse(row[8]);
        }

        public void ItemCsvLoad(string[] row)
        {
            HP = float.Parse(row[1]);
            MP = float.Parse(row[2]);
            Attack = float.Parse(row[3]);
            Defense = float.Parse(row[4]);
            Speed = float.Parse(row[5]);
            Range = float.Parse(row[6]);
            Luck = float.Parse(row[7]);
        }

        public static AllyStatus operator +(AllyStatus a, AllyStatus b)
        {
            return new AllyStatus()
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
    }
}
