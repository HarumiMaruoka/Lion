using System.Collections;
using System.Threading;
using UnityEngine;

public class KnifeSpawner : WeaponBase
{
    [SerializeField]
    private Knife _knifePrefab;

    private Vector3 _lastPosition;
    private Vector3 _lastDir = new Vector2(1f, 0f); // 初期値は右

    protected override void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        // 移動していれば方向、座標を更新する。
        if (_lastPosition != transform.position)
        {
            _lastDir = transform.position - _lastPosition;
            _lastPosition = transform.position;
        }
    }

    // 方向を記録する関数を作成する。

    protected override void Spawn()
    {
        var randomPoint = GetRandomPoint();
        var knife = Instantiate(_knifePrefab, transform.position + randomPoint, Quaternion.identity, transform);
        knife.Initialize(_equippedActor, TotalStatus, _lastDir);
    }

    [SerializeField]
    private float _maxRandoRadius;

    public override WeaponType WeaponType => WeaponType.Knife;

    private Vector3 GetRandomPoint()
    {
        var randoRadius = UnityEngine.Random.Range(0f, _maxRandoRadius);
        var randomRadian = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

        var randomX = Mathf.Cos(randomRadian) * randoRadius;
        var randomY = Mathf.Sin(randomRadian) * randoRadius;

        return new Vector2(randomX, randomY);
    }

    public override string ToString() => "Knife";
}