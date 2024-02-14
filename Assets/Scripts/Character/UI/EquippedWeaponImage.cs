using System;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterInventoryWindowUI
{
    public class EquippedWeaponImage : MonoBehaviour
    {
        [SerializeField]
        private Image _background;
        [SerializeField]
        private Image _foreground;

        public Image Background => _background;
        public Image Foreground => _foreground;
    }
}