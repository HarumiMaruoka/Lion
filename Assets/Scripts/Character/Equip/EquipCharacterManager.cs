using System;
using UnityEngine;

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

    public void EquipCharacter(int index, CharacterIndividualData characterData)
    {
        if (index < 0 || index >= _equippedCharacters.Length)
        {
            Debug.LogError("Out of Range: The index is beyond the valid range.");
            return;
        }

        _equippedCharacters[index].IndividualData = characterData;
    }

    public CharacterIndividualData GetEquippedCharacter(int index)
    {
        if (index < 0 || index >= _equippedCharacters.Length)
        {
            Debug.LogError("Out of Range: The index is beyond the valid range.");
            return null;
        }

        return _equippedCharacters[index].IndividualData;
    }
}