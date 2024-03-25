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

    public Action<int> OnSelected; // “n‚³‚ê‚éˆø”‚Í CookingFoodID

    public void Initialize(CookingFoodDataBase foodDataBase) // CookingFoodInventory‚Ì‰Šú‰»‚ªŠ®—¹‚µ‚Ä‚©‚çŒÄ‚Ño‚·B
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