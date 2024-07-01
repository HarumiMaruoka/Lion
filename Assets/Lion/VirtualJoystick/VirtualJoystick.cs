using Lion.LionDebugger;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lion
{
    [RequireComponent(typeof(Image))]
    public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private VirtualJoystickUI _ui;

        [SerializeField]
        private DebugModeToggle _debugModeToggle;

        private Color _initialColor;
        private Image _draggableArea;

        private void Start()
        {
            _draggableArea = GetComponent<Image>();
            _initialColor = _draggableArea.color;

            _debugModeToggle.DebugModeChanged += OnDebugModeChanged;
            OnDebugModeChanged(_debugModeToggle.IsDebugMode);
        }

        private void OnDebugModeChanged(bool isDebugMode)
        {
            if (isDebugMode)
                _draggableArea.color = _initialColor;
            else
                _draggableArea.color = Color.clear;
        }

        public Vector2 Vector => _currentTouchPoint - _beginTouchPoint;

        public bool IsDragging { get; private set; } = false;

        private Vector2 _beginTouchPoint;
        private Vector2 _currentTouchPoint;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _ui.Begin(eventData.position);
            IsDragging = true;
            _beginTouchPoint = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _ui.OnUpdate(eventData.position);
            _currentTouchPoint = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _ui.End();
            IsDragging = false;
        }
    }
}