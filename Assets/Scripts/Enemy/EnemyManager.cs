using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _current;
    public static EnemyManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private EnemyController[] _enemyPrefabs;

    [SerializeField]
    private int _capacity;

    private Dictionary<int, HashSet<EnemyController>> _activeEnemiesDictionary = new Dictionary<int, HashSet<EnemyController>>();
    private Dictionary<int, Stack<EnemyController>> _inactiveEnemiesDictionary = new Dictionary<int, Stack<EnemyController>>();

    private HashSet<EnemyController> _activeEnemies = new HashSet<EnemyController>();
    private HashSet<EnemyController> _inactiveEnemies = new HashSet<EnemyController>();
    public IEnumerable<EnemyController> ActiveEnemies => _activeEnemies;

    private Action<int, EnemyController> _onEnemyDeadBuffer;
    public event Action<int, EnemyController> OnEnemyDead
    {
        add
        {
            _onEnemyDeadBuffer += value;
            foreach (var elem in _activeEnemies) elem.OnDead += value;
            foreach (var elem in _inactiveEnemies) elem.OnDead += value;
        }
        remove
        {
            _onEnemyDeadBuffer -= value;
            foreach (var elem in _activeEnemies) elem.OnDead -= value;
            foreach (var elem in _inactiveEnemies) elem.OnDead -= value;
        }
    }

    public void CreateEnemy(int enemyID, Vector3 position)
    {
        // ディクショナリに要素が無ければ追加する。
        if (!_activeEnemiesDictionary.TryGetValue(enemyID, out HashSet<EnemyController> activeEnemies))
        {
            activeEnemies = new HashSet<EnemyController>();
            _activeEnemiesDictionary.Add(enemyID, activeEnemies);
        }
        if (!_inactiveEnemiesDictionary.TryGetValue(enemyID, out Stack<EnemyController> inactiveEnemies))
        {
            inactiveEnemies = new Stack<EnemyController>();
            _inactiveEnemiesDictionary.Add(enemyID, inactiveEnemies);
        }

        // キャパシティを超える場合は無効。
        if (_activeEnemies.Count > _capacity) return;

        // 非アクティブなエネミーがいなければ生成する。
        EnemyController enemy = null;
        if (inactiveEnemies.Count == 0)
        {
            var enemyPrefab = _enemyPrefabs[enemyID];
            enemy = Instantiate(enemyPrefab, transform);
            enemy.OnDead += _onEnemyDeadBuffer;
        }
        // 非アクティブなエネミーがいれば、そのエネミーを利用する。
        else
        {
            enemy = inactiveEnemies.Pop();
        }

        activeEnemies.Add(enemy);
        enemy.gameObject.SetActive(true);
        enemy.Initialize(enemyID, position);
        enemy.OnDead += OnDeadEnemy;

        _activeEnemies.Add(enemy);
        _inactiveEnemies.Remove(enemy);
    }

    private void OnDeadEnemy(int enemyID, EnemyController enemy)
    {
        var activeEnemies = _activeEnemiesDictionary[enemyID];
        var inactiveEnemies = _inactiveEnemiesDictionary[enemyID];

        enemy.OnDead -= OnDeadEnemy;
        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        inactiveEnemies.Push(enemy);

        _activeEnemies.Remove(enemy);
        _inactiveEnemies.Add(enemy);
    }

    public EnemyController GetNearestEnemy(Vector2 position)
    {
        if (_activeEnemies.Count == 0)
        {
            return null;
        }

        EnemyController result = null;
        float minDistance = float.MaxValue;

        foreach (var enemy in _activeEnemies)
        {
            if (!result)
            {
                result = enemy;
            }
            else
            {
                var distance = ((Vector2)enemy.transform.position - position).sqrMagnitude;
                if (Mathf.Approximately(distance, 0f))
                {
                    continue;
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = enemy;
                }
            }
        }

        return result;
    }
}