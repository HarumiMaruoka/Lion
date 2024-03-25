using UnityEngine;

public class AxeSpawner : WeaponBase
{
    [SerializeField]
    private Axe _axePrefab;

    public override WeaponType WeaponType => WeaponType.Axe;

    public override string ToString() => "Axe";

    protected override void Spawn()
    {
        var instance = Instantiate(_axePrefab, transform);
        instance.Initialize(_equippedActor, TotalStatus);
    }
}