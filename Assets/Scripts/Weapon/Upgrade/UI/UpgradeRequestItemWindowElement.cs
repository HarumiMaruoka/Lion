using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRequestItemWindowElement : MonoBehaviour
{
    [SerializeField]
    private Text _itemNameText;
    [SerializeField]
    private Text _inventoryItemCountText;
    [SerializeField]
    private Text _requestItemCountText;

    private ItemData _itemData;

    private ItemInventory Inventory => ItemInventory.Instance;
    private UpgradeManager UpgradeManager => UpgradeManager.Current;

    public void Initialize(ItemData item)
    {
        _itemNameText.text = item.Name;
        ApplyItemCountText(Inventory.GetItemCount(item));
        Inventory.OnChangedItemCount[item] += ApplyItemCountText;

        UpgradeManager.RequestItemCount.TryGetValue(item, out int requestItemCount);
        ApplyRequestItemCountText(requestItemCount);
        UpgradeManager.OnChangedRequestItemCount[item] += ApplyRequestItemCountText;
    }

    private void ApplyItemCountText(int count)
    {
        _inventoryItemCountText.text = $"èäéùêî: {count}";
    }

    private void ApplyRequestItemCountText(int count)
    {
        if (count <= 0) this.gameObject.SetActive(false);
        else this.gameObject.SetActive(true);

        _requestItemCountText.text = $"óvãÅêî: {count}";
    }
}