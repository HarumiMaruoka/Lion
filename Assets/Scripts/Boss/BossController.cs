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
    private int _defeatRequiredEnemyCount; // “|‚³‚È‚¢‚Æ‚¢‚¯‚È‚¢“G‚Ì”B

    private int _defeatedEnemyCount = 0; // “|‚µ‚½“G‚Ì”B

    public string Name => _name;
    public float BattlePower => _battlePower;
    public DropItemData[] DropItemData => _dropItemData;

    public int DefeatRequiredEnemyCount => _defeatRequiredEnemyCount;
    public int DefeatedEnemyCount => _defeatedEnemyCount;

    /// <summary> ‚±‚Ìƒ{ƒX‚Æí‚¤‚±‚Æ‚ª‚Å‚«‚é‚©‚Ç‚¤‚©B </summary>
    public bool IsChallengeable => _defeatedEnemyCount >= _defeatRequiredEnemyCount;
    public bool IsPlayerStronger => PlayerController.Current.PlayerStatus.Sum > BattlePower;

    public event Action<int> OnDeadEnemyCountChanged;

    private void Start()
    {
        EnemyManager.Current.OnEnemyDead += OnEnemyDead;
    }

    private void OnEnemyDead(int deadEnemyID, EnemyController deadEnemy)
    {
        // Šù‚É•K—v”“|‚µ‚Ä‚¢‚éê‡‚Í–³ŒøB
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