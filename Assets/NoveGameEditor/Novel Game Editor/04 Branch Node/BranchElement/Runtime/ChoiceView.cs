using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private Text _text = null;
    [SerializeField]
    private Color _hoveredColor = Color.red;
    [SerializeField]
    private Color _normalColor = Color.white;

    public event Action<int> OnClick;

    private int _index;

    public void Initialize(int index, string text)
    {
        _image.color = _normalColor;
        _index = index;
        _text.text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(_index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = _hoveredColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _normalColor;
    }
}