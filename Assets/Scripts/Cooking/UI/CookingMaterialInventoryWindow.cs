using System;
using UnityEngine;

public class CookingMaterialInventoryWindow : MonoBehaviour
{
    [SerializeField]
    private CookingController _controller;
    [SerializeField]
    private CookingMaterialInventoryWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    public Action<int> OnSelected; // “n‚³‚ê‚éˆø”‚Í CookingMaterialID

    public void Initialize(CookingMaterialDataBase materialDataBase) // CookingMaterialInventory‚Ì‰Šú‰»‚ªŠ®—¹‚µ‚Ä‚©‚çŒÄ‚Ño‚·B
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