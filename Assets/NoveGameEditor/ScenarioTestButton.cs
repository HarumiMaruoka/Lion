using System;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioTestButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private ScenarioController _scenarioController;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _scenarioController.Begin();
    }
}