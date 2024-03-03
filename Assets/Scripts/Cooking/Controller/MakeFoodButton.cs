using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeFoodButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CookingController _controller;
    [SerializeField]
    private int _makeCount = 1;

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < _makeCount; i++)
        {
            bool isDeselect = i == (_makeCount - 1); // ÅŒã‚¾‚¯—¿—‘fÞ‚Ì‘I‘ðó‘Ô‚ð‰ðœ‚·‚éB
            _controller.MakeFood(isDeselect);
        }
    }
}