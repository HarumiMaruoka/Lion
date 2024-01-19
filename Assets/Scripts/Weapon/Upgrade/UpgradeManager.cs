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

    public event Action<WeaponBase> OnSelectedChanged;

    public void ChangeUpgradeTarget(WeaponType weaponType)
    {
        _requestItemCount.Clear();
        foreach (var i in OnChangedRequestItemCount) { i.Value.Invoke(0); }

        var weapon = WeaponManager.Current.GetWeapon(weaponType);
        if (weapon != null)
        {
            _selected = weapon;
            _targetLevel = weapon.CurrentLevel;
            TargetLevelChanged?.Invoke(_targetLevel);
        }
        else
        {
            weapon = null;
        }
        OnSelectedChanged?.Invoke(_selected);
    }

    private Dictionary<ItemData, int> _requestItemCount = new Dictionary<ItemData, int>();
    public Dictionary<ItemData, Action<int>> OnChangedRequestItemCount = new Dictionary<ItemData, Action<int>>();

    public IReadOnlyDictionary<ItemData, int> RequestItemCount => _requestItemCount;

    private int _targetLevel;
    public int TargetLevel => _targetLevel;

    public event Action<int> TargetLevelChanged;

    public void TargetLevelUpRequest()
    {
        if (!CanTargetLevelUp) return;
        _targetLevel++;
        TargetLevelChanged?.Invoke(_targetLevel);

        var currentLevelUpgradeCost = _selected.UpgradeCosts[_targetLevel];
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
        TargetLevelChanged?.Invoke(_targetLevel);

        var currentLevelUpgradeCost = _selected.UpgradeCosts[_targetLevel];
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
        ChangeUpgradeTarget(_selected.WeaponType);
    }
}