using System;
using UnityEngine;
using UnityEngine.UI;

public class GoToBossButton : MonoBehaviour
{
    [SerializeField]
    private BossController _bossController;

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _label;

    [SerializeField]
    private Color _validImageColor = Color.white;
    [SerializeField]
    private Color _invalidImageColor = Color.gray;

    private void Start()
    {
        UpdateLabel();
        _bossController.OnDeadEnemyCountChanged += _ => UpdateLabel();
        _button.onClick.AddListener(GoToBoss);
    }

    private void GoToBoss()
    {
        if (!_bossController.IsChallengeable) return;

        var player = PlayerController.Current;
        if (!player)
        {
            Debug.LogWarning("Player is not found.");
            return;
        }

        player.BeginMoveCoroutine(PlayerController.MoveMode.GoToBoss);
    }

    private void UpdateLabel()
    {
        if (_bossController.IsChallengeable)
        {
            _button.image.color = _validImageColor;
            _label.text = _bossController.Name + "に挑戦可能。";
        }
        else
        {
            _button.image.color = _invalidImageColor;
            var remainingEnemyCount = _bossController.DefeatRequiredEnemyCount - _bossController.DefeatedEnemyCount;
            _label.text = $"{_bossController.Name}に挑戦できるまで\nあと{remainingEnemyCount}体の敵を倒す必要があります。";
        }
    }
}