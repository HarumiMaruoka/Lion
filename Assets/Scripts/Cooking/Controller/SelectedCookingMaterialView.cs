using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedCookingMaterialView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CookingController _controller;

    [SerializeField]
    private int _index;
    [SerializeField]
    private Image _foreground;
    [SerializeField]
    private Text _label;

    private void Start()
    {
        ApplyData(_controller.SelectedMaterialIDs[_index]);
        _controller.OnSelectedChangeds[_index] += ApplyData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RevertData();
    }

    public void ApplyData(int selectedMaterialID) // selectedMaterialIDが無効な値であることもある。(-1は無効の値を表現する。)
    {
        bool isExistSelected = CookingMaterialDataBase.Current.TryGetData(selectedMaterialID, out CookingMaterialData selected);

        if (isExistSelected)
        {
            _foreground.color = Color.white;
            _foreground.sprite = selected.Sprite;
            _label.text = null;
        }
        else
        {
            _foreground.color = Color.clear;
            _foreground.sprite = null;
            _label.text = "Selected is null.";
        }
    }

    public void RevertData()
    {
        _controller.ExcludeMaterial(_index);
    }
}