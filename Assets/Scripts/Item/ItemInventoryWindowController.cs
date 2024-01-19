using System;
using UnityEngine;

public class ItemInventoryWindowController : MonoBehaviour
{
    [SerializeField]
    private ItemInventoryWindowElement _inventoryWindowElementPrefab;
    [SerializeField]
    private Transform _elementsParent;

    private void Start()
    {
        var itemData = ItemManager.Current.ItemData;
        var inventory = ItemInventory.Instance;

        foreach (var data in itemData)
        {
            var element = Instantiate(_inventoryWindowElementPrefab, _elementsParent);
            element.Initialize(data, inventory);
        }
    }
}