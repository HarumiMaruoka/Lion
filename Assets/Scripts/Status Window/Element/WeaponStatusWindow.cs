using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusWindow : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _content;

    private WeaponBase _weapon;

    public WeaponBase Weapon
    {
        get => _weapon;
        set
        {
            _weapon = value;
            _title.text = value.WeaponName;
            _content.text = $"Level: {value.CurrentLevel}\n" + value.WeaponStatus.ToString();
        }
    }
}