using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeApplyButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(UpgradeManager.Current.ApplyUpgradeLevel);
    }
}