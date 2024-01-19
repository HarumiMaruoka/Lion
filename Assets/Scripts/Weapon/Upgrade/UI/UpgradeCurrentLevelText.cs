using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCurrentLevelText : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private void Start()
    {
        UpgradeManager.Current.OnSelectedChanged += ApplyText;
    }

    public void ApplyText(WeaponBase selected)
    {
        _text.text = "Current Level: " + selected.CurrentLevel.ToString();
    }
}