using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory
{
    private static CharacterInventory _instance = null;
    public static CharacterInventory Instance => _instance ??= new CharacterInventory();
    private CharacterInventory() { }

    private HashSet<CharacterIndividualInfo> _collection = new HashSet<CharacterIndividualInfo>();
    public IReadOnlyCollection<CharacterIndividualInfo> Collection => _collection;

    public void GetCharacter(CharacterSpeciesInfo speciesInfo)
    {
        var instance = new CharacterIndividualInfo(speciesInfo, 0);
        _collection.Add(instance);
    }
}