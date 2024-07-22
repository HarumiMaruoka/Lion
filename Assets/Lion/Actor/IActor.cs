using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;

public interface IActor : IGemCollector, IGoldCollector
{
    GameObject gameObject { get; }
    void Heal(float amount);
    void Damage(float amount);
}
