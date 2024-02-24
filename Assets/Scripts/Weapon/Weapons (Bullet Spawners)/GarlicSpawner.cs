using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GarlicSpawner : WeaponBase
{
    // List‚ÌÅ‰‚ÌCapacityB—Ìˆæ‚ÌÄŠm•Û–hŽ~—pB
    [SerializeField]
    private int _initialCapacity;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private SpriteRenderer _garlicSpriteRenderer;

    public override WeaponType WeaponType => WeaponType.Garlic;

    public override string ToString() => "Garlic";

    protected override void Start()
    {
        _enemies.Capacity = _initialCapacity;
    }

    public override void Activate(Transform parent)
    {
        base.Activate(parent);
        _garlicSpriteRenderer.enabled = true;
    }

    public override void Inactivate()
    {
        base.Inactivate();
        _garlicSpriteRenderer.enabled = false;
    }


    protected override IEnumerator SpawnAsync(WeaponStatus status, CancellationToken token)
    {
        float t = 0f;
        Spawn();
        while (t < _baseSpawnInterval &&
               !token.IsCancellationRequested)
        {
            t += Time.deltaTime * TimeScale * (TotalStatus.Cooldown + TotalStatus.Speed);
            yield return null;
        }
    }

    protected override IEnumerator WaitCooldownAsync(CancellationToken token)
    {
        return default; // ‚È‚É‚à‚µ‚È‚¢
    }

    [SerializeField]
    private LayerMask _layerMask;

    private List<EnemyController> _enemies = new List<EnemyController>();

    protected override void Spawn()
    {
        _enemies.Clear();
        _enemies.AddRange(EnemyManager.Current.ActiveEnemies);

        foreach (var enemy in _enemies)
        {
            var sqrDistance = (this.transform.position - enemy.transform.position).sqrMagnitude;
            if (sqrDistance < _radius * _radius)
            {
                enemy.Damage(TotalStatus.AttackPower * TotalStatus.Amount);
            }
        }
    }

#if UNITY_EDITOR
    [SerializeField]
    private Color _gizmoColor;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}