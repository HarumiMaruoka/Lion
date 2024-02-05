using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory
{
    private static CharacterInventory _instance = null;
    public static CharacterInventory Instance => _instance ??= new CharacterInventory();
    private CharacterInventory() { }

    private HashSet<IndividualCharacterData> _collection = new HashSet<IndividualCharacterData>();
    public IReadOnlyCollection<IndividualCharacterData> Collection => _collection;

    public void GetCharacter(RaceCharacterData raceCharacterData)
    {
        var instance = new IndividualCharacterData(raceCharacterData, 0);
        _collection.Add(instance);
    }
}