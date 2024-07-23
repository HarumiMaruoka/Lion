using Lion.LevelManagement;
using System;
using UnityEngine;

namespace Lion.Player
{
    public struct PlayerStatus : IStatus
    {
        public float HP;
        public float Speed;

        public float BattlePower => HP + Speed;
        public float MoveSpeed => 3f + Speed * 0.03f;

        public int Level { get; private set; }

        public void LoadStatusFromCsv(string[] csv)
        {
            Level = int.Parse(csv[0]);
            HP = float.Parse(csv[1]);
            Speed = float.Parse(csv[2]);
        }

        public static PlayerStatus operator +(PlayerStatus a, PlayerStatus b)
        {
            return new PlayerStatus()
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