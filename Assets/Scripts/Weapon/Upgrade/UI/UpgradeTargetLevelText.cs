using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTargetLevelText : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private void Start()
    {
        UpgradeManager.Current.TargetLevelChanged += ApplyText;
    }

    public void ApplyText(int targetLevel)
    {
        _text.text = "Target Level: " + targetLevel;
    }
}