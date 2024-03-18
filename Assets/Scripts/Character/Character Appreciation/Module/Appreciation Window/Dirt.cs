using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dirt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _maxDirtinessLevel; // 最大汚れ度
    [SerializeField]
    private float _dirtinessLevel; // 汚れ度

    public void Initialize(float maxDirtinessLevel, float initialDirtinessLevel)
    {
        _maxDirtinessLevel = maxDirtinessLevel;
        _dirtinessLevel = initialDirtinessLevel;
    }

    private void Update()
    {
        // このオブジェクト上でドラッグされていれば
        // 泡パーティクルを生成し、汚れ度を減らす。
        if (!_isMouseOver) return;
        DragUpdate();
    }

    private bool _isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
        _currentMousePosition = Input.mousePosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
        _currentMousePosition = Vector2.zero;
    }

    private Vector2 _previousMousePosition = Vector2.zero;
    private Vector2 _currentMousePosition = Vector2.zero;

    private float _bubbleVFXInterval = 0.2f;
    private float _bubbleVFXIntervalTimer = 0f;

    private void DragUpdate()
    {
        _previousMousePosition = _currentMousePosition;
        _currentMousePosition = Input.mousePosition;

        if (_bubbleVFXIntervalTimer > 0f)
            _bubbleVFXIntervalTimer -= Time.deltaTime;

        if (_previousMousePosition != _currentMousePosition)
        {
            var diff = _previousMousePosition - _currentMousePosition;
            var magnitude = diff.magnitude; // 移動量

            _dirtinessLevel -= magnitude;
            UpdateAlpha();

            if (_bubbleVFXIntervalTimer <= 0)
            {
                _bubbleVFXIntervalTimer = _bubbleVFXInterval;
                VFXManager.Current.CreateBubbleVFX(Input.mousePosition);
            }
        }
    }
    private void UpdateAlpha()
    {
        var col = _image.color;
        col.a = _dirtinessLevel / _maxDirtinessLevel;
        _image.color = col;
    }
}