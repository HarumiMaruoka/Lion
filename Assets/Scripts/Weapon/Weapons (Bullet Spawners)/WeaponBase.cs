using System;
using System.Collections;
using System.Numerics;
using System.Threading;
using Unity.VisualScripting;
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

    private bool _isShowedTryGetWeaponStatusWarning = false;

    public WeaponStatus WeaponStatus
    {
        get
        {
            if (TryGetWeaponStatus(out WeaponStatus status)) return status;
            else
            {
                if (!_isShowedTryGetWeaponStatusWarning) // 警告は一度だけ出す。
                {
                    _isShowedTryGetWeaponStatusWarning = true;
                    Debug.LogWarning("The data retrieval has failed, so the assigned values in the inspector will be used.");
                }
                return _weaponStatus;
            }
        }
    }

    protected float TimeScale => GameSpeedManager.Instance.TimeScale;

    public WeaponStatus TotalStatus => WeaponStatus + Player.WeaponStatus;

    private IEnumerator _mainRoutine = null;

    public void Activate(Transform parent)
    {
        Debug.Log("Activate");
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

    public void Inactivate()
    {
        if (_mainRoutine != null)
        {
            StopCoroutine(_mainRoutine);
        }
        else
        {
            Debug.Log("_mainRoutine is null.");
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
    [SerializeField]
    private TextAsset _upgradePlayerStatusCsv;
    [SerializeField]
    private TextAsset _upgradeWeaponStatusCsv;
    [SerializeField]
    private TextAsset _upgradeCostCsv;

    private int _currentLevel; // Levelと配列のIndexは同義とする。
    public int CurrentLevel => _currentLevel;

    private PlayerStatus[] _upgradePlayerStatus; // Levelを渡すとPlayerStatusを取得できる。
    private WeaponStatus[] _upgradeWeaponStatus; // Levelを渡すとWeaponStatusを取得できる。
    private UpgradeCost[][] _upgradeCosts;       // Levelを渡すとUpgradeCostをまとめて取得できる。

    public PlayerStatus[] UpgradePlayerStatus => _upgradePlayerStatus;
    public WeaponStatus[] UpgradeWeaponStatus => _upgradeWeaponStatus;
    public UpgradeCost[][] UpgradeCosts => _upgradeCosts;

    private bool _isUpgradeInitialized = false;

    public void UpgradeInitialize()
    {
        string[][] playerStatusCsvString = TextAssetToCsv(_upgradePlayerStatusCsv, 1);
        _upgradePlayerStatus = new PlayerStatus[playerStatusCsvString.Length];
        for (int i = 0; i < playerStatusCsvString.Length; i++)
        {
            _upgradePlayerStatus[i] = PlayerStatus.Parse(playerStatusCsvString[i]);
        }

        string[][] weaponStatusCsvString = TextAssetToCsv(_upgradeWeaponStatusCsv, 1);
        _upgradeWeaponStatus = new WeaponStatus[weaponStatusCsvString.Length];
        for (int i = 0; i < weaponStatusCsvString.Length; i++)
        {
            _upgradeWeaponStatus[i] = WeaponStatus.Parse(weaponStatusCsvString[i]);
        }

        string[][] costCsvString = TextAssetToCsv(_upgradeCostCsv, 1);
        _upgradeCosts = new UpgradeCost[costCsvString.Length][];
        for (int i = 0; i < costCsvString.Length; i++)
        {
            _upgradeCosts[i] = UpgradeCost.Parse(costCsvString[i]);
        }

        _isUpgradeInitialized = true;
    }

    public void UpgradeRequest(int level)
    {
        _currentLevel = level;
    }

    public static string[][] TextAssetToCsv(TextAsset csvTextAsset, int ignoreRowCount = 0)
    {
        string[] costRows = csvTextAsset.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int costRowCount = costRows.Length;
        string[][] costColumns = new string[costRowCount][];

        for (int i = ignoreRowCount; i < costRowCount; i++)
        {
            costColumns[i] = costRows[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        return costColumns[ignoreRowCount..];
    }

    public bool TryGetWeaponStatus(out WeaponStatus weaponStatus)
    {
        if (!_isUpgradeInitialized) UpgradeInitialize();

        var level = _currentLevel;
        if (_upgradeWeaponStatus == null || level < 0 || level >= _upgradeWeaponStatus.Length)
        {
            weaponStatus = default;
            return false;
        }
        weaponStatus = _upgradeWeaponStatus[level];
        return true;
    }
    #endregion
}