using System;
using System.Collections.Generic;
using UnityEngine;

public class CookingFoodDataBase : MonoBehaviour
{
    #region Singleton
    private static CookingFoodDataBase _current = null;
    public static CookingFoodDataBase Current => _current;

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
    #endregion

    [SerializeField]
    private CookingFoodData[] _data;
    public IEnumerable<CookingFoodData> Data => _data;

    private Dictionary<int, CookingFoodData> _idToData = new Dictionary<int, CookingFoodData>();
    private Dictionary<string, CookingFoodData> _nameToData = new Dictionary<string, CookingFoodData>();

    public CookingFoodData GetData(int id) => _idToData[id];
    public CookingFoodData GetData(string name) => _nameToData[name];

    public void Initialize()
    {
        foreach (var element in _data)
        {
            if (!_idToData.TryAdd(element.ID, element))
            {
                Debug.LogWarning($"ID: {element.ID}, Name: {element.Name} が重複しています。");
            }
            if (!_nameToData.TryAdd(element.Name, element))
            {
                Debug.LogWarning($"ID: {element.ID}, Name: {element.Name} が重複しています。");
            }
        }
    }
}