using System;
using UnityEngine;

namespace Lion.UI
{
    public class DamageVFX : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _label;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _duration;

        private float _value;
        private float _timer;
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                _label.text = value.ToString();
                _timer = 0f;
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                DamageVFXPool.Instance.Destroy(this);
                return;
            }
            transform.position += Vector3.up * _speed * Time.deltaTime;
        }
    }
}