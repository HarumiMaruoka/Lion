using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingResultView : MonoBehaviour
{
    [SerializeField]
    private CookingController _controller;

    [SerializeField]
    private Image _foreground;
    [SerializeField]
    private Text _label;

    private void Start()
    {
        MakableFoodChanged(_controller.MakableFood);
        _controller.OnMakableFoodChanged += MakableFoodChanged;
    }

    private void MakableFoodChanged(CookingFoodData foodData)
    {
        if (foodData != null)
        {
            _foreground.color = Color.white;
            _foreground.sprite = foodData.Sprite;
            _label.text = null;
        }
        else
        {
            _foreground.color = Color.white;
            _foreground.sprite = null;
            _label.text = "There is no matching FoodData.";
        }
    }
}