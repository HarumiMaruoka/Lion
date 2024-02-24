using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeFoodButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CookingController _controller;

    public void OnPointerClick(PointerEventData eventData)
    {
        _controller.MakeFood();
    }
}