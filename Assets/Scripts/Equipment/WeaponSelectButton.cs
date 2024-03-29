using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Character;

namespace EquipmentWindowElement
{
    public class WeaponSelectButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _weaponImage;
        [SerializeField]
        private Text _label;

        private int _index = -1;
        private WeaponBase _weapon;
        private CharacterIndividualData _character;

        public int Index => _index;
        public WeaponInventoryWindow WeaponInventoryWindow => WeaponInventoryWindow.Current;

        public WeaponBase Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                var old = _weapon;
                _weapon = value;
                if (value != null)
                {
                    _weaponImage.sprite = value.Data.WeaponIcon;
                    _weaponImage.color = Color.white;
                    ApplyLevelLabel(value.CurrentLevel);

                    if (old) old.OnLevelChanged -= ApplyLevelLabel;
                    value.OnLevelChanged += ApplyLevelLabel;
                }
                else
                {
                    _weaponImage.sprite = null;
                    _weaponImage.color = Color.clear;
                    _label.text = null;
                }
            }
        }

        public CharacterIndividualData Character
        {
            get
            {
                return _character;
            }
            set
            {
                _character = value;
                if (value != null)
                {
                    var equippedWeapons = value.EquippedWeapons;
                    if (_index < 0 || _index >= equippedWeapons.Length)
                    {
                        Debug.LogError("Out of Range: The index is beyond the valid range.");
                        return;
                    }
                    Weapon = equippedWeapons[_index];
                }
                else
                {
                    Weapon = null;
                }
            }
        }

        private void ApplyLevelLabel(int level)
        {
            _label.text = $"Lv. {level}";
        }

        public void SetIndex(int index)
        {
            _index = index;
        }

        private CharacterSelectButton _characterSelectButton;

        public void SetCharacterSelectButton(CharacterSelectButton characterSelectButton)
        {
            if (_characterSelectButton) _characterSelectButton.OnCharacterChanged -= OnCharacterChanged; // 過剰な登録を防止する。
            _characterSelectButton = characterSelectButton;
            Character = characterSelectButton.EquippedCharacter;
            characterSelectButton.OnCharacterChanged += OnCharacterChanged;
        }

        private void OnCharacterChanged(CharacterIndividualData character)
        {
            Character = character;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Character == null)
            {
                Debug.LogError("Character is null.");
                return;
            }

            WeaponInventoryWindow.OnSelectedWeapon += OnSelectedWeapon; // 武器が選択されたとき
            WeaponInventoryWindow.OnHided += OnHidedInventoryWindow; // 武器が選択されずにWeaponInventoryWindowが閉じたとき
            WeaponInventoryWindow.Show(WeaponInventoryWindow.ShowMode.Inventory);
        }

        private void OnSelectedWeapon(WeaponBase selected)
        {
            // 後で利用するので装備していた武器を変数に保存する。
            var old = Character.EquippedWeapons[_index];

            // 選択された武器を装備し、インベントリから選択された武器を取り除く。
            Character.EquippedWeapons[_index] = selected;
            WeaponInventory.Current.RemoveWeapon(selected);
            // キャラクターが装備済みであれば武器をアクティブ化する。
            if (Character.EquipIndex != -1)
            {
                var equipCharacter = EquipCharacterManager.Current.GetCharacterBehaviour(Character.EquipIndex);
                if (equipCharacter) selected.Activate(equipCharacter.IndividualData, equipCharacter.transform);
                else Debug.Log("equipCharacter is null.");
            }

            // 装備していた武器をインベントリに収める。
            if (old)
            {
                WeaponInventory.Current.AddWeapon(old);
                old.Inactivate();
            }

            // 武器インベントリウィンドウを閉じる。
            WeaponInventoryWindow.Hide();

            Weapon = selected;
        }

        private void OnHidedInventoryWindow()
        {
            WeaponInventoryWindow.OnSelectedWeapon -= OnSelectedWeapon;
            WeaponInventoryWindow.OnHided -= OnHidedInventoryWindow;
        }
    }
}