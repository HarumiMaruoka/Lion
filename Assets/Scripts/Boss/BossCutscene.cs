using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossCutscene : MonoBehaviour
{
    public void PlayAnimation()
    {
        StopAllCoroutines(); // 既に再生中の全てのコルーチンを停止する。
        StartCoroutine(RunSequential());
    }

    // 背景フェードイン → アクターフェードイン → プレイヤー攻撃アニメーション →
    // 結果表示（勝ち負けで異なる演出）→ 全体フェードアウト
    private IEnumerator RunSequential()
    {
        CacheInitialValues();

        yield return BackgroundFadeIn();
        yield return ActorFadeIn();
        yield return PlayerAttackAnimation();
        yield return ShowResult();
        yield return AllFadeOut();

        ApplyCacheValues();
        FinishedCutscene();
    }

    [SerializeField]
    private BossManager _bossManager;
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _playerImage;
    [SerializeField]
    private Image _enemyImage;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    private bool IsWin // プレイヤーの勝ちかどうか表現するプロパティ
    {
        get
        {
            var player = PlayerController.Current;
            if (!player)
            {
                Debug.LogWarning("PlayerController is not find");
                return false;
            }

            // プレイヤーのステータスがボスのステータスを上回ったら
            // 勝ちとして、trueを返す。
            if (player.PlayerStatus.Sum > _bossManager.BossStatus) return true;

            return false;
        }
    }

    [Header(" ========== Background Fade In ========== ")]
    [SerializeField]
    private Vector2 _backgroundFadeInStartPos;
    [SerializeField]
    private Vector2 _backgroundFadeInEndPos;
    [SerializeField]
    private float _backgroundFadeInDuration;

    private IEnumerator BackgroundFadeIn()
    {
        var startPos = _backgroundFadeInStartPos;
        var endPos = _backgroundFadeInEndPos;
        _background.rectTransform.anchoredPosition = startPos;

        for (float t = 0f; t < _backgroundFadeInDuration; t += Time.deltaTime * TimeScale)
        {
            _background.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t / _backgroundFadeInDuration);
            yield return null;
        }

        _background.rectTransform.anchoredPosition = endPos;
    }

    [Header(" ========== Actor Fade In ========== ")]
    [SerializeField]
    private Vector2 _playerFadeInStartPos;
    [SerializeField]
    private Vector2 _playerFadeInEndPos;

    [SerializeField]
    private Vector2 _enemyFadeInStartPos;
    [SerializeField]
    private Vector2 _enemyFadeInEndPos;

    [SerializeField]
    private float _actorFadeInDuration;
    [SerializeField]
    private float _actorFadeInWaitTime;

    private IEnumerator ActorFadeIn()
    {
        var playerStartPos = _playerFadeInStartPos;
        var playerEndPos = _playerFadeInEndPos;
        var enemyStartPos = _enemyFadeInStartPos;
        var enemyEndPos = _enemyFadeInEndPos;

        _playerImage.rectTransform.anchoredPosition = playerStartPos;
        _enemyImage.rectTransform.anchoredPosition = enemyStartPos;

        for (float t = 0f; t < _actorFadeInDuration; t += Time.deltaTime * TimeScale)
        {
            _playerImage.rectTransform.anchoredPosition = Vector2.Lerp(playerStartPos, playerEndPos, t / _actorFadeInDuration);
            _enemyImage.rectTransform.anchoredPosition = Vector2.Lerp(enemyStartPos, enemyEndPos, t / _actorFadeInDuration);
            yield return null;
        }

        _playerImage.rectTransform.anchoredPosition = playerEndPos;
        _enemyImage.rectTransform.anchoredPosition = enemyEndPos;

        for (float t = 0f; t < _actorFadeInWaitTime; t += Time.deltaTime * TimeScale)
        {
            yield return null;
        }
    }

    [Header(" ========== Player Attack Animation ========== ")]
    [SerializeField]
    private float _attackDuration;
    [SerializeField]
    private float _attackInterval;
    [SerializeField]
    private int _attackCount;
    [SerializeField]
    private RectTransform[] _attackPoints;

    private IEnumerator PlayerAttackAnimation()
    {
        _attackPoints.Shuffle();
        for (int i = 0; i < _attackCount; i++)
        {
            var targetPoint = _attackPoints[i % _attackPoints.Length];
            var startPos = _playerImage.rectTransform.anchoredPosition;
            var endPos = targetPoint.anchoredPosition;

            for (float t = 0f; t < _attackDuration; t += Time.deltaTime * TimeScale)
            {
                _playerImage.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t / _attackDuration);
                yield return null;
            }
            for (float t = 0f; t < _attackInterval; t += Time.deltaTime * TimeScale) yield return null;
        }
    }

    private IEnumerator ShowResult()
    {
        if (IsWin) yield return WinAnimation();
        else yield return LoseAnimation();
    }

    [Header(" ========== Win Animation ========== ")]
    [SerializeField]
    private Transform _winAnimationTargetPosition;
    [SerializeField]
    private float _winAnimationDuration;
    [SerializeField]
    private Vector3 _winAnimationInitialVelocity;
    [SerializeField]
    private Image _winImage;
    [SerializeField]
    private float _winImageFadeInDuration;
    [SerializeField]
    private float _winAnimationAfterWaitTime;

    [SerializeField]
    private BossVictoryWindowController _bossVictoryWindowController;

    private IEnumerator WinAnimation()
    {
        yield return ThrowAnimationExtensions.ThrowAnimationAsync(
            _playerImage.transform, _winAnimationTargetPosition, _winAnimationDuration, _winAnimationInitialVelocity);
        yield return _winImage.FadeAsync(1f, _winImageFadeInDuration);
        for (float t = 0f; t < _winAnimationAfterWaitTime; t += Time.deltaTime * TimeScale) yield return null;

        // ドロップアイテム確認ウィンドウ系の操作。
        yield return _bossVictoryWindowController.ShowAsync(); // ウィンドウを表示。
        yield return _bossVictoryWindowController.WaitCloseRequest(); // ウィンドウを閉じる要求を待機。
        yield return _bossVictoryWindowController.HideAsync(); // ウィンドウを閉じる。
    }

    [Header(" ========== Lose Animation ========== ")]
    [SerializeField]
    private float _loseAnimationBeforeWaitTime;
    [SerializeField]
    private Transform _loseAnimationTargetPosition;
    [SerializeField]
    private float _loseAnimationDuration;
    [SerializeField]
    private Vector3 _loseAnimationInitialVelocity;

    private IEnumerator LoseAnimation()
    {
        for (float t = 0f; t < _loseAnimationBeforeWaitTime; t += Time.deltaTime * TimeScale) yield return null;

        var rotateAnimationCoroutine = StartCoroutine(CutsceneAnimationExtensions.RotateAnimationAsync(_playerImage.transform, _loseAnimationDuration, 12));
        var throwAnimationCoroutine = StartCoroutine(ThrowAnimationExtensions.ThrowAnimationAsync(
            _playerImage.transform, _loseAnimationTargetPosition, _loseAnimationDuration, _loseAnimationInitialVelocity));

        yield return rotateAnimationCoroutine;
        yield return throwAnimationCoroutine;
    }

    [Header(" ========== All Fade Out ========== ")]
    [SerializeField]
    private float _allFadeOutDuration;
    [SerializeField]
    private PlayerController.MoveMode _winedMoveMode;
    [SerializeField]
    private PlayerController.MoveMode _losedMoveMode;
    [SerializeField]
    private Transform _losedTeleportPosition;

    private IEnumerator AllFadeOut()
    {
        var bg = StartCoroutine(_background.FadeAsync(0f, _allFadeOutDuration));
        var player = StartCoroutine(_playerImage.FadeAsync(0f, _allFadeOutDuration));
        var enemy = StartCoroutine(_enemyImage.FadeAsync(0f, _allFadeOutDuration));
        var winImage = StartCoroutine(_winImage.FadeAsync(0f, _allFadeOutDuration));

        yield return bg;
        yield return player;
        yield return enemy;
        yield return winImage;

        if (IsWin)
        {
            _playerController.BeginMoveCoroutine(_winedMoveMode);
        }
        else
        {
            _playerController.RequestTeleport(_losedTeleportPosition.position, _losedMoveMode);
        }
    }

    private CachedData _cachedData;

    private void CacheInitialValues()
    {
        _cachedData = new CachedData();

        _cachedData._backgroundPosition = _background.rectTransform.anchoredPosition;
        _cachedData._playerPosition = _playerImage.rectTransform.anchoredPosition;
        _cachedData._enemyPosition = _enemyImage.rectTransform.anchoredPosition;
        _cachedData._winPosition = _winImage.rectTransform.anchoredPosition;

        _cachedData._backgroundColor = _background.color;
        _cachedData._playerColor = _playerImage.color;
        _cachedData._enemyColor = _enemyImage.color;
        _cachedData._winColor = _winImage.color;

        _cachedData._playerRotation = _playerImage.rectTransform.rotation;
    }

    private void ApplyCacheValues()
    {
        _background.rectTransform.anchoredPosition = _cachedData._backgroundPosition;
        _playerImage.rectTransform.anchoredPosition = _cachedData._playerPosition;
        _enemyImage.rectTransform.anchoredPosition = _cachedData._enemyPosition;
        _winImage.rectTransform.anchoredPosition = _cachedData._winPosition;

        _background.color = _cachedData._backgroundColor;
        _playerImage.color = _cachedData._playerColor;
        _enemyImage.color = _cachedData._enemyColor;
        _winImage.color = _cachedData._winColor;

        _playerImage.rectTransform.rotation = _cachedData._playerRotation;
    }

    public event Action OnFinishedCutscene;

    private void FinishedCutscene()
    {
        OnFinishedCutscene?.Invoke();
    }

    public struct CachedData
    {
        public Vector2 _backgroundPosition;
        public Vector2 _playerPosition;
        public Vector2 _enemyPosition;
        public Vector2 _winPosition;

        public Color _backgroundColor;
        public Color _playerColor;
        public Color _enemyColor;
        public Color _winColor;

        public Quaternion _playerRotation;
    }
}