using System;
using System.Collections.Generic;
using UnityEngine;

public class StatsWindowController : MonoBehaviour
{
    [SerializeField]
    private ActorStatusWindow _actorStatusWindowPrefab;
    private HashSet<ActorStatusWindow> _activeActorStatusWindows = new();
    private Stack<ActorStatusWindow> _inactiveActorStatusWindows = new();

    [SerializeField]
    private WeaponStatusWindow _weaponStatusWindowPrefab;
    private HashSet<WeaponStatusWindow> _activeWeaponStatusWindows = new();
    private Stack<WeaponStatusWindow> _inactiveWeaponStatusWindows = new();

    [SerializeField]
    private Transform _prefabParent;

    public void Show()
    {
        Clear();

        // Show Player Status
        ShowActorStatus(PlayerController.Current);
        // Show Player Weapons Status
        foreach (var weapon in PlayerController.Current.PlayerWeapons)
            ShowWeaponStatus(weapon);

        foreach (var character in EquipCharacterManager.Current.EquippedCharacters)
        {
            if (character && character.IndividualData != null)
            {
                // Show Equipped Characters Status.
                ShowActorStatus(character.IndividualData);
                // Show Equipped Character Weapons Status.
                foreach (var weapon in character.IndividualData.EquippedWeapons)
                {
                    if (weapon) ShowWeaponStatus(weapon);
                }
            }
        }
    }

    private ActorStatusWindow ShowActorStatus(IActor actor)
    {
        ActorStatusWindow result;
        if (_inactiveActorStatusWindows.Count == 0)
        {
            result = Instantiate(_actorStatusWindowPrefab, _prefabParent);
        }
        else
        {
            result = _inactiveActorStatusWindows.Pop();
            result.gameObject.SetActive(true);
        }
        _activeActorStatusWindows.Add(result);
        result.Actor = actor;
        result.transform.SetAsLastSibling();
        return result;
    }

    private WeaponStatusWindow ShowWeaponStatus(WeaponBase weapon)
    {
        WeaponStatusWindow result;
        if (_inactiveWeaponStatusWindows.Count == 0)
        {
            result = Instantiate(_weaponStatusWindowPrefab, _prefabParent);
        }
        else
        {
            result = _inactiveWeaponStatusWindows.Pop();
            result.gameObject.SetActive(true);
        }
        _activeWeaponStatusWindows.Add(result);
        result.Weapon = weapon;
        result.transform.SetAsLastSibling();
        return result;
    }

    private void Clear()
    {
        // Clear Actor Status Windows.
        foreach (var elem in _activeActorStatusWindows)
        {
            elem.gameObject.SetActive(false);
            _inactiveActorStatusWindows.Push(elem);
        }
        _activeActorStatusWindows.Clear();

        // Clear Weapon Status Windows.
        foreach (var elem in _activeWeaponStatusWindows)
        {
            elem.gameObject.SetActive(false);
            _inactiveWeaponStatusWindows.Push(elem);
        }
        _activeWeaponStatusWindows.Clear();
    }
}