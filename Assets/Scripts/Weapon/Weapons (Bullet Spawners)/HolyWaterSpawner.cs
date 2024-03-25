using UnityEngine;

public class HolyWaterSpawner : WeaponBase
{
    [SerializeField]
    private HolyWaterThrowAnimation _holyWaterPrefab;

    public override string ToString() => "Holy Water";

    protected override void Spawn()
    {
        var spawnPos = GetRandomSpawnPosition(_spawnTopLeft, _spawnBottomRight);
        var holyWaterThrowAnimation = Instantiate(_holyWaterPrefab, spawnPos, Quaternion.identity, transform);

        var windowTopLeft = GameWindowArea.Current.TopLeft;
        var windowBottomRight = GameWindowArea.Current.BottomRight;

        var targetPos = GetRandomSpawnPosition(windowTopLeft, windowBottomRight);
        holyWaterThrowAnimation.Initialize(_equippedActor, TotalStatus, targetPos);
    }

    [SerializeField]
    private Transform _spawnTopLeft;
    [SerializeField]
    private Transform _spawnBottomRight;

    public override WeaponType WeaponType => WeaponType.HolyWater;

    private Vector2 GetRandomSpawnPosition(Transform topLeft, Transform bottomRight)
    {
        var minX = topLeft.position.x;
        var maxX = bottomRight.position.x;

        var minY = bottomRight.position.y;
        var maxY = topLeft.position.y;

        var randomX = UnityEngine.Random.Range(minX, maxX);
        var randomY = UnityEngine.Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
}