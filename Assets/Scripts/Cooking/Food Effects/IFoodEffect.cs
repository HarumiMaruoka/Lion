using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public interface IFoodEffect
{
    IEnumerator RunAsync(CancellationToken token);
}