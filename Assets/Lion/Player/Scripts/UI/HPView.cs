using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Player.UI
{
    public class HPView : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _player;
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Vector3 _offset;

        private void Start()
        {
            _slider.minValue = 0;
            _slider.maxValue = 1;

            SetHP(PlayerController.Instance.CurrentHP);

            transform.position = PlayerController.Instance.transform.position + _offset;
        }

        private void Update()
        {
            SetHP(PlayerController.Instance.CurrentHP);

            transform.position = PlayerController.Instance.transform.position + _offset;
        }

        public void SetHP(float hp)
        {
            var maxHP = PlayerController.Instance.MaxHP;
            _slider.value = hp / maxHP;
        }
    }
}