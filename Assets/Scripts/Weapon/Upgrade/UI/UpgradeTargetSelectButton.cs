using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeTargetSelectButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private WeaponInventoryWindow.ShowMode _showMode = WeaponInventoryWindow.ShowMode.Everything;
    [SerializeField]
    private Image _foreground;
    [SerializeField]
    private Text _label;

    private WeaponInventoryWindow WeaponInventoryWindow => WeaponInventoryWindow.Current;

    private void Start()
    {
        OnUpgradeTargetChanged(UpgradeManager.Current.Selected);
        UpgradeManager.Current.OnUpgradeTargetChanged += OnUpgradeTargetChanged;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Show();
    }

    private void Show()
    {
        WeaponInventoryWindow.OnHided += Hide;
        WeaponInventoryWindow.OnSelectedWeapon += OnSelected;

        WeaponInventoryWindow.Show(_showMode);
    }

    private void Hide()
    {
        WeaponInventoryWindow.OnHided -= Hide;
        WeaponInventoryWindow.OnSelectedWeapon -= OnSelected;
    }

    private void OnSelected(WeaponBase selected)
    {
        UpgradeManager.Current.ChangeUpgradeTarget(selected);
        WeaponInventoryWindow.Hide();

    }

    private void OnUpgradeTargetChanged(WeaponBase upgradeTarget)
    {
        if (upgradeTarget)
        {
            _foreground.sprite = upgradeTarget.Data.WeaponIcon;
            _label.text =
                $"Name: {upgradeTarget.WeaponName}\n" +
                $"Level: {upgradeTarget.CurrentLevel}\n";
        }
        else
        {
            _foreground.sprite = null;
            _label.text = $"Upgrade target is null.";
        }
    }
}