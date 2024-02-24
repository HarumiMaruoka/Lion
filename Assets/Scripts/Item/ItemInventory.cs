using System;
using System.Collections.Generic;

public class ItemInventory
{
    private static ItemInventory _instance;
    public static ItemInventory Instance => _instance ??= new ItemInventory();
    private ItemInventory() { }

    // 第一型パラメータはItemID, 第二型パラメータが所有するアイテムの数。
    private Dictionary<int, int> _itemCountData = new Dictionary<int, int>();

    public Dictionary<int, Action<int>> OnChangedItemCount = new Dictionary<int, Action<int>>();

    public void Initialize() // ItemManagerが初期化された後に呼び出してください。
    {
        var items = ItemManager.Current.ItemData;
        foreach (var item in items)
        {
            _itemCountData.Add(item.ID, 0);
            OnChangedItemCount.Add(item.ID, default);
        }
    }

    public void ChangeItemCount(ItemData item, int count)
    {
        _itemCountData[item.ID] += count;
        OnChangedItemCount[item.ID]?.Invoke(_itemCountData[item.ID]);
    }

    public int GetItemCount(int itemID)
    {
        return _itemCountData[itemID];
    }

    public int GetItemCount(ItemData item)
    {
        return _itemCountData[item.ID];
    }

    public void UseItem(ItemData item, int amount)
    {
        _itemCountData[item.ID] -= amount;
        OnChangedItemCount[item.ID]?.Invoke(_itemCountData[item.ID]);
    }

    public bool TryUseItem(int itemID, int amount)
    {
        if (_itemCountData[itemID] - amount < 0) return false;

        _itemCountData[itemID] -= amount;
        OnChangedItemCount[itemID]?.Invoke(_itemCountData[itemID]);

        return true;
    }
}