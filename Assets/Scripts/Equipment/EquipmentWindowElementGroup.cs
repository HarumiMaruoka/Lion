// 日本語対応
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentWindowElement
{
    public class EquipmentWindowElementGroup : MonoBehaviour
    {
        [SerializeField]
        private CharacterSelectButton _characterSelectButton;
        [SerializeField]
        private WeaponSelectButton _weaponSelectButtonPrefab;
        [SerializeField]
        private Transform _weaponSelectButtonParent;

        private List<WeaponSelectButton> _weaponSelectButtons = new List<WeaponSelectButton>();

        public void Initialize(int index)
        {
            _characterSelectButton.Initialize(index); // キャラ選択ボタンの初期化。
            OnChangedCharacter(_characterSelectButton.EquippedCharacter); // 武器選択ボタンの初期化。
            _characterSelectButton.OnCharacterChanged += OnChangedCharacter; // キャラが変更される度に武器選択ボタンの初期化を実行。
        }

        private void OnChangedCharacter(CharacterIndividualData character)
        {
            if (character != null)
            {
                // 不足分を生成する。
                int equippableCount = character.EquippedWeapons.Length;
                while (_weaponSelectButtons.Count < equippableCount)
                {
                    var instance = Instantiate(_weaponSelectButtonPrefab, _weaponSelectButtonParent);
                    _weaponSelectButtons.Add(instance);
                }

                // 必要な分だけ起動する。過剰分は停止する。
                for (int i = 0; i < _weaponSelectButtons.Count; i++)
                {
                    if (i < equippableCount)
                    {
                        _weaponSelectButtons[i].gameObject.SetActive(true);
                        _weaponSelectButtons[i].SetIndex(i);
                        _weaponSelectButtons[i].SetCharacterSelectButton(_characterSelectButton);
                    }
                    else
                    {
                        _weaponSelectButtons[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // 全て停止する。
                for (int i = 0; i < _weaponSelectButtons.Count; i++)
                {
                    _weaponSelectButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}