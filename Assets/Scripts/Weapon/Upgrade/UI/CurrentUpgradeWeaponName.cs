using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrentUpgradeWeaponName : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private void Start()
    {
        ApplyText(UpgradeManager.Current.Selected);
        UpgradeManager.Current.OnSelectedChanged += ApplyText;
    }

    private void ApplyText(WeaponBase weapon)
    {
        if (weapon) _text.text = weapon.WeaponName;
        else _text.text = "Not Selected";
    }
}