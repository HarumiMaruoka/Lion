using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _current = null;
    public static UpgradeManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;

        foreach (var item in ItemManager.Current.ItemData)
        {
            OnChangedRequestItemCount.Add(item, default);
        }
    }

    private void OnDestroy()
    {
        _current = null;
    }

    private WeaponBase _selected; // アップグレード対象
    public WeaponBase Selected => _selected;

    public event Action<WeaponBase> OnUpgradeTargetChanged;

    public void ChangeUpgradeTarget(WeaponBase weapon)
    {
        _requestItemCount.Clear();
        foreach (var item in OnChangedRequestItemCount)
        {
            item.Value.Invoke(0);
        }

        _selected = weapon;
        if (_selected)
        {
            _targetLevel = weapon.CurrentLevel;
            OnTargetLevelChanged?.Invoke(_targetLevel);
        }
        OnUpgradeTargetChanged?.Invoke(_selected);
    }

    private Dictionary<ItemData, int> _requestItemCount = new Dictionary<ItemData, int>();
    public Dictionary<ItemData, Action<int>> OnChangedRequestItemCount = new Dictionary<ItemData, Action<int>>();

    public IReadOnlyDictionary<ItemData, int> RequestItemCount => _requestItemCount;

    private int _targetLevel;
    public int TargetLevel => _targetLevel;

    public event Action<int> OnTargetLevelChanged;

    public void TargetLevelUpRequest()
    {
        if (!CanTargetLevelUp) return;
        _targetLevel++;
        OnTargetLevelChanged?.Invoke(_targetLevel);

        var currentLevelUpgradeCost = _selected.UpgradeCosts;
        foreach (var cost in currentLevelUpgradeCost)
        {
            var item = ItemManager.Current.GetItemData(cost.ItemID);
            if (!_requestItemCount.ContainsKey(item))
            {
                _requestItemCount.Add(item, 0);
            }

            _requestItemCount[item] += cost.Amount;
            OnChangedRequestItemCount[item]?.Invoke(_requestItemCount[item]);
        }
    }

    public void TargetLevelDownRequest()
    {
        if (!CanTargetLevelDown) return;
        _targetLevel--;
        OnTargetLevelChanged?.Invoke(_targetLevel);

        var currentLevelUpgradeCost = _selected.UpgradeCosts;
        foreach (var cost in currentLevelUpgradeCost)
        {
            var item = ItemManager.Current.GetItemData(cost.ItemID);
            if (!_requestItemCount.ContainsKey(item))
            {
                _requestItemCount.Add(item, 0);
            }

            _requestItemCount[item] -= cost.Amount;
            OnChangedRequestItemCount[item]?.Invoke(_requestItemCount[item]);
        }
    }

    private bool CanTargetLevelUp
    {
        get
        {
            if (!_selected)
            {
                Debug.Log("_selected is null");
                return false;
            }
            if (TargetLevel == _selected.UpgradeCosts.Length - 1)
            {
                Debug.Log("選択可能範囲を超えることはできません。");
                return false;
            }
            return true;
        }
    }

    private bool CanTargetLevelDown
    {
        get
        {
            if (!_selected)
            {
                Debug.Log("_selected is null");
                return false;
            }
            if (_selected.CurrentLevel == _targetLevel)
            {
                Debug.Log("現在のレベル以下にすることはできません。");
                return false;
            }
            return true;
        }
    }

    public bool IsUpgradable
    {
        get
        {
            foreach (var request in _requestItemCount)
            {
                var requestCount = request.Value;
                var inventoryCount = ItemInventory.Instance.GetItemCount(request.Key);

                if (requestCount > inventoryCount)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void ApplyUpgradeLevel()
    {
        if (!IsUpgradable) return;

        foreach (var request in _requestItemCount)
        {
            ItemInventory.Instance.UseItem(request.Key, request.Value);
        }

        _selected.UpgradeRequest(_targetLevel);
        ChangeUpgradeTarget(_selected);
    }

    [SerializeField]
    private UpgradeInputData[] _upgradeInputData;

    private Dictionary<WeaponType, ActorStatus[]> _upgradePlayerStatus = new Dictionary<WeaponType, ActorStatus[]>();
    private Dictionary<WeaponType, WeaponStatus[]> _upgradeWeaponStatus = new Dictionary<WeaponType, WeaponStatus[]>();
    private Dictionary<WeaponType, UpgradeCost[][]> _upgradeCosts = new Dictionary<WeaponType, UpgradeCost[][]>();

    private bool _isInitializedUpgradeData = false;

    private void InitializeUpgradeData()
    {
        foreach (var inputData in _upgradeInputData)
        {
            var weaponType = inputData.WeaponType;
            if (!_upgradePlayerStatus.ContainsKey(weaponType) ||
                !_upgradeWeaponStatus.ContainsKey(weaponType) ||
                !_upgradeCosts.ContainsKey(weaponType))
            {

                string[][] playerStatusCsvString = TextAssetToCsv(inputData.PlayerStatusData, 1);
                var upgradePlayerStatus = new ActorStatus[playerStatusCsvString.Length];
                for (int i = 0; i < playerStatusCsvString.Length; i++)
                {
                    upgradePlayerStatus[i] = ActorStatus.Parse(playerStatusCsvString[i]);
                }

                string[][] weaponStatusCsvString = TextAssetToCsv(inputData.WeaponStatusData, 1);
                var upgradeWeaponStatus = new WeaponStatus[weaponStatusCsvString.Length];
                for (int i = 0; i < weaponStatusCsvString.Length; i++)
                {
                    upgradeWeaponStatus[i] = WeaponStatus.Parse(weaponStatusCsvString[i]);
                }

                string[][] costCsvString = TextAssetToCsv(inputData.UpgradeCostData, 1);
                var upgradeCosts = new UpgradeCost[costCsvString.Length][];
                for (int i = 0; i < costCsvString.Length; i++)
                {
                    upgradeCosts[i] = UpgradeCost.Parse(costCsvString[i]);
                }

                _upgradePlayerStatus.Add(weaponType, upgradePlayerStatus);
                _upgradeWeaponStatus.Add(weaponType, upgradeWeaponStatus);
                _upgradeCosts.Add(weaponType, upgradeCosts);
            }
            else
            {
                Debug.LogError("キーが重複しています。");
            }
        }
        _isInitializedUpgradeData = true;
    }

    public ActorStatus RequestPlayerStatus(int level, WeaponType weaponType)
    {
        if (!_isInitializedUpgradeData) InitializeUpgradeData();
        return _upgradePlayerStatus[weaponType][level];
    }

    public WeaponStatus RequestWeaponStatus(int level, WeaponType weaponType)
    {
        if (!_isInitializedUpgradeData) InitializeUpgradeData();
        return _upgradeWeaponStatus[weaponType][level];
    }

    public UpgradeCost[] RequestUpgradeCosts(int level, WeaponType weaponType)
    {
        if (!_isInitializedUpgradeData) InitializeUpgradeData();
        return _upgradeCosts[weaponType][level];
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
}

[Serializable]
public struct UpgradeInputData
{
    [SerializeField]
    private TextAsset _playerStatusData;
    [SerializeField]
    private TextAsset _weaponStatusData;
    [SerializeField]
    private TextAsset _upgradeCostData;
    [SerializeField]
    private WeaponType _weaponType;

    public TextAsset PlayerStatusData => _playerStatusData;
    public TextAsset WeaponStatusData => _weaponStatusData;
    public TextAsset UpgradeCostData => _upgradeCostData;
    public WeaponType WeaponType => _weaponType;
}