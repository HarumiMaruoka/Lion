using UnityEngine;

public class HolyBibleSpawner : WeaponBase
{
    [SerializeField]
    private HolyBible _holyBiblePrefab;

    private int _spawnCounter = 0;

    public override WeaponType WeaponType => WeaponType.HolyBible;

    public override string ToString() => "Holy Bible";

    protected override void Spawn()
    {
        var instance = Instantiate(_holyBiblePrefab, transform);
        instance.Initialize(TotalStatus, Player.transform, TotalStatus.Amount, _spawnCounter, _baseSpawnInterval);
        _spawnCounter++;
    }
}