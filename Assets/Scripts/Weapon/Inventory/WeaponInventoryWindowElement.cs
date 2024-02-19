using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponInventoryWindowElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _weaponImage;
    [SerializeField]
    private Text _label;

    private WeaponBase _weapon;

    public WeaponBase Weapon
    {
        get => _weapon;
        set
        {
            _weapon = value;
            if (value)
            {
                _weaponImage.sprite = value.Data.WeaponIcon;
                _label.text =
                    // $"{value.WeaponName}\n" +
                    $"Lv. {value.CurrentLevel}\n";
            }
            else
            {
                _weaponImage.sprite = null;
                _label.text = null;
            }
        }
    }
    public event Action<WeaponBase> OnSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(_weapon);
    }
}