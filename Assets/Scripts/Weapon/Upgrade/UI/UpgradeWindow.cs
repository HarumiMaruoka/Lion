using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : WindowBase
{
    [SerializeField]
    private Text _targetWeaponNameText;
    [SerializeField]
    private Text _currentLevelText;
    [SerializeField]
    private Text _targetLevelText;

    private void Start()
    {
        ApplyText(UpgradeManager.Current.Selected);
        UpgradeManager.Current.OnUpgradeTargetChanged += ApplyText;
        UpgradeManager.Current.OnTargetLevelChanged += OnTargetLevelChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UpgradeManager.Current.ChangeUpgradeTarget(null);
    }

    private void ApplyText(WeaponBase upgradeTarget)
    {
        if (upgradeTarget)
        {
            _targetWeaponNameText.text = $"Target Weapon: {upgradeTarget.WeaponName}";
            _currentLevelText.text = $"Current Level: {upgradeTarget.CurrentLevel}";
            _targetLevelText.text = $"Target Level: {UpgradeManager.Current.TargetLevel}";
        }
        else
        {
            _targetWeaponNameText.text = $"Target Weapon: Target weapon null.";
            _currentLevelText.text = $"Current Level: Target weapon null.";
            _targetLevelText.text = $"Target Level: Target weapon null.";
        }
    }

    private void OnTargetLevelChanged(int targetLevel)
    {
        _targetLevelText.text = $"Target Level: {targetLevel}";
    }
}