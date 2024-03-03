using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public interface ISkill
{
    string Name { get; }
    string Description { get; }
    IEnumerator RunEffectAsync(CancellationToken token = default);
}