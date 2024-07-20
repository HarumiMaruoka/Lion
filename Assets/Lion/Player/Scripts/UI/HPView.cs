using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Player.UI
{
    public class HPView : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private Vector3 _offset;

        private void Start()
        {
            _slider.minValue = 0;
            _slider.maxValue = 1;

            SetHP(PlayerController.Instance.CurrentHP);
            // PlayerController.Instance.OnHPChanged += SetHP;

            transform.position = PlayerController.Instance.transform.position + _offset;
        }

        private void Update()
        {
            SetHP(PlayerController.Instance.CurrentHP);

            transform.position = PlayerController.Instance.transform.position + _offset;
        }

        private void OnDestroy()
        {
            // if (PlayerController.Instance) PlayerController.Instance.OnHPChanged -= SetHP;
        }

        public void SetHP(float hp)
        {
            var maxHP = PlayerController.Instance.MaxHP;
            _slider.value = hp / maxHP;
        }
    }
}