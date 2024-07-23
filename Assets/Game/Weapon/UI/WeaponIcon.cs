using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Weapon.UI
{
    [RequireComponent(typeof(Button))]
    public class WeaponIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _itemView;
        [SerializeField]
        private TMPro.TextMeshProUGUI _levelView;

        private WeaponInstance _weapon;

        public event Action<WeaponInstance> OnSelected;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Select);
        }

        private void Select()
        {
            OnSelected?.Invoke(_weapon);
        }

        public void SetWeapon(WeaponInstance weapon)
        {
            _weapon = weapon;

            _itemView.sprite = weapon.Data.Icon;
            // _levelView.text = $"Lv.{weapon.LevelManager.CurrentLevel}";
        }
    }
}