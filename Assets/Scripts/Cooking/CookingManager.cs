using System;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    [SerializeField]
    private CookingFoodDataBase _foodDataBase;
    [SerializeField]
    private CookingMaterialDataBase _materialBase;

    private void Awake()
    {
        CookingFoodInventory.Instance.Initialize(_foodDataBase);
        CookingMaterialInventory.Instance.Initialize(_materialBase);
    }
}