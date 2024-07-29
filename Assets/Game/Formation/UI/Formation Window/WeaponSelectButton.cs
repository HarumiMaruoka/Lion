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
            // 装備者
            var equippable = _weaponEquippableButton.Equippable;

            GetComponent<Button>().onClick.AddListener(OpenWindow);

            _weaponEquippableButton.OnSelected += OnTargetChanged;
            OnTargetChanged(equippable);

            if (equippable != null)
            {
                var equipped = equippable.Equipped(_index);
                UpdateSprite(equipped);
            }
            else
            {
                UpdateSprite(null);
            }
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
            // 選択された武器を保存する。
            _selected = _weaponEquippableButton.Equippable.Equipped(_index);
            // 武器のアイコン画像を設定する。
            UpdateSprite(_selected);
            // ウィンドウを閉じる。
            _weaponInventoryWindow.gameObject.SetActive(false);
        }

        private void UpdateSprite(WeaponInstance weapon)
        {
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