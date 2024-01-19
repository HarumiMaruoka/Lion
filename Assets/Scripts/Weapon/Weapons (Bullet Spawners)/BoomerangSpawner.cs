using System;
using UnityEngine;

public class BoomerangSpawner : WeaponBase
{
    [SerializeField]
    private Boomerang _boomerangPrefab;

    public override WeaponType WeaponType => WeaponType.Boomerang;

    public override string ToString() => "Boomerang";

    protected override void Spawn()
    {
        var instance = Instantiate(_boomerangPrefab, transform);
        instance.Initialize(TotalStatus);
    }
}