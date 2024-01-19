using System;
using UnityEngine;

public class UpgradeRequestItemWindow : MonoBehaviour
{
    [SerializeField]
    private UpgradeRequestItemWindowElement _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private void Start()
    {
        CreateElements();
    }

    public void CreateElements()
    {
        var allItems = ItemManager.Current.ItemData;

        foreach (var item in allItems)
        {
            var instance = GameObject.Instantiate(_elementPrefab, _elementParent);
            instance.Initialize(item);
        }
    }
}