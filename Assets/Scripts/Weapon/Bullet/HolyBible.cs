using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBible : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private float _radius = 3f;
    [SerializeField]
    private float _rotateSpeed = 8f;

    private Transform _centerTransform;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    private WeaponStatus _status;

    public void Initialize(WeaponStatus status, Transform centerTransform, int maxCount, int count, float spawnInterval)
    {
        _status = status;

        float proportion = (float)count / (float)maxCount; // 割り合いを出す。
        float initialRadian = 2f * Mathf.PI * proportion;

        _centerTransform = centerTransform;
        _radian = initialRadian;

        // 経過時間分の補完を行う。
        _radian += spawnInterval * count * _rotateSpeed;

        var offsetX = Mathf.Cos(_radian) * _radius;
        var offsetY = Mathf.Sin(_radian) * _radius;

        var offset = new Vector3(offsetX, offsetY);

        transform.position = _centerTransform.position + offset;
    }

    [SerializeField]
    private float _radian;

    private float _elapsed = 0f;

    private void Update()
    {
        // 回転処理
        _radian += Time.deltaTime * _rotateSpeed * TimeScale;

        var offsetX = Mathf.Cos(_radian) * _radius;
        var offsetY = Mathf.Sin(_radian) * _radius;

        var offset = new Vector3(offsetX, offsetY);

        transform.position = _centerTransform.position + offset;

        _elapsed += Time.deltaTime * TimeScale;
        if (_elapsed > _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private HashSet<EnemyController> _hits = new HashSet<EnemyController>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemy))
        {
            if (_hits.Contains(enemy)) return;
            else
            {
                enemy.Damage(_status.AttackPower);
                StartCoroutine(HitIntervalAsync(enemy));
            }
        }
    }

    [SerializeField]
    private float _hitInterval;
    private IEnumerator HitIntervalAsync(EnemyController hit)
    {
        _hits.Add(hit);
        for (float t = 0f; t < _hitInterval; t += Time.deltaTime * TimeScale)
        {
            yield return null;
        }
        _hits.Remove(hit);
    }
}