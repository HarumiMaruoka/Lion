using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataBase : MonoBehaviour
{
    private static CharacterDataBase _current = null;
    public static CharacterDataBase Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
        Initialize();
    }

    private void OnDestroy()
    {
        _current = null;
    }

    [SerializeField]
    private CharacterSpeciesData[] _characterSpeciesData;

    private Dictionary<int, CharacterSpeciesData> _idToData = new Dictionary<int, CharacterSpeciesData>();
    private Dictionary<string, CharacterSpeciesData> _nameToData = new Dictionary<string, CharacterSpeciesData>();

    private void Initialize()
    {
        foreach (var item in _characterSpeciesData)
        {
            if (!_idToData.TryAdd(item.ID, item))
            {
                Debug.LogError($"{item.ToString()}: CharacterIDが重複しています。");
            }
            if (!_nameToData.TryAdd(item.Name, item))
            {
                Debug.LogError($"{item.ToString()}: Character Nameが重複しています。");
            }
        }
    }

    public CharacterSpeciesData GetRaceCharacterData(int id)
    {
        if (_idToData.TryGetValue(id, out CharacterSpeciesData result))
        {
            return result;
        }
        Debug.Log($"ID {id} is Missing.");
        return null;
    }
    public CharacterSpeciesData GetRaceCharacterData(string name)
    {
        if (_nameToData.TryGetValue(name, out CharacterSpeciesData result))
        {
            return result;
        }
        Debug.Log($"{name} is Missing.");
        return null;
    }
}