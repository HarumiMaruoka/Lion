using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lion
{
    public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private VirtualJoystickUI _ui;

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