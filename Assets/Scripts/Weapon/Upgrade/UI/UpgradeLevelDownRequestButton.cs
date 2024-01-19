using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLevelDownRequestButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(UpgradeManager.Current.TargetLevelDownRequest);
    }
}