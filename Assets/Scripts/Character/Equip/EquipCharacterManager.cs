using System;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Analytics;

public class EquipCharacterManager : MonoBehaviour
{
    private static EquipCharacterManager _current = null;
    public static EquipCharacterManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private CharacterBehaviour[] _equippedCharacters;

    public int EquippableCharacterCount => _equippedCharacters.Length;

    private List<WeaponBase> _characterEquippedWeapons = new List<WeaponBase>();

    public IEnumerable<WeaponBase> CharacterEquippedWeapons
    {
        get
        {
            _characterEquippedWeapons.Clear();

            foreach (var character in _equippedCharacters)
            {
                if (!character) continue;
                if (character.IndividualData == null) continue;

                foreach (var weapon in character.IndividualData.EquippedWeapons)
                {
                    if (!weapon) continue;
                    _characterEquippedWeapons.Add(weapon);
                }
            }

            return _characterEquippedWeapons;
        }
    }

    public void EquipCharacter(int index, CharacterIndividualData characterData)
    {
        if (index < 0 || index >= _equippedCharacters.Length)
        {
            Debug.LogError("Out of Range: The index is beyond the valid range.");
            return;
        }

        _equippedCharacters[index]?.IndividualData?.Unequip();
        _equippedCharacters[index].IndividualData = characterData;
        _equippedCharacters[index]?.IndividualData?.Equip(index);
    }

    public CharacterIndividualData GetEquippedCharacterData(int index)
    {
        if (index < 0 || index >= _equippedCharacters.Length)
        {
            Debug.LogError("Out of Range: The index is beyond the valid range.");
            return null;
        }

        return _equippedCharacters[index].IndividualData;
    }

    public CharacterBehaviour GetCharacterBehaviour(int index)
    {
        if (index < 0 || index >= _equippedCharacters.Length)
        {
            Debug.LogError("Out of Range: The index is beyond the valid range.");
            return null;
        }

        return _equippedCharacters[index];
    }
}