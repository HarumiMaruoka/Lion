using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dirt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _maxDirtinessLevel; // ç≈ëÂâòÇÍìx
    [SerializeField]
    private float _dirtinessLevel; // âòÇÍìx

    public void Initialize(float maxDirtinessLevel, float dirtinessLevel, Vector2 pos)
    {
        transform.position = pos;
        _maxDirtinessLevel = maxDirtinessLevel;
        _dirtinessLevel = dirtinessLevel;

        UpdateAlpha();
    }

    private void Update()
    {
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
        if (_isMouseOver)
        {
            DragUpdate();
        }

        _isMouseOver = false;
        _currentMousePosition = Vector2.zero;
    }

    private Vector2 _previousMousePosition = Vector2.zero;
    private Vector2 _currentMousePosition = Vector2.zero;

    private float _bubbleVFXInterval = 0.2f;
    private float _bubbleVFXIntervalTimer = 0f;

    public event Action<Dirt> OnDestroyed;

    private void DragUpdate()
    {
        if (!_isMouseOver) return;

        _previousMousePosition = _currentMousePosition;
        _currentMousePosition = Input.mousePosition;

        if (_bubbleVFXIntervalTimer > 0f)
            _bubbleVFXIntervalTimer -= Time.deltaTime;

        if (_previousMousePosition != _currentMousePosition)
        {
            var diff = _previousMousePosition - _currentMousePosition;
            var magnitude = diff.magnitude; // à⁄ìÆó 

            _dirtinessLevel -= magnitude;
            UpdateAlpha();

            if (_bubbleVFXIntervalTimer <= 0f)
            {
                _bubbleVFXIntervalTimer = _bubbleVFXInterval;
                VFXManager.Current.CreateBubbleVFX(Input.mousePosition);
            }

            if (_dirtinessLevel < 0)
            {
                OnDestroyed?.Invoke(this);
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