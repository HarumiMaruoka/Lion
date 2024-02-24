using System;
using System.Collections.Generic;
using UnityEngine;

public class CookingMaterialDataBase : MonoBehaviour
{
    #region Singleton
    private static CookingMaterialDataBase _current = null;
    public static CookingMaterialDataBase Current => _current;

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
    private CookingMaterialData[] _data;

    private Dictionary<int, CookingMaterialData> _idToData = new Dictionary<int, CookingMaterialData>();
    private Dictionary<string, CookingMaterialData> _nameToData = new Dictionary<string, CookingMaterialData>();

    public IEnumerable<CookingMaterialData> Data => _data;

    public CookingMaterialData GetData(int id) => _idToData[id];
    public bool TryGetData(int selectedMaterialID, out CookingMaterialData result) =>
        _idToData.TryGetValue(selectedMaterialID, out result);
    public CookingMaterialData GetData(string name) => _nameToData[name];

    [SerializeField]
    private CookingMaterialInventoryWindow _inventoryWindow;

    public void Initialize()
    {
        // 入力を基にデータを作成。
        foreach (var element in _data)
        {
            if (!_idToData.TryAdd(element.ID, element))
            {
                Debug.LogWarning($"ID: {element.ID}, Name: {element.Name} IDが重複しています。");
            }
            if (!_nameToData.TryAdd(element.Name, element))
            {
                Debug.LogWarning($"ID: {element.ID}, Name: {element.Name} 名前が重複しています。");
            }
        }

        // UIの初期化。
        if (_inventoryWindow) _inventoryWindow.Initialize();
    }
}