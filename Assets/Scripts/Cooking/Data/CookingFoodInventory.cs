using System;
using System.Collections.Generic;
using UnityEngine;

public class CookingFoodInventory
{
    private static CookingFoodInventory _instance = null;
    public static CookingFoodInventory Instance => _instance ??= new CookingFoodInventory();

    // KeyはCookingFoodIDを表現する。
    // Valueは所持数を表現する。
    private Dictionary<int, int> _collection = new Dictionary<int, int>();

    public void Initialize(CookingFoodDataBase foodDataBase)
    {
        foreach (var element in foodDataBase.Data)
        {
            if (!_collection.TryAdd(element.ID, 0)) // 一旦最初の所持数は全部0で。
            {
                Debug.Log($"ID: {element.ID}, Name: {element.Name} が重複しています。");
            }
        }

    }

    public int this[int cookingMaterialID]
    {
        get
        {
            return _collection[cookingMaterialID];
        }
    }

    public void Add(int cookingFoodID) => _collection[cookingFoodID]++;
    public void Add(int cookingFoodID, int amount) => _collection[cookingFoodID] += amount;

    public void Use(int cookingFoodID) => _collection[cookingFoodID]--;
    public void Use(int cookingFoodID, int amount) => _collection[cookingFoodID] -= amount;

    public bool TryUse(int cookingFoodID)
    {
        var count = _collection[cookingFoodID];
        if (count <= 0) return false;
        _collection[cookingFoodID]--;
        return true;
    }

    public bool TryUse(int cookingFoodID, int amount)
    {
        var count = _collection[cookingFoodID];
        if (count - amount < 0) return false;
        _collection[cookingFoodID] -= amount;
        return true;
    }
}