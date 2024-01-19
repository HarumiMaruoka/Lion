using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HolyWater : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private float _damageInterval;

    private Dictionary<EnemyController, Coroutine> _activeCoroutine = new Dictionary<EnemyController, Coroutine>();
    private CancellationTokenSource _cancellationOnDestroy = new CancellationTokenSource();

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    private WeaponStatus _status;
    private float _elapsed = 0f;

    public void Initialize(WeaponStatus status)
    {
        _status = status;
    }

    private void Update()
    {
        _elapsed += Time.deltaTime * TimeScale;
        if (_elapsed > _lifeTime) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _cancellationOnDestroy.Cancel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemy))
        {
            var coroutine = StartCoroutine(AttackAsync(enemy, _cancellationOnDestroy.Token));
            _activeCoroutine.TryAdd(enemy, coroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyController enemy))
        {
            if (_activeCoroutine.TryGetValue(enemy, out var coroutine))
            {
                StopCoroutine(coroutine);
            }
            _activeCoroutine.Remove(enemy);
        }
    }

    private IEnumerator AttackAsync(EnemyController enemy, CancellationToken token)
    {
        var elapsed = 0f;
        enemy.Damage(_status.AttackPower);
        while (!token.IsCancellationRequested)
        {
            elapsed += Time.deltaTime * TimeScale;

            if (elapsed > _damageInterval)
            {
                elapsed -= _damageInterval;
                enemy.Damage(_status.AttackPower);
            }
            yield return null;
        }
    }
}