using System;
using UnityEngine;

public class CookingFoodInventoryWindow : WindowBase
{
    [SerializeField]
    private CookingController _controller;
    [SerializeField]
    private CookingFoodInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;
    [SerializeField]
    private EatWindowController _eatWindowController;

    public Action<int> OnSelected; // 渡される引数は CookingFoodID

    public void Initialize(CookingFoodDataBase foodDataBase) // CookingFoodInventoryの初期化が完了してから呼び出す。
    {
        OnSelected += _eatWindowController.EatFoodRequest;
        foreach (var element in foodDataBase.Data)
        {
            var instance = Instantiate(_elementPrefab, _elementParent);
            instance.Initialize(element);
            instance.OnSelected += OnSelected;
        }
    }
}