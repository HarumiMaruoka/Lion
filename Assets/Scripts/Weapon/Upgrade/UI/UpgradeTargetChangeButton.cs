using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTargetChangeButton : MonoBehaviour
{
    [SerializeField]
    private Text _weaponNameView;
    [SerializeField]
    private Button _button;

    private WeaponBase _weapon;

    public void Initialize(WeaponBase weapon)
    {
        _weapon = weapon;
        _weaponNameView.text = weapon.WeaponName;
        _button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        UpgradeManager.Current.ChangeUpgradeTarget(_weapon.WeaponType);
    }
}