using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public struct Status : IStatus
    {
        public float HP;
        public float Speed;
        public float BattlePower => HP + Speed;

        public float MoveSpeed => 3f + Speed * 0.03f;

        public void LoadExpSheet(string[] row)
        {
            HP = float.Parse(row[2]);
            Speed = float.Parse(row[3]);
        }

        public void LoadItemSheet(string[] row)
        {
            HP = float.Parse(row[1]);
            Speed = float.Parse(row[2]);
        }

        public static Status operator +(Status a, Status b)
        {
            return new Status()
            {
                HP = a.HP + b.HP,
                Speed = a.Speed + b.Speed,
            };
        }

        public override string ToString()
        {
            return
                $"HP: {HP}\n" +
                $"Speed: {Speed}";
        }
    }
}