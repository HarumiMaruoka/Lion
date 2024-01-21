using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EquippedWeaponUI
{
    public class DragHandler : MonoBehaviour
    {
        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private ScrollRect _equippedWindowScrollRect;
        [SerializeField]
        private ScrollRect _inventoryScrollRect;

        private WeaponBase _weaponData;

        private IDraggable _dragBegin;
        private IDraggable _dragEnd;

        public WeaponBase Weapon
        {
            get => _weaponData;
            set
            {
                _weaponData = value;
                if (value)
                {
                    _iconImage.sprite = value.Data.WeaponIcon;
                    _iconImage.enabled = true;
                }
                else
                {
                    _iconImage.sprite = null;
                    _iconImage.enabled = false;
                }
            }
        }

        private void Start()
        {
            Weapon = null;
        }

        private void Update()
        {
            transform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _dragBegin = GetMouseOverlapDraggable();
                if (_dragBegin != null) BeginDrag(_dragBegin);
            }
            if (Input.GetMouseButtonUp(0))
            {
                _dragEnd = GetMouseOverlapDraggable();
                if (_dragBegin != null && _dragEnd != null) EndDrag(_dragBegin, _dragEnd);
                EndDragFinally();
            }
        }

        private bool _isDragging = false;
        public bool IsDragging => _isDragging;
        public event Action OnDragBegin;
        public event Action OnDragEnd;

        private void BeginDrag(IDraggable begin)
        {
            _isDragging = true;
            Weapon = begin.Weapon;

            OnDragBegin?.Invoke();

            _equippedWindowScrollRect.vertical = false;
            _inventoryScrollRect.vertical = false;
        }

        private void EndDrag(IDraggable begin, IDraggable end)
        {
            SwapWeaponData(begin, end);
            Weapon = null;
        }

        private void EndDragFinally()
        {
            _isDragging = false;
            OnDragEnd?.Invoke();

            _equippedWindowScrollRect.vertical = true;
            _inventoryScrollRect.vertical = true;
        }

        private List<RaycastResult> _rayCastUIResults = new List<RaycastResult>();

        private IDraggable GetMouseOverlapDraggable()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            Vector2 mousePosition = Input.mousePosition;
            eventData.position = mousePosition;

            EventSystem.current.RaycastAll(eventData, _rayCastUIResults);

            foreach (var ui in _rayCastUIResults)
            {
                if (ui.gameObject.TryGetComponent(out IDraggable draggable))
                {
                    return draggable;
                }
            }

            return null;
        }

        private void SwapWeaponData(IDraggable a, IDraggable b)
        {
            var aIndex = a.Index;
            var bIndex = b.Index;

            var aType = a.EquippedWeaponType;
            var bType = b.EquippedWeaponType;

            if (aType == EquippedWeaponType.Equipped && bType == EquippedWeaponType.Equipped)
            {
                WeaponInventory.Current.SwapEquipped(aIndex, bIndex);
            }
            else if (aType == EquippedWeaponType.Inventory && bType == EquippedWeaponType.Inventory)
            {
                WeaponInventory.Current.SwapInInventory(aIndex, bIndex);
            }
            else if (aType == EquippedWeaponType.Inventory && bType == EquippedWeaponType.Equipped)
            {
                WeaponInventory.Current.SwapEquippedAndInventory(bIndex, aIndex);
            }
            else if (aType == EquippedWeaponType.Inventory && bType == EquippedWeaponType.Equipped)
            {
                WeaponInventory.Current.SwapEquippedAndInventory(aIndex, bIndex);
            }
        }
    }
}