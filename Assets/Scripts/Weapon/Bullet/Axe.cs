using System;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField]
    private int _maxHitCount;

    [SerializeField]
    private float _maxInitialSpeed;
    [SerializeField]
    private float _minInitialSpeed;

    private WeaponStatus _status;
    private IActor _equippedActor;

    public void Initialize(IActor equippedActor, WeaponStatus status)
    {
        _equippedActor = equippedActor;
        _status = status;
    }

    private void Start()
    {
        _initialPosition = transform.position;

        Vector2 dir = GetRandomDirection(-40f, 40f);
        float initialSpeed = UnityEngine.Random.Range(_minInitialSpeed, _maxInitialSpeed);

        _initialVelocityX = dir.x * initialSpeed;
        _initialVelocityY = dir.y * initialSpeed;

        var randomRot = UnityEngine.Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRot);
    }

    private Vector2 GetRandomDirection(float minDeg, float maxDeg)
    {
        float randomDeg = UnityEngine.Random.Range(minDeg, maxDeg);
        float randomRad = randomDeg * Mathf.Deg2Rad;

        float vectorX = Mathf.Sin(randomRad);
        float vectorY = Mathf.Cos(randomRad);

        return new Vector2(vectorX, vectorY);
    }

    private Vector3 _initialPosition;
    private float _gravity = 9.8f;

    private float _initialVelocityX;
    private float _initialVelocityY;

    private float _currentTime = 0f;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    void Update()
    {
        // 時間経過による位置の更新
        _currentTime += Time.deltaTime * TimeScale;

        // 新しい位置を計算
        float newX = _initialPosition.x + _initialVelocityX * _currentTime;
        float newY = _initialPosition.y + _initialVelocityY * _currentTime - 0.5f * _gravity * _currentTime * _currentTime;
        float newZ = _initialPosition.z;

        // 新しい位置にオブジェクトを移動
        transform.position = new Vector3(newX, newY, newZ);

        if (!this.IsInGameZone()) // GameZoneの外に出たら破棄。
        {
            Destroy(gameObject);
        }
    }

    private int _hitCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemy))
        {
            enemy.Damage(_equippedActor, _status.AttackPower);

            _hitCount++;
            if (_hitCount >= _maxHitCount)
            {
                Destroy(this.gameObject);
            }
            return;
        }
    }
}