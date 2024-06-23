using System;
using UnityEngine;

namespace Lion.Player
{
    public struct Status : IStatus
    {
        public float Health;
        public float Speed;

        public float MoveSpeed => 1f + Speed * 0.01f;

        public void ExpCsvLoad(string[] row)
        {
            Health = float.Parse(row[1]);
            Speed = float.Parse(row[2]);
        }

        public void ItemCsvLoad(string[] row)
        {
            Health = float.Parse(row[0]);
            Speed = float.Parse(row[1]);
        }

        public static Status operator +(Status a, Status b)
        {
            return new Status()
            {
                Health = a.Health + b.Health,
                Speed = a.Speed + b.Speed,
            };
        }
    }
}