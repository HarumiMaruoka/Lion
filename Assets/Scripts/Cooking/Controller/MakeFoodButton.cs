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
            bool isDeselect = i == (_makeCount - 1); // 最後だけ料理素材の選択状態を解除する。
            _controller.MakeFood(isDeselect);
        }
    }
}