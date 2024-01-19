using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeGage : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    public void Initialize(float max, float initialLife,ref Action<float> onValueChanged)
    {
        _slider.minValue = 0f;
        _slider.maxValue = max;

        ApplyValue(initialLife);
        onValueChanged += ApplyValue;
    }

    private void ApplyValue(float remainingLife)
    {
        _slider.value = remainingLife;
    }
}