using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;

public interface IActor : IGemCollector, IGoldCollector
{
    GameObject gameObject { get; }
    public void Heal(float amount);
    public void Damage(float amount);
}
