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

    private int _activeEnemyCount = 0;

    private Dictionary<int, HashSet<EnemyController>> _activeEnemiesDictionary = new Dictionary<int, HashSet<EnemyController>>();
    private Dictionary<int, Stack<EnemyController>> _inactiveEnemiesDictionary = new Dictionary<int, Stack<EnemyController>>();

    private HashSet<EnemyController> _activeEnemies = new HashSet<EnemyController>();
    public IEnumerable<EnemyController> ActiveEnemies => _activeEnemies;

    public void CreateEnemy(int enemyID, Vector3 position)
    {
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

        if (_activeEnemyCount > _capacity) return;

        EnemyController enemy = null;
        if (inactiveEnemies.Count == 0)
        {
            var enemyPrefab = _enemyPrefabs[enemyID];
            enemy = Instantiate(enemyPrefab, transform);
        }
        else
        {
            enemy = inactiveEnemies.Pop();
        }

        activeEnemies.Add(enemy);
        enemy.gameObject.SetActive(true);
        enemy.Initialize(enemyID, position);
        enemy.OnDead += OnDeadEnemy;

        _activeEnemies.Add(enemy);

        _activeEnemyCount++;
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

        _activeEnemyCount--;
    }
}