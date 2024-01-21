using EquippedWeaponUI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWindowElement : MonoBehaviour, IDraggable
{
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private EquippedWeaponType _equippedWeaponType;

    private WeaponBase _weapon;

    private int _index;

    public void Initialize(int index, ref Action<WeaponBase> onWeaponChanged)
    {
        _index = index;
        onWeaponChanged += weapon => Weapon = weapon;
    }

    public WeaponBase Weapon
    {
        get => _weapon;
        set
        {
            _weapon = value;
            if (value)
            {
                _iconImage.sprite = value.Data.WeaponIcon;
            }
            else if (_equippedWeaponType == EquippedWeaponType.Equipped)
            {
                _iconImage.sprite = null;
            }
            else
            {
                _iconImage.sprite = null;
                gameObject.SetActive(false);
            }
        }
    }

    public int Index => _index;

    public EquippedWeaponType EquippedWeaponType => _equippedWeaponType;
}