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
    public Dictionary<int, Action<int>> OnCountChanged = new Dictionary<int, Action<int>>();

    public void Initialize(CookingFoodDataBase foodDataBase)
    {
        foreach (var element in foodDataBase.Data)
        {
            if (!_collection.TryAdd(element.ID, 0)) // 一旦最初の所持数は全部0で。
            {
                Debug.Log($"ID: {element.ID}, Name: {element.Name} が重複しています。");
            }
            OnCountChanged?.Add(element.ID, null);
        }

    }

    public int GetCount(int cookingFoodID)
    {
        return _collection[cookingFoodID];
    }

    public void Add(int cookingFoodID, int amount = 1)
    {
        var result = (_collection[cookingFoodID] += amount);
        OnCountChanged[cookingFoodID]?.Invoke(result);
    }

    public void Use(int cookingFoodID, int amount = 1)
    {
        var result = (_collection[cookingFoodID] -= amount);
        OnCountChanged[cookingFoodID]?.Invoke(result);
    }

    public bool TryUse(int cookingFoodID, int amount = 1)
    {
        var count = _collection[cookingFoodID];
        if (count - amount < 0) return false;

        var result = (_collection[cookingFoodID] -= amount);
        OnCountChanged[cookingFoodID]?.Invoke(result);

        return true;
    }
}