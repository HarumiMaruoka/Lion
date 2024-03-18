using System;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class ChangeMoveModeButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _label;
    [SerializeField]
    private PlayerController _playerController;

    private void Start()
    {
        UpdateLabel(_playerController.CurrentMoveMode);
        _playerController.OnMoveModeChanged += UpdateLabel;

        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        _playerController.OnMoveModeChanged -= UpdateLabel;

        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void UpdateLabel(MoveMode moveMode)
    {
        _label.text = $"Move Mode: {moveMode.ToString()}";
    }

    private void OnButtonClicked()
    {
        var currentMoveMode = _playerController.CurrentMoveMode;
        // 現在のMoveModeがマニュアルならオートにする。
        if (currentMoveMode == MoveMode.Manual)
        {
            _playerController.BeginMoveCoroutine(MoveMode.Auto);
        }
        // 現在のMoveModeがオートかボスならマニュアルにする。
        else if (currentMoveMode == MoveMode.Auto || currentMoveMode == MoveMode.GoToBoss)
        {
            _playerController.BeginMoveCoroutine(MoveMode.Manual);
        }
        else
        {
            throw new ArgumentException("移動モードの変更に失敗。");
        }
    }
}