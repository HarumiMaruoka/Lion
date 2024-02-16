using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ThunderboltSpawner : WeaponBase
{
    [SerializeField]
    private Vector2 _size;
    [SerializeField]
    private int _capacity;

    [SerializeField]
    private ThunderboltVFX _thunderboltVFXPrefab;

    private Collider2D[] _overlapBoxResult;
    private HashSet<EnemyController> _enemies = new HashSet<EnemyController>();

    public override WeaponType WeaponType => WeaponType.Thunderbolt;

    public override string ToString() => "Thunderbolt";

    protected override IEnumerator SpawnAsync(WeaponStatus status, CancellationToken token)
    {
        Spawn();
        return default;
    }

    [SerializeField]
    private LayerMask _layerMask;

    protected override void Spawn()
    {
        if (_overlapBoxResult == null) _overlapBoxResult = new Collider2D[_capacity];

        var point = transform.position;
        var overlapBoxResultCount = Physics2D.OverlapBoxNonAlloc(point, _size, 0f, _overlapBoxResult, _layerMask);

        _enemies.Clear();
        for (int i = 0; i < overlapBoxResultCount; i++)
        {
            var col = _overlapBoxResult[i];

            if (col.TryGetComponent(out EnemyController enemy))
            {
                _enemies.Add(enemy);
            }
        }

        for (int i = 0; i < TotalStatus.Amount; i++)
        {
            if (_enemies.Count == 0) return;
            var index = UnityEngine.Random.Range(0, _enemies.Count);
            var enemy = _enemies.ElementAt(index);
            enemy.OnDead += RemoveEnemy;
            enemy.Damage(TotalStatus.AttackPower);
            enemy.OnDead -= RemoveEnemy;
            Instantiate(_thunderboltVFXPrefab, enemy.transform.position, Quaternion.identity);
        }

        void RemoveEnemy(int enemyID, EnemyController enemy)
        {
            _enemies.Remove(enemy);
        }
    }

#if UNITY_EDITOR
    [SerializeField]
    private Color _gizmoColor = Color.red;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(transform.position, _size);
    }
#endif
}