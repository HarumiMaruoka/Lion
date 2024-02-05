using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectWindow : MonoBehaviour
{
    [SerializeField]
    private WeaponSelectWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private HashSet<WeaponSelectWindowElement> _actives = new HashSet<WeaponSelectWindowElement>();
    private Stack<WeaponSelectWindowElement> _inactives = new Stack<WeaponSelectWindowElement>();

    public event Action<WeaponBase> OnSelected;

    private void OnEnable()
    {
        Show();
    }

    private void OnDisable()
    {
        Hide();
    }

    public void Show()
    {
        var weaponCollection = WeaponInventory.Current.WeaponCollection;
        foreach (var item in weaponCollection)
        {
            WeaponSelectWindowElement elem;
            if (_inactives.Count != 0)
            {
                elem = _inactives.Pop();
            }
            else
            {
                elem = Instantiate(_elementPrefab, _elementParent);
            }

            elem.Weapon = item;

            elem.gameObject.SetActive(true);
            elem.OnSelected += OnSelectedCharacter;
            _actives.Add(elem);
        }
    }

    public void Hide()
    {
        foreach (var active in _actives)
        {
            active.gameObject.SetActive(false);
            active.OnSelected -= OnSelectedCharacter;
            _inactives.Push(active);
        }
        _actives.Clear();
    }

    private void OnSelectedCharacter(WeaponBase selectedWeapon)
    {
        OnSelected?.Invoke(selectedWeapon);
        gameObject.SetActive(false);
    }
}