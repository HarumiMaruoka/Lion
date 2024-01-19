using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnInterval = 3f;
    [SerializeField]
    private int _spawnEnemyID;

    private CancellationTokenSource _cancellationOnDestroy = new CancellationTokenSource();

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    private void Start()
    {
        StartCoroutine(SpawnAsync(_cancellationOnDestroy.Token));
    }

    private void OnDestroy()
    {
        _cancellationOnDestroy.Cancel();
    }

    private float _elapsed = 0f;

    private IEnumerator SpawnAsync(CancellationToken token)
    {
        var interval = GetRandomInterval();
        while (!token.IsCancellationRequested)
        {
            _elapsed += Time.deltaTime * TimeScale;
            if (_elapsed > interval)
            {
                _elapsed -= _spawnInterval;
                // 生成間隔にランダム性を持たせる為、生成する度にインターバルを変更する。
                interval = GetRandomInterval();
                EnemyManager.Current.CreateEnemy(_spawnEnemyID, transform.position);
            }
            yield return null;
        }
    }

    private float GetRandomInterval()
    {
        return UnityEngine.Random.Range(_spawnInterval - 0.5f, _spawnInterval + 0.5f);
    }
}