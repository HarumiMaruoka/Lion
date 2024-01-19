using System;
using System.Collections.Generic;

public class ItemInventory
{
    private static ItemInventory _instance;
    public static ItemInventory Instance => _instance ??= new ItemInventory();
    private ItemInventory() { }

    // 第一型パラメータはItemID, 第二型パラメータが所有するアイテムの数。
    public Dictionary<ItemData, int> _itemCountData = new Dictionary<ItemData, int>();

    public Dictionary<ItemData, Action<int>> OnChangedItemCount = new Dictionary<ItemData, Action<int>>();

    public void Initialize() // ItemManagerが初期化された後に呼び出してください。
    {
        var items = ItemManager.Current.ItemData;
        foreach (var item in items)
        {
            _itemCountData.Add(item, 0);
            OnChangedItemCount.Add(item, default);
        }
    }

    public void ChangeItemCount(ItemData item, int count)
    {
        _itemCountData[item] += count;
        OnChangedItemCount[item]?.Invoke(_itemCountData[item]);
    }

    public int GetItemCount(ItemData item)
    {
        return _itemCountData[item];
    }

    public void UseItem(ItemData itemID, int amount)
    {
        _itemCountData[itemID] -= amount;
        OnChangedItemCount[itemID]?.Invoke(_itemCountData[itemID]);
    }
}