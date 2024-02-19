using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenWeaponInventoryWindowButton : MonoBehaviour
{
    [SerializeField]
    private WeaponInventoryWindow.ShowMode _showMode = WeaponInventoryWindow.ShowMode.Inventory;
    [SerializeField]
    private Button _button;

    private WeaponInventoryWindow WeaponInventoryWindow => WeaponInventoryWindow.Current;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        WeaponInventoryWindow.Show(_showMode);
    }
}