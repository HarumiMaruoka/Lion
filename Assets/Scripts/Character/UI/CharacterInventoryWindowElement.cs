using CharacterInventoryWindowUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character
{
    public class CharacterInventoryWindowElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _background;
        [SerializeField]
        private Image _actorImage;
        [SerializeField]
        private Text _label;

        public Image Background => _background;
        public Image ActorImage => _actorImage;
        public Text Label => _label;

        private CharacterIndividualData _characterData;

        public CharacterIndividualData CharacterData
        {
            get { return _characterData; }
            set
            {
                _characterData = value;
                if (value != null)
                {
                    _actorImage.sprite = value.SpeciesData.Sprite;
                    _label.text = value.ToString();
                    ShowEquippedWeapon(value);
                }
                else
                {
                    _actorImage.sprite = null;
                    _label.text = "value is null.";
                }
            }
        }

        private void OnDisable()
        {
            HideEquippedWeapon();
        }

        public event Action<CharacterIndividualData> OnCharacterSelected;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnCharacterSelected?.Invoke(_characterData);
        }

        [SerializeField]
        private EquippedWeaponImage _weaponImagePrefab;
        [SerializeField]
        private Transform _weaponImageParent;

        private HashSet<EquippedWeaponImage> _actives = new HashSet<EquippedWeaponImage>();
        private Queue<EquippedWeaponImage> _inactives = new Queue<EquippedWeaponImage>();

        private void ShowEquippedWeapon(CharacterIndividualData character)
        {
            if (character == null) return;

            var equippeds = character.EquippedWeapons;
            for (int i = 0; i < equippeds.Length; i++)
            {
                var weaponImage = GetWeaponImage();

                if (equippeds[i])
                {
                    weaponImage.Foreground.sprite = equippeds[i].Data.WeaponIcon;
                    weaponImage.Foreground.color = Color.white;
                }
                else
                {
                    weaponImage.Foreground.sprite = null;
                    weaponImage.Foreground.color = Color.clear;
                }
            }
        }

        private void HideEquippedWeapon()
        {
            foreach (var item in _actives)
            {
                _inactives.Enqueue(item);
                item.gameObject.SetActive(false);
            }
            _actives.Clear();
        }

        private EquippedWeaponImage GetWeaponImage()
        {
            EquippedWeaponImage result;
            if (_inactives.Count != 0)
            {
                result = _inactives.Dequeue();
                result.transform.SetAsLastSibling(); // Layout Group‚Ì“s‡‚ÅƒqƒGƒ‰ƒ‹ƒL[‚Ìˆê”Ô‰º‚ÉŽ‚Á‚Ä‚­‚éB
            }
            else
            {
                result = Instantiate(_weaponImagePrefab, _weaponImageParent);
            }
            result.gameObject.SetActive(true);
            _actives.Add(result);
            return result;
        }
    }
}