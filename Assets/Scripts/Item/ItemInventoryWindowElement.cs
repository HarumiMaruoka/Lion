using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryWindowElement : MonoBehaviour
{
    [SerializeField]
    private Text _itemNameText;
    [SerializeField]
    private Text _itemCountText;

    public void Initialize(ItemData item, ItemInventory inventory)
    {
        _itemNameText.text = item.Name;
        ApplyItemCountText(inventory.GetItemCount(item));
        inventory.OnChangedItemCount[item.ID] += ApplyItemCountText;
    }

    private void ApplyItemCountText(int count)
    {
        if (count <= 0) this.gameObject.SetActive(false);
        else this.gameObject.SetActive(true);

        _itemCountText.text = $"x{count}";
    }
}