using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _battlePower;
    [SerializeField]
    private DropItemData[] _dropItemData;
    [SerializeField]
    private int _defeatRequiredEnemyCount; // 倒さないといけない敵の数。

    private int _defeatedEnemyCount = 0; // 倒した敵の数。

    public string Name => _name;
    public float BattlePower => _battlePower;
    public DropItemData[] DropItemData => _dropItemData;

    public int DefeatRequiredEnemyCount => _defeatRequiredEnemyCount;
    public int DefeatedEnemyCount => _defeatedEnemyCount;

    /// <summary> このボスと戦うことができるかどうか。 </summary>
    public bool IsChallengeable => _defeatedEnemyCount >= _defeatRequiredEnemyCount;
    public bool IsPlayerStronger => PlayerController.Current.BattlePower > BattlePower;

    public event Action<int> OnDeadEnemyCountChanged;

    private void Start()
    {
        EnemyManager.Current.OnEnemyDead += OnEnemyDead;
    }

    private void OnEnemyDead(int deadEnemyID, EnemyController deadEnemy)
    {
        // 既に必要数倒している場合は無効。
        if (_defeatedEnemyCount > _defeatRequiredEnemyCount)
        {
            return;
        }

        _defeatedEnemyCount++;
        OnDeadEnemyCountChanged?.Invoke(_defeatedEnemyCount);
    }
}

[Serializable]
public struct DropItemData
{
    public int ItemID;
    public int Amount;
}