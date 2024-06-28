using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.CameraUtility
{
    public class CameraSizeSlider : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _minSize = 6;
        [SerializeField] private float _maxSize = 20;

        private void Start()
        {
            _slider.minValue = _minSize;
            _slider.maxValue = _maxSize;
            _slider.value = Mathf.Clamp(_camera.orthographicSize, _minSize, _maxSize);

            OnValueChanged(_slider.value);
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        public void OnValueChanged(float value)
        {
            _camera.orthographicSize = value;
        }
    }
}