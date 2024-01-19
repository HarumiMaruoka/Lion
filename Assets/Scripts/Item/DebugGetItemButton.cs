using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugGetItemButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(GetItem);
    }

    [SerializeField]
    private int _count;

    private void GetItem()
    {
        var itemData = ItemManager.Current.ItemData;
        foreach (var item in itemData)
        {
            ItemInventory.Instance.ChangeItemCount(item, _count);
        }
    }
}