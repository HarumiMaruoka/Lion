using System;
using EquipmentWindowElement;
using UnityEngine;

public class EquipmentWindow : WindowBase
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
            // 装備画面の各要素を生成初期化。
            var instance = Instantiate(_elementPrefab, _elementParent);
            instance.Initialize(i);
        }
    }
}