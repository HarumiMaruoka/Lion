using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region Singleton
    private static PlayerController _current = null;
    public static PlayerController Current => _current;

    private void Awake()
    {
        _current = this;
    }

    private void OnDestroy()
    {
        _cancellationOnDestroy.Cancel();
        _current = null;
    }
    #endregion

    #region Fields and properties
    private Rigidbody2D _rigidbody2D;
    private CancellationTokenSource _cancellationOnDestroy = new CancellationTokenSource();

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    private void Start()
    {
        InitializeFields();
        BeginMoveCoroutine(_initialMoveMode);
        InitializeWeapons();
        _levelManager.Initialize();
    }

    private void InitializeFields()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _life = PlayerStatus.MaxLife;
        _lifeGage.Initialize(_life, _life, ref OnLifeChanged);
    }
    #endregion

    #region Status
    [Header("Status")]
    [SerializeField]
    private PlayerStatus _playerStatus;
    [SerializeField]
    private WeaponStatus _basicWeaponStatus;
    [SerializeField]
    private LifeGage _lifeGage;

    private float _life = 0f;

    public WeaponStatus WeaponStatus => _basicWeaponStatus;

    public event Action<float> OnLifeChanged;
    public event Action<PlayerController> OnDead;

    public PlayerStatus PlayerStatus
    {
        get
        {
            var sum = _playerStatus;
            return sum;
        }
    }

    public void Damage(float value)
    {
        _life -= value;
        OnLifeChanged?.Invoke(_life);

        if (_life <= 0)
        {
            OnDead?.Invoke(this);
        }
    }
    #endregion

    #region Move
    [SerializeField]
    private MoveMode _initialMoveMode;

    public enum MoveMode
    {
        Manual,
        Auto,
        GoToBoss,
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
    #endregion

    #region Level
    [Header("Level")]
    [SerializeField]
    private LevelManager _levelManager;

    public LevelManager LevelManager => _levelManager;

    public void CollectGem(Gem gem)
    {
        StartCoroutine(_levelManager.GetGem(gem));
    }
    #endregion

    #region Boss
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
    #endregion

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
    [Header("Weapon")]
    [SerializeField]
    private Transform _initialWeaponParent;
    [SerializeField]
    private Transform _weaponParent;
    public Transform WeaponParent => _weaponParent;

    [SerializeField]
    private WeaponBase[] _playerWeapons;

    public WeaponBase[] PlayerWeapons => _playerWeapons;

    public void InitializeWeapons()
    {
        for (int i = 0; i < _playerWeapons.Length; i++)
        {
            EquipWeapon(_playerWeapons[i], i);
        }
    }

    public WeaponBase EquipWeapon(WeaponBase equipWeapon, int index)
    {
        var old = _playerWeapons[index];
        if (old) old.Inactivate();

        _playerWeapons[index] = equipWeapon;
        if (equipWeapon) equipWeapon.Activate(_initialWeaponParent);

        return old;
    }
    #endregion
}