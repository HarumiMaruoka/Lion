using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Singleton
    private static WeaponManager _current = null;
    public static WeaponManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
        InitializeDrop();
        InitializeUpgrade();
    }

    private void OnDestroy()
    {
        _current = null;
    }
    #endregion

    [SerializeField]
    private DroppedWeapon _droppedWeaponPrefab;
    [SerializeField]
    private Transform _droppedWeaponParent;
    [SerializeField]
    private WeaponData[] _weaponData;

    private Dictionary<int, WeaponData> _weaponTypeToData = new Dictionary<int, WeaponData>();

    public void InitializeDrop()
    {
        foreach (var data in _weaponData)
        {
            _weaponTypeToData.Add((int)data.WeaponPrefab.WeaponType, data);
        }
    }

    private HashSet<DroppedWeapon> _activeItems = new HashSet<DroppedWeapon>();
    private Stack<DroppedWeapon> _inactiveItems = new Stack<DroppedWeapon>();

    public void DropWeapon(Vector3 position, WeaponType weaponType, float probability) // probabilityは確率を表現する。0.0から1.0で判断する。0の方が出にくく、1の方が出やすい。
    {
        // 指定されたItemIDのItemが見つからなければ警告を出してリターン。
        if (!_weaponTypeToData.TryGetValue((int)weaponType, out WeaponData data))
        {
            Debug.Log($"Not found weapon data. ItemID: {weaponType}");
            return;
        }

        var random = UnityEngine.Random.Range(0f, 1f);

        if (probability > random) return; // 確率を下回ればアイテムを生成しない。


        // Create Item.
        DroppedWeapon weapon = null;
        if (_inactiveItems.Count == 0)
            weapon = Instantiate(_droppedWeaponPrefab, _droppedWeaponParent);
        else
            weapon = _inactiveItems.Pop();

        // Activate Item
        _activeItems.Add(weapon);
        weapon.gameObject.SetActive(true);
        weapon.Initialize(position, data);
        weapon.OnDead += DeleteItem;
    }

    private void DeleteItem(DroppedWeapon weapon)
    {
        weapon.OnDead -= DeleteItem;
        weapon.gameObject.SetActive(false);
        _activeItems.Remove(weapon);
        _inactiveItems.Push(weapon);
    }

    #region Initial Weapon
    [SerializeField]
    private WeaponBase[] _initialWeapons;

    private Dictionary<int, WeaponBase> _weapons = new Dictionary<int, WeaponBase>();

    public void InitializeUpgrade()
    {
        foreach (var weapon in _initialWeapons)
        {
            _weapons.Add((int)weapon.WeaponType, weapon);
        }
    }
    #endregion
}

[Serializable]
public enum WeaponType
{
    Knife = 0,
    Axe = 1,
    Thunderbolt = 2,
    Boomerang = 3,
    HolyWater = 4,
    HolyBible = 5,
    Garlic = 6,
}