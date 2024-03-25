using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Character;

namespace EquipmentWindowElement
{
    public class CharacterSelectButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _characterImage;
        [SerializeField]
        private Text _label;

        private int _index = -1;
        private CharacterIndividualData _selectedCharacter;

        public CharacterInventory CharacterInventory => CharacterInventory.Instance;
        public EquipCharacterManager EquipCharacterManager => EquipCharacterManager.Current;
        public CharacterInventoryWindow CharacterInventoryWindow => CharacterInventoryWindow.Current;

        public event Action<CharacterIndividualData> OnCharacterChanged;

        public CharacterIndividualData EquippedCharacter
        {
            get
            {
                return _selectedCharacter;
            }
            set
            {
                _selectedCharacter = value;
                OnCharacterChanged?.Invoke(value);
                if (value != null)
                {
                    _characterImage.sprite = value.SpeciesData.Sprite;
                    _characterImage.color = Color.white;
                    _label.text = value.ToString();
                }
                else
                {
                    _characterImage.sprite = null;
                    _characterImage.color = Color.clear;
                    _label.text = null;
                }
            }
        }

        public void Initialize(int index)
        {
            _index = index;
            EquippedCharacter = EquipCharacterManager.GetEquippedCharacterData(_index);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CharacterInventoryWindow.OnCharacterSelected += OnCharacterSelected;
            CharacterInventoryWindow.OnHided += OnHided;
            CharacterInventoryWindow.Show();
        }

        private void OnCharacterSelected(CharacterIndividualData selected)
        {
            // 装備していたキャラは後で使うので変数に保存する。
            var old = EquipCharacterManager.GetEquippedCharacterData(_index);

            // 選択されたキャラを装備し、インベントリから取り除く。
            // （インベントリの容量がいっぱいである事を想定し、先に取り除く。）
            EquipCharacterManager.EquipCharacter(_index, selected);
            CharacterInventory.RemoveCharacter(selected);

            // 装備していたキャラをインベントリに収める。
            if (old != null) CharacterInventory.AddCharacter(old);

            // キャラのインベントリウィンドウを閉じる。
            CharacterInventoryWindow.Hide();
            CharacterInventoryWindow.OnCharacterSelected -= OnCharacterSelected;
            CharacterInventoryWindow.OnHided -= OnHided;

            // 選択されたキャラをメンバ変数に保存する。
            EquippedCharacter = selected;
        }

        private void OnHided()
        {
            CharacterInventoryWindow.OnCharacterSelected -= OnCharacterSelected;
            CharacterInventoryWindow.OnHided -= OnHided;
        }
    }
}