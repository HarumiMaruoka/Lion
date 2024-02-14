using EquipmentWindowElement;
using System;
using UnityEngine;

public class EquipmentWindow : MonoBehaviour
{
    [SerializeField]
    private EquipmentWindowElementGroup _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var equippableCount = EquipCharacterManager.Current.EquippableCharacterCount;

        for (int i = 0; i < equippableCount; i++)
        {
            // ‘•”õ‰æ–Ê‚ÌŠe—v‘f‚ð¶¬‰Šú‰»B
            var instance = Instantiate(_elementPrefab, _elementParent);
            instance.Initialize(i);
        }
    }
}