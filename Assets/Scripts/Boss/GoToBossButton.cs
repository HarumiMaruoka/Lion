using System;
using UnityEngine;
using UnityEngine.UI;

public class GoToBossButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(GoToBoss);
    }

    private void GoToBoss()
    {
        var player = PlayerController.Current;
        if (!player)
        {
            Debug.LogWarning("Player is not find.");
            return;
        }

        player.BeginMoveCoroutine(PlayerController.MoveMode.GoToBoss);
    }
}