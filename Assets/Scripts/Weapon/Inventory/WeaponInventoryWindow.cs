using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryWindow : MonoBehaviour
{
    private static WeaponInventoryWindow _current = null;
    public static WeaponInventoryWindow Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private WeaponInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private HashSet<WeaponInventoryWindowElement> _actives = new HashSet<WeaponInventoryWindowElement>();
    private Queue<WeaponInventoryWindowElement> _inactives = new Queue<WeaponInventoryWindowElement>();

    public event Action<WeaponBase> OnSelectedWeapon;
    public event Action OnShowed;
    public event Action OnHided;

    public void Show()
    {
        gameObject.SetActive(true);
        var weaponCollection = WeaponInventory.Current.WeaponCollection;
        foreach (var item in weaponCollection)
        {
            WeaponInventoryWindowElement elem;
            if (_inactives.Count != 0)
            {
                elem = _inactives.Dequeue();
                elem.transform.SetAsLastSibling(); // Layout Groupの都合でヒエラルキーの一番下に持ってくる。
            }
            else
            {
                elem = Instantiate(_elementPrefab, _elementParent);
            }

            elem.Weapon = item;

            elem.gameObject.SetActive(true);
            elem.OnSelected += OnSelectedWeapon;
            _actives.Add(elem);
        }

        OnShowed?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        foreach (var active in _actives)
        {
            active.gameObject.SetActive(false);
            active.OnSelected -= OnSelectedWeapon;
            _inactives.Enqueue(active);
        }
        _actives.Clear();

        OnHided?.Invoke();
    }
}