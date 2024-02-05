using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipWeaponSelector : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _weaponImage;
    [SerializeField]
    private WeaponSelectWindow _weaponSelectWindow;
    [SerializeField]
    private CharacterBehaviour _character;
    [SerializeField]
    private int _index;

    public void OnPointerClick(PointerEventData eventData)
    {
        _weaponSelectWindow.OnSelected += OnSelectedWeapon;
        _weaponSelectWindow.gameObject.SetActive(true);
    }

    private void OnSelectedWeapon(WeaponBase selectedWeapon)
    {
        var equipped = _character.EquippedWeapon[_index];
        if (equipped != null) { equipped.gameObject.SetActive(false); }

        _character.EquippedWeapon[_index] = selectedWeapon;
        if (selectedWeapon)
        {
            selectedWeapon.gameObject.SetActive(true);
            _weaponImage.sprite = selectedWeapon.Data.WeaponIcon;
        }
        else
        {
            _weaponImage.sprite = null;
        }
    }
}