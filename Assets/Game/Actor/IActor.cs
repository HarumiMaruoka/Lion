using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;

public interface IActor
{
    Transform transform { get; }
    void CollectGold(int amount);
    void CollectGem(int amount);
    GameObject gameObject { get; }
    void Heal(float amount);
    void Damage(float amount);
    float PhysicalPower { get; }
    float MagicPower { get; }
}
