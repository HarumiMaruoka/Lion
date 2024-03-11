using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusWindow : MonoBehaviour
{
    [SerializeField]
    private Text _label;

    private WeaponStatus _weaponStatus;

    public WeaponStatus WeaponStatus
    {
        get => _weaponStatus;
        set
        {
            _weaponStatus = value;
            _label.text = value.ToString();
        }
    }
}