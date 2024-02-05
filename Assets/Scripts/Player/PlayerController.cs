using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController _current = null;
    public static PlayerController Current => _current;

    [Header("Basic Information")]
    [SerializeField]
    private PlayerStatus _playerStatus;
    [SerializeField]
    private WeaponStatus _basicWeaponStatus;

    private Rigidbody2D _rigidbody2D;
    private CancellationTokenSource _cancellationOnDestroy = new CancellationTokenSource();

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    public WeaponStatus WeaponStatus => _basicWeaponStatus;

    [SerializeField]
    private MoveMode _initialMoveMode;

    public event Action<PlayerStatus> OnPlayerStatusChanged;
    private void OnPlayerStatusChange(WeaponBase weapon)
    {
        OnPlayerStatusChanged?.Invoke(PlayerStatus);
    }

    public PlayerStatus PlayerStatus
    {
        get
        {
            var sum = _playerStatus;
            var inventory = WeaponInventory.Current;
            if (!inventory)
            {
                Debug.Log("Weapon Inventory is Missing...");
                return default;
            }

            return sum;
        }
    }

    private float _life = 0f;

    private void Awake()
    {
        _current = this;
    }

    private void Start()
    {
        InitializeFields();
        BeginMoveCoroutine(_initialMoveMode);

        _levelManager.Initialize();
    }

    private void InitializeFields()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _life = PlayerStatus.MaxLife;
        _lifeGage.Initialize(_life, _life, ref OnLifeChanged);
    }

    private Coroutine _moveCoroutine;

    public void BeginMoveCoroutine(MoveMode moveMode)
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);

        if (moveMode == MoveMode.Manual)
        {
            _moveCoroutine = StartCoroutine(ManualMoveAsync(_cancellationOnDestroy.Token));
        }
        else if (moveMode == MoveMode.Auto)
        {
            _moveCoroutine = StartCoroutine(AutoMoveAsync(_cancellationOnDestroy.Token));
        }
        else if (moveMode == MoveMode.GoToBoss)
        {
            _moveCoroutine = StartCoroutine(GoToBoss(_cancellationOnDestroy.Token));
        }
    }

    private void OnDestroy()
    {
        _cancellationOnDestroy.Cancel();
        _current = null;
    }

    private IEnumerator ManualMoveAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            _rigidbody2D.velocity = new Vector2(h, v).normalized * PlayerStatus.MoveSpeed * TimeScale;
            yield return null;
        }
    }

    [SerializeField]
    private LifeGage _lifeGage;

    public event Action<float> OnLifeChanged;
    public event Action<PlayerController> OnDead;

    public void Damage(float value)
    {
        _life -= value;
        OnLifeChanged?.Invoke(_life);

        if (_life <= 0)
        {
            OnDead?.Invoke(this);
        }
    }

    [SerializeField]
    private LevelManager _levelManager;

    public LevelManager LevelManager => _levelManager;

    public void CollectGem(Gem gem)
    {
        StartCoroutine(_levelManager.GetGem(gem));
    }

    [SerializeField]
    private WayPointManager _wayPointManager;

    private IEnumerator AutoMoveAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Vector3 startPos = this.transform.position;
            Vector3 targetPos = _wayPointManager.CurrentTarget.position;
            var dir = (targetPos - startPos).normalized;

            yield return AwaitArrivalAsync(startPos, targetPos, transform, () => _rigidbody2D.velocity = dir * PlayerStatus.MoveSpeed * TimeScale, token);

            _wayPointManager.OnNext();
        }
    }

    [SerializeField]
    private Transform _bossBattlePosition;
    [SerializeField]
    private BossCutscene _cutscene;

    private IEnumerator GoToBoss(CancellationToken token)
    {
        var startPos = transform.position;
        var targetPos = _bossBattlePosition.position;
        var dir = (targetPos - startPos).normalized;

        yield return AwaitArrivalAsync(startPos, targetPos, transform, () => _rigidbody2D.velocity = dir * PlayerStatus.MoveSpeed * TimeScale, token);

        _rigidbody2D.velocity = Vector3.zero;
        _cutscene.PlayAnimation();
    }

    public enum MoveMode
    {
        Manual,
        Auto,
        GoToBoss,
    }

    private IEnumerator AwaitArrivalAsync(Vector3 startPos, Vector3 endPos, Transform origin, Action onUpdate, CancellationToken token)
    {
        transform.position = startPos;

        if (startPos.x <= endPos.x && startPos.y <= endPos.y) // ‰Eã
        {
            while (!token.IsCancellationRequested &&
                   origin.position.x <= endPos.x && origin.position.y <= endPos.y)
            {
                onUpdate?.Invoke();
                yield return null;
            }
        }
        else if (startPos.x >= endPos.x && startPos.y <= endPos.y) // ¶ã
        {
            while (!token.IsCancellationRequested &&
                   origin.position.x >= endPos.x && origin.position.y <= endPos.y)
            {
                onUpdate?.Invoke();
                yield return null;
            }
        }
        else if (startPos.x <= endPos.x && startPos.y >= endPos.y) // ‰E‰º
        {
            while (!token.IsCancellationRequested &&
                   origin.position.x <= endPos.x && origin.position.y >= endPos.y)
            {
                onUpdate?.Invoke();
                yield return null;
            }
        }
        else if (startPos.x >= endPos.x && startPos.y >= endPos.y) // ¶‰º
        {
            while (!token.IsCancellationRequested &&
                   origin.position.x >= endPos.x && origin.position.y >= endPos.y)
            {
                onUpdate?.Invoke();
                yield return null;
            }
        }

        transform.position = endPos;
    }

    #region Teleport
    [Header("Teleport")]
    [SerializeField]
    private Image _teleportFadeImage;
    [SerializeField]
    private float _teleportFadeInDuration;
    [SerializeField]
    private float _teleportFadeOutDuration;

    public Coroutine RequestTeleport(Vector3 position, MoveMode teleportedMoveMode = MoveMode.Manual)
    {
        return StartCoroutine(TeleportAsync(position, teleportedMoveMode));
    }

    public IEnumerator TeleportAsync(Vector3 position, MoveMode teleportedMoveMode = MoveMode.Manual)
    {
        // Fade In
        yield return _teleportFadeImage.FadeAsync(1f, _teleportFadeInDuration);
        // Change Position
        transform.position = position;
        // Fade Out
        yield return _teleportFadeImage.FadeAsync(0f, _teleportFadeInDuration);
        BeginMoveCoroutine(teleportedMoveMode);
    }
    #endregion

    #region Weapon
    [SerializeField]
    private Transform _weaponParent;
    public Transform WeaponParent => _weaponParent;
    #endregion
}