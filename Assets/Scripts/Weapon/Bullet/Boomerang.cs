using System;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField]
    private float _initialSpeed;
    [SerializeField]
    private float _moveSpeed;

    private Vector2 _forward;

    private float _speed;
    private float _zDegree = 0f;
    private float _rotateSpeed = 600f;

    private Vector2 _position;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    private WeaponStatus _status;
    private IActor _equippedActor;

    public void Initialize(IActor equippedActor, WeaponStatus status)
    {
        _equippedActor = equippedActor;
        _status = status;
    }

    private void Start()
    {
        _forward = GetRandomVector().normalized;
        _speed = _initialSpeed;
        _position = transform.position;
    }

    private void Update()
    {
        // 移動処理
        _speed -= Time.deltaTime * _moveSpeed * TimeScale;

        _position += Time.deltaTime * _forward * _speed * TimeScale;
        transform.position = _position;

        // 回転処理
        _zDegree += Time.deltaTime * _rotateSpeed * TimeScale;
        transform.rotation = Quaternion.Euler(0f, 0f, _zDegree);

        if (!this.IsInGameZone()) // GameZoneの外に出たら破棄。
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemy))
        {
            enemy.Damage(_equippedActor, _status.AttackPower);
            return;
        }
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
            return;
        }
    }

    private Vector2 GetRandomVector()
    {
        var x = UnityEngine.Random.Range(-1f, 1f);
        var y = UnityEngine.Random.Range(-1f, 1f);
        return new Vector2(x, y);
    }
}