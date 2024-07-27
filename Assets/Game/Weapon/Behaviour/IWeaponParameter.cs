using System;
using UnityEngine;

namespace Lion.Weapon
{
    public interface IWeaponParameter
    {
        public IActor Actor { get; } // 武器使用者
        float PhysicalPower { get; }
        float MagicPower { get; }
        float Range { get; }
        float Size { get; }
        float Duration { get; }
        float AttackSpeed { get; }
        int Amount { get; }
    }
}