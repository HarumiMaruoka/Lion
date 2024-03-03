using System;
using System.Collections.Generic;
using UnityEngine;

public class CookingMaterialInventory
{
    private static CookingMaterialInventory _instance = null;
    public static CookingMaterialInventory Instance => _instance ??= new CookingMaterialInventory();

    // KeyはCookingMaterialIDを表現する。
    // Valueは所持数を表現する。
    private Dictionary<int, int> _collection = new Dictionary<int, int>();

    public Dictionary<int, Action<int>> OnCountChanged = new Dictionary<int, Action<int>>();

    public void Initialize(CookingMaterialDataBase dataBase)
    {
        foreach (var element in dataBase.Data)
        {
            if (!_collection.TryAdd(element.ID, 0))
            {
                Debug.Log($"ID: {element.ID}, Name: {element.Name} が重複しています。");
            }
            else // いずれかのDictionaryで重複していなければ 他のDictionaryはチェックしなくてもよい。
            {
                OnCountChanged.Add(element.ID, null);
            }
        }
    }

    public int GetCount(int cookingMaterialID) => _collection[cookingMaterialID];

    public int this[int cookingMaterialID]
    {
        get
        {
            return _collection[cookingMaterialID];
        }
    }

    public void Add(int cookingMaterialID, int amount = 1)
    {
        var result = (_collection[cookingMaterialID] += amount);
        OnCountChanged[cookingMaterialID]?.Invoke(result);
    }

    public void Use(int cookingMaterialID, int amount = 1)
    {
        var result = (_collection[cookingMaterialID] -= amount);
        OnCountChanged[cookingMaterialID]?.Invoke(result);
    }

    public bool TryUse(int cookingMaterialID, int amount = 1)
    {
        var count = _collection[cookingMaterialID];
        if (count - amount < 0) return false;
        var result = (_collection[cookingMaterialID] -= amount);
        OnCountChanged[cookingMaterialID]?.Invoke(result);
        return true;
    }

    public bool VerifyInventory(CookingFoodData makableFood)
    {
        if (!TryUse(makableFood.CookingMaterialID1))
        {
            var material = CookingMaterialDataBase.Current.GetData(makableFood.CookingMaterialID1);
            Debug.Log($"{material.Name} が足りません。");
            return false;
        }

        if (!TryUse(makableFood.CookingMaterialID2))
        {
            Add(makableFood.CookingMaterialID1);

            var material = CookingMaterialDataBase.Current.GetData(makableFood.CookingMaterialID2);
            Debug.Log($"{material.Name} が足りません。");

            return false;
        }

        if (!TryUse(makableFood.CookingMaterialID3))
        {
            Add(makableFood.CookingMaterialID1);
            Add(makableFood.CookingMaterialID2);

            var material = CookingMaterialDataBase.Current.GetData(makableFood.CookingMaterialID3);
            Debug.Log($"{material.Name} が足りません。");

            return false;
        }

        Add(makableFood.CookingMaterialID1);
        Add(makableFood.CookingMaterialID2);
        Add(makableFood.CookingMaterialID3);

        return true;
    }
}