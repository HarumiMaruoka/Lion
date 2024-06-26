using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public struct Status : IStatus
    {
        public float Health;
        public float Speed;

        public float MoveSpeed => 3f + Speed * 0.03f;

        public void ExpCsvLoad(string[] row)
        {
            Health = float.Parse(row[2]);
            Speed = float.Parse(row[3]);
        }

        public void ItemCsvLoad(string[] row)
        {
            Health = float.Parse(row[1]);
            Speed = float.Parse(row[2]);
        }

        public static Status operator +(Status a, Status b)
        {
            return new Status()
            {
                Health = a.Health + b.Health,
                Speed = a.Speed + b.Speed,
            };
        }

        public override string ToString()
        {
            return $"Health: {Health}, Speed: {Speed}";
        }
    }
}