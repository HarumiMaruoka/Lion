using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;

namespace Lion.Damage
{
    public interface IDamagable
    {
        void PhysicalDamage(float physicalPower, IActor actor);
        void MagicDamage(float magicPower, IActor actor);
        event Action OnDead;
    }
}