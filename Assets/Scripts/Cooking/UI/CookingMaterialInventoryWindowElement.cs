using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookingMaterialInventoryWindowElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _foreground;
    [SerializeField]
    private Text _countLabel;

    private CookingMaterialData _data;

    public event Action<int> OnSelected;

    public void Initialize(CookingMaterialData data) // CookingMaterialInventoryの初期化が終ってから呼び出す。
    {
        _data = data;

        OnCountChanged(CookingMaterialInventory.Instance.GetCount(data.ID));
        CookingMaterialInventory.Instance.OnCountChanged[data.ID] += OnCountChanged;

        _foreground.sprite = data.Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(_data.ID);
    }

    private void OnCountChanged(int count)
    {
        _countLabel.text = "x " + count.ToString();

        // 一つも所有していない場合は非表示にする。
        if (count == 0) gameObject.SetActive(false);
        // 一つでも所持していれば表示する。
        else gameObject.SetActive(true);
    }
}