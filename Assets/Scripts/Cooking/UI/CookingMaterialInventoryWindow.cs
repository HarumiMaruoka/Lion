using System;
using UnityEngine;

public class CookingMaterialInventoryWindow : WindowBase
{
    [SerializeField]
    private CookingController _controller;
    [SerializeField]
    private CookingMaterialInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    public Action<int> OnSelected; // 渡される引数は CookingMaterialID

    public void Initialize(CookingMaterialDataBase materialDataBase) // CookingMaterialInventoryの初期化が完了してから呼び出す。
    {
        OnSelected += _controller.SelectMaterial;
        foreach (var element in materialDataBase.Data)
        {
            var instance = Instantiate(_elementPrefab, _elementParent);
            instance.Initialize(element);
            instance.OnSelected += OnSelected;
        }
    }
}