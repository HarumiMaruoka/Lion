using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Weapon.UI
{
    public class WeaponIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _itemView;
        [SerializeField]
        private TMPro.TextMeshProUGUI _levelView;

        private WeaponInstance _weapon;

        public void SetWeapon(WeaponInstance weapon)
        {
            _weapon = weapon;

            _itemView.sprite = weapon.Data.Icon;
            // _levelView.text = $"Lv.{weapon.LevelManager.CurrentLevel}";
        }
    }
}