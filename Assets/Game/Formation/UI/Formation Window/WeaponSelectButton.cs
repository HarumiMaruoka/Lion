using Lion.Weapon;
using Lion.Weapon.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Formation.UI
{
    [RequireComponent(typeof(Button))]
    public class WeaponSelectButton : MonoBehaviour
    {
        [SerializeField]
        private WeaponInventoryWindow _weaponInventoryWindow;

        [SerializeField]
        private Image _icon;
        [SerializeField]
        private WeaponEquippableButton _weaponEquippableButton;
        [SerializeField]
        private int _index;

        private WeaponInstance _selected;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenWindow);
            _weaponEquippableButton.OnSelected += OnTargetChanged;

            OnTargetChanged(_weaponEquippableButton.Equippable);
            var equipped = _weaponEquippableButton.Equippable?.Equipped(_index);
            ChangeWeapon(equipped);
        }

        private void OnTargetChanged(IWeaponEquippable equippable)
        {
            gameObject.SetActive(equippable != null);
        }

        private void OpenWindow()
        {
            _weaponInventoryWindow.Open();
            _weaponInventoryWindow.OnSelected += OnSelectedWeapon;
            _weaponInventoryWindow.OnDisabled += OnClosedWindow;

            if (_selected == null) return;
            _weaponIcon = _weaponInventoryWindow.GetWeaponIcon(_selected);
            _weaponIcon.RemoveLabel.gameObject.SetActive(true);
        }

        private WeaponIcon _weaponIcon;

        private void OnClosedWindow()
        {
            _weaponInventoryWindow.OnSelected -= OnSelectedWeapon;
            _weaponInventoryWindow.OnDisabled -= OnClosedWindow;

            if (_weaponIcon == null) return;
            _weaponIcon.RemoveLabel.gameObject.SetActive(false);
        }

        private void OnSelectedWeapon(WeaponInstance selected)
        {
            // 既に装備している武器が選択された場合は何もしない。
            // 装備された武器と選択された武器が同一の場合は例外。
            var equipped = _weaponEquippableButton.Equippable.Equipped(_index);
            if (equipped != selected && selected.IsActive) return;

            // 武器の装備処理。
            _weaponEquippableButton.Equippable.Equip(selected, _index);
            // 武器のアイコン画像を設定する。
            equipped = _weaponEquippableButton.Equippable.Equipped(_index);
            ChangeWeapon(equipped);
            // ウィンドウを閉じる。
            _weaponInventoryWindow.gameObject.SetActive(false);
        }

        private void ChangeWeapon(WeaponInstance weapon)
        {
            _selected = weapon;

            var sprite = weapon?.Data?.Icon;
            if (sprite)
            {
                _icon.sprite = sprite;
                _icon.color = Color.white;
            }
            else
            {
                _icon.sprite = null;
                _icon.color = Color.clear;
            }
        }
    }
}