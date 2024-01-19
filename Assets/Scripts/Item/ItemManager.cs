using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ItemManager : MonoBehaviour
{
    #region Singleton
    private static ItemManager _current;
    public static ItemManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;

        Initialize();
    }

    private void OnDestroy()
    {
        _current = null;
    }
    #endregion

    [SerializeField]
    private DroppedItem _itemPrefab;
    [SerializeField]
    private ItemData[] _itemData;

    private Dictionary<int, ItemData> _itemIDToData = new Dictionary<int, ItemData>();
    private Dictionary<string, ItemData> _itemNameToData = new Dictionary<string, ItemData>();

    public ItemData GetItemData(int itemID)
    {
        return _itemIDToData[itemID];
    }

    public ItemData GetItemData(string itemName)
    {
        return _itemNameToData[itemName];
    }

    public void Initialize()
    {
        foreach (var data in _itemData)
        {
            _itemNameToData.Add(data.Name, data);
            _itemIDToData.Add(data.ID, data);
        }
    }

    public IEnumerable<ItemData> ItemData => _itemData;

    public int ItemTypeCount => _itemData.Length;

    private HashSet<DroppedItem> _activeItems = new HashSet<DroppedItem>();
    private Stack<DroppedItem> _inactiveItems = new Stack<DroppedItem>();

    public void DropItem(Vector3 position, int itemID, float probability) // probabilityは確率を表現する。0.0から1.0で判断する。0の方が出にくく、1の方が出やすい。
    {
        // 指定されたItemIDのItemが見つからなければ警告を出してリターン。
        if (!_itemIDToData.TryGetValue(itemID, out ItemData data))
        {
            Debug.Log($"Not found item data. ItemID: {itemID}");
            return;
        }

        var random = UnityEngine.Random.Range(0f, 1f);

        if (probability > random) return; // 確率を下回ればアイテムを生成しない。


        // Create Item.
        DroppedItem item = null;
        if (_inactiveItems.Count == 0)
            item = Instantiate(_itemPrefab, this.transform);
        else
            item = _inactiveItems.Pop();

        // Activate Item
        _activeItems.Add(item);
        item.gameObject.SetActive(true);
        item.Initialize(position, data);
        item.OnDead += DeleteItem;
    }

    private void DeleteItem(DroppedItem item)
    {
        item.OnDead -= DeleteItem;
        item.gameObject.SetActive(false);
        _activeItems.Remove(item);
        _inactiveItems.Push(item);
    }
}