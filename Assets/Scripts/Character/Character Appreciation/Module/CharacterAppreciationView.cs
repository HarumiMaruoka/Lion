using System;
using System.Collections;
using UnityEngine;

public class CharacterAppreciationView : MonoBehaviour
{
    [SerializeField]
    private CharacterInventoryWindow _inventoryWindow;

    public void ShowCharacterSelectWindow()
    {
        _inventoryWindow.OnCharacterSelected += _appreciationWindow.Show;
        _inventoryWindow.OnHided += OnCharacterSelectWindowHided;

        _inventoryWindow.Show();
    }

    public void OnCharacterSelectWindowHided()
    {
        _inventoryWindow.OnCharacterSelected -= _appreciationWindow.Show;
        _inventoryWindow.OnHided -= OnCharacterSelectWindowHided;
    }

    [SerializeField]
    private CharacterAppreciationWindow _appreciationWindow;

    public void HideAppreciationWindow()
    {
        _appreciationWindow.Hide();
    }
}