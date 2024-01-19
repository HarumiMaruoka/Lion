using System;
using UnityEngine;

[DefaultExecutionOrder(100)] // WeaponManager‚Ìˆ—‚ÌŒã‚Ìˆ×AŒÄ‚Ño‚µ‚ğ’x‚ç‚¹‚éB
public class UpgradeWindow : MonoBehaviour
{
    private void Start()
    {
        CreateUpgradeTargetChangeButtons();
    }

    [SerializeField]
    private UpgradeTargetChangeButton _upgradeTargetChangeButtonPrefab;
    [SerializeField]
    private Transform _upgradeTargetChangeButtonParent;

    private void CreateUpgradeTargetChangeButtons()
    {
        var weapons = WeaponManager.Current.Weapons;
        foreach (var weapon in weapons)
        {
            var instance = Instantiate(_upgradeTargetChangeButtonPrefab, _upgradeTargetChangeButtonParent);
            instance.Initialize(weapon.Value);
        }
    }
}