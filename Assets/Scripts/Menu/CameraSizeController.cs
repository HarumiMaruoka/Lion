using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraSizeController : MonoBehaviour
{
    [SerializeField]
    private Slider _controlSlider;
    [SerializeField]
    private Camera _targetCamera;

    [SerializeField]
    private float _maxValue = 30f;
    [SerializeField]
    private float _minValue = 6f;

    [SerializeField]
    private Text _label;

    public float CurrentValue => _controlSlider.value;

    private void Start()
    {
        // Apply initial value.
        _controlSlider.maxValue = _maxValue;
        _controlSlider.minValue = _minValue;

        _controlSlider.value = _targetCamera.orthographicSize;
        UpdateLabel(_controlSlider.value);

        // Register functions.
        _controlSlider.onValueChanged.AddListener(UpdateCameraSize);
        _controlSlider.onValueChanged.AddListener(UpdateLabel);

        //_controlSlider.OnPointerDown(PlayerController.Current.OnUISelect);
        //_controlSlider.OnPointerUp(PlayerController.Current.OnUIDeselect);
    }

    private void UpdateCameraSize(float value)
    {
        _targetCamera.orthographicSize = value;
    }

    private void UpdateLabel(float value)
    {
        if (!_label) return;

        _label.text = $"Camera Size: {value.ToString(".0")}";
    }
}