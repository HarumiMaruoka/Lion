using System;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeaponInventoryWindowElement : MonoBehaviour
{
    [SerializeField]
    private Image _iconImage;

    private WeaponData _weaponData;

    public WeaponData WeaponData
    {
        get => _weaponData;
        set
        {
            _weaponData = value;
            if (value)
            {
                _iconImage.sprite = value.WeaponIcon;
                gameObject.SetActive(true);
            }
            else
            {
                _iconImage.sprite = null;
                transform.SetAsLastSibling();
                gameObject.SetActive(false);
            }
        }
    }
}