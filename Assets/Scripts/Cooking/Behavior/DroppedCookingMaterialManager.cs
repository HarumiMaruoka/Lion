using System;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCookingMaterialManager : MonoBehaviour
{
    #region Singleton
    private static DroppedCookingMaterialManager _current = null;
    public static DroppedCookingMaterialManager Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    private void OnDestroy()
    {
        _current = null;
    }
    #endregion

    [SerializeField]
    private DroppedCookingMaterial _droppedCookingMaterialPrefab;
    [SerializeField]
    private Transform _droppedCookingMaterialParent;

    private HashSet<DroppedCookingMaterial> _activeItems = new HashSet<DroppedCookingMaterial>();
    private Stack<DroppedCookingMaterial> _inactiveItems = new Stack<DroppedCookingMaterial>();

    public void DropCookingMaterial(Vector3 position, int cookingMaterialID, float probability) // probabilityは確率を表現する。0.0から1.0で判断する。0の方が出にくく、1の方が出やすい。
    {
        var random = UnityEngine.Random.Range(0f, 1f);

        if (probability > random) return; // 確率を下回ればアイテムを生成しない。

        // Create Item.
        DroppedCookingMaterial cookingMaterial = null;
        if (_inactiveItems.Count == 0)
            cookingMaterial = Instantiate(_droppedCookingMaterialPrefab, _droppedCookingMaterialParent);
        else
            cookingMaterial = _inactiveItems.Pop();

        // Activate Item
        _activeItems.Add(cookingMaterial);
        cookingMaterial.gameObject.SetActive(true);
        cookingMaterial.Initialize(position, cookingMaterialID);
        cookingMaterial.OnDead += DeleteItem;
    }

    private void DeleteItem(DroppedCookingMaterial cookingMaterial)
    {
        cookingMaterial.OnDead -= DeleteItem;
        cookingMaterial.gameObject.SetActive(false);
        _activeItems.Remove(cookingMaterial);
        _inactiveItems.Push(cookingMaterial);
    }
}