using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory
{
    private static CharacterInventory _instance = null;
    public static CharacterInventory Instance => _instance ??= new CharacterInventory();
    private CharacterInventory() { }

    private HashSet<CharacterIndividualData> _collection = new HashSet<CharacterIndividualData>();
    public IReadOnlyCollection<CharacterIndividualData> Collection => _collection;

    private readonly int Capacity = 20;

    /// <summary>
    /// V‚µ‚¢ŒÂ‘Ì‚ğæ“¾‚µ‚½‚Æ‚«B
    /// </summary>
    /// <param name="speciesData"> í‘°î•ñ </param>
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
    /// Šù‚É‘¶İ‚·‚éŒÂ‘Ì‚ğæ“¾‚µ‚½‚Æ‚«B
    /// </summary>
    /// <param name="speciesData"> ŒÂ‘Ìî•ñ </param>
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