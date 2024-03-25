using Character;
using System;
using UnityEngine;

public interface IActor
{
    string Name { get; }
    int Level { get; }
    ActorStatus Status { get; }
    Ability Ability { get; }
}