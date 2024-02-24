using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private WeaponData _data;
    [SerializeField]
    protected PlayerStatus _playerStatus;
    [SerializeField]
    protected WeaponStatus _weaponStatus;
    [SerializeField]
    protected float _baseSpawnInterval; // 基本となるスポーン間隔。
    [SerializeField]
    protected float _baseCooldownTime; // 基本となる待機時間。

    private CancellationTokenSource _cancellationOnDestroy = new CancellationTokenSource();

    protected PlayerController Player => PlayerController.Current;

    public WeaponData Data => _data;
    public abstract WeaponType WeaponType { get; }
    public string WeaponName => ToString();
    public abstract override string ToString();

    protected float TimeScale => GameSpeedManager.Instance.TimeScale;

    public WeaponStatus TotalStatus => WeaponStatus + Player.WeaponStatus;

    private IEnumerator _mainRoutine = null;

    public virtual void Activate(Transform parent)
    {
        if (_mainRoutine == null)
        {
            _mainRoutine = MainRoutine();
        }

        if (parent)
        {
            transform.SetParent(parent);
            transform.position = parent.position;
        }
        else
        {
            Debug.Log("parent is null.");
        }

        StartCoroutine(_mainRoutine);
    }

    public virtual void Inactivate()
    {
        if (_mainRoutine != null)
        {
            StopCoroutine(_mainRoutine);
        }

        transform.SetParent(WeaponInventory.Current.WeaponParent);
    }


    protected virtual void Start() { }

    private void OnDestroy()
    {
        _cancellationOnDestroy.Cancel();
    }

    private IEnumerator MainRoutine(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            yield return SpawnAsync(TotalStatus, token);
            yield return WaitCooldownAsync(token);
        }
    }

    protected virtual IEnumerator SpawnAsync(WeaponStatus status, CancellationToken token)
    {
        for (int i = 0; i < status.Amount; i++)
        {
            Spawn();
            float t = 0f;
            while (t < _baseSpawnInterval &&
                   !token.IsCancellationRequested)
            {
                t += Time.deltaTime * TimeScale;
                yield return null;
            }
        }
    }

    protected virtual IEnumerator WaitCooldownAsync(CancellationToken token)
    {
        float t = 0f;
        while (t < _baseCooldownTime &&
               !token.IsCancellationRequested)
        {
            t += Time.deltaTime * TimeScale * TotalStatus.Cooldown;
            yield return null;
        }
    }

    protected virtual void Spawn()
    {
        Debug.Log("Not yet implemented.");
    }

    #region Upgrade
    private int _currentLevel;
    public int CurrentLevel => _currentLevel;

    public PlayerStatus PlayerStatus => UpgradeManager.Current.RequestPlayerStatus(_currentLevel, WeaponType);
    public WeaponStatus WeaponStatus => UpgradeManager.Current.RequestWeaponStatus(_currentLevel, WeaponType);
    public UpgradeCost[] UpgradeCosts => UpgradeManager.Current.RequestUpgradeCosts(_currentLevel, WeaponType);

    public void UpgradeRequest(int level)
    {
        _currentLevel = level;
    }
    #endregion
}