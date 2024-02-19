using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterInventory
{
    private static CharacterInventory _instance = null;
    public static CharacterInventory Instance => _instance ??= new CharacterInventory();
    private CharacterInventory() { }

    private HashSet<CharacterIndividualData> _collection = new HashSet<CharacterIndividualData>();
    public IReadOnlyCollection<CharacterIndividualData> Collection => _collection;

    private readonly int Capacity = 20;

    private List<WeaponBase> _characterEquippedWeapons = new List<WeaponBase>();

    public IEnumerable<WeaponBase> CharacterEquippedWeapons
    {
        get
        {
            _characterEquippedWeapons.Clear();

            // キャラクターが装備している武器をリストに登録していく。
            foreach (var character in _collection)
            {
                if (character == null) continue;
                foreach (var weapon in character.EquippedWeapons)
                {
                    if (!weapon) continue;
                    _characterEquippedWeapons.Add(weapon);
                }
            }

            return _characterEquippedWeapons;
        }
    }

    /// <summary>
    /// 新しい個体を取得したとき。
    /// </summary>
    /// <param name="speciesData"> 種族情報 </param>
    public void AddCharacter(CharacterSpeciesData speciesData)
    {
        if (_collection.Count >= Capacity)
        {
            // Debug.Log("Inventory capacity exceeded, cannot collect more characters.");
            return;
        }

        var instance = new CharacterIndividualData(speciesData, 0);
        _collection.Add(instance);
    }

    /// <summary>
    /// 既に存在する個体を取得したとき。
    /// </summary>
    /// <param name="speciesData"> 個体情報 </param>
    public void AddCharacter(CharacterIndividualData individualData)
    {
        if (individualData == null)
        {
            Debug.LogWarning("Null is invalid.");
            return;
        }

        if (_collection.Count >= Capacity)
        {
            Debug.Log("Inventory capacity exceeded, cannot collect more characters.");
            return;
        }

        _collection.Add(individualData);
    }

    public void RemoveCharacter(CharacterIndividualData individualData)
    {
        _collection.Remove(individualData);
    }
}