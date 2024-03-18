using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraSizeControlSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerController.Current.OnUIDeselect();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerController.Current.OnUISelect();
    }
}