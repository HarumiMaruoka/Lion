using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public interface ICookingEffect
{
    IEnumerator RunAsync(CancellationToken token);
}