using System;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleAnimationController : MonoBehaviour
{
    [SerializeField]
    private GameObject _animationWindow;

    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private BossController _bossController;

    [SerializeField]
    private Animator _bossBattleAnimator;

    [SerializeField]
    private string _beginAnimationName;
    [SerializeField]
    private string _isPlayerStrongerAnimationParameterName;

    [SerializeField]
    private Transform _playerDefeatedTeleportPosition;

    [SerializeField]
    private Text _playerBattlePowerLabel;
    [SerializeField]
    private Text _bossBattlePowerLabel;

    private void Start()
    {
        _animationWindow.SetActive(false);
    }

    bool _isPlayerVictoried;

    public void Play()
    {
        _isPlayerVictoried = _bossController.IsPlayerStronger;

        _playerBattlePowerLabel.text = _playerController.BattlePower.ToString();
        _bossBattlePowerLabel.text = _bossController.BattlePower.ToString();

        _animationWindow.SetActive(true);
        _bossBattleAnimator.SetBool(_isPlayerStrongerAnimationParameterName, _isPlayerVictoried);
        _bossBattleAnimator.Play(_beginAnimationName);
    }

    public void OnAnimationCompleteEvent() // Unityアニメーションイベントから呼び出す。
    {
        _animationWindow.SetActive(false);

        if (_isPlayerVictoried)
        {
            _playerController.BeginMoveCoroutine(PlayerController.MoveMode.Manual);
        }
        else
        {
            _playerController.RequestTeleport(_playerDefeatedTeleportPosition.position, PlayerController.MoveMode.Manual);
        }
    }
}