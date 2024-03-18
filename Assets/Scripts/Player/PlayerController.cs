using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IActor
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

    public string Name => "Player";
    public int Level => _levelManager.Level;

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

        _life = Status.MaxLife;
        _lifeGage.Initialize(_life, _life, ref OnLifeChanged);
    }
    #endregion

    #region Status
    [Header("Status")]
    [SerializeField]
    private ActorStatus _playerStatus;
    [SerializeField]
    private WeaponStatus _basicWeaponStatus;
    [SerializeField]
    private LifeGage _lifeGage;

    private List<ActorStatus> _playerStatusEffects = new List<ActorStatus>();
    private List<WeaponStatus> _weaponStatusEffects = new List<WeaponStatus>();

    public List<ActorStatus> PlayerStatusEffects => _playerStatusEffects;
    public List<WeaponStatus> WeaponStatusEffects => _weaponStatusEffects;

    public float BattlePower
    {
        get
        {
            float sum = 0f;
            sum += _playerStatus.BattlePower;
            foreach (var weapon in PlayerWeapons)
            {
                if (weapon) sum += weapon.BattlePower;
            }

            foreach (var character in EquipCharacterManager.Current.EquippedCharacters)
            {
                if (character && character.IndividualData != null)
                {
                    sum += character.IndividualData.BattlePower;

                    foreach (var weapon in character.IndividualData.EquippedWeapons)
                    {
                        if (weapon) sum += weapon.BattlePower;
                    }
                }
            }

            return sum;
        }
    }

    public ActorStatus Status
    {
        get
        {
            var sum = _playerStatus;

            for (int i = 0; i < _playerStatusEffects.Count; i++)
            {
                sum += _playerStatusEffects[i];
            }

            return sum;
        }
    }

    public WeaponStatus WeaponStatus
    {
        get
        {
            var sum = _basicWeaponStatus;

            for (int i = 0; i < _weaponStatusEffects.Count; i++)
            {
                sum += _weaponStatusEffects[i];
            }

            return sum;
        }
    }

    private float _life = 0f;

    public float Life => _life;

    public event Action<float> OnLifeChanged;
    public event Action<PlayerController> OnDead;

    public void Heal(float value)
    {
        _life += value;
        OnLifeChanged?.Invoke(_life);

        _life = Mathf.Clamp(_life, 0f, Status.MaxLife);
    }

    public void Damage(float value)
    {
        _life -= value;
        OnLifeChanged?.Invoke(_life);

        if (_life <= 0)
        {
            OnDead?.Invoke(this);
        }

        _life = Mathf.Clamp(_life, 0f, Status.MaxLife);
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

    public event Action<MoveMode> OnMoveModeChanged;

    private Coroutine _moveCoroutine;
    public MoveMode CurrentMoveMode { get; private set; }

    public void BeginMoveCoroutine(MoveMode moveMode)
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);

        CurrentMoveMode = moveMode;
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

        OnMoveModeChanged?.Invoke(moveMode);
    }

    [SerializeField]
    private VirtualJoystickUI _virtualJoystickUI;

    private Vector2 _touchBeginPos;

    private int _selectUICount = 0;

    public int SelectUICount => _selectUICount;

    public void OnUISelect()
    {
        _selectUICount++;
    }

    public void OnUIDeselect()
    {
        _selectUICount--;
    }

    private IEnumerator ManualMoveAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            // UI‘€ì’†‚Å‚ ‚ê‚Î–³ŒøB
            if (_selectUICount != 0)
            {
                yield return null;
                continue;
            }

            float x = 0f;
            float y = 0f;

            if (Input.touchCount == 1)
            {
                var touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    _touchBeginPos = touch.position;
                }

                var moveDir = (touch.position - _touchBeginPos).normalized;
                x = moveDir.x;
                y = moveDir.y;
            }

            x += Input.GetAxisRaw("Horizontal");
            y += Input.GetAxisRaw("Vertical");

            _rigidbody2D.velocity = new Vector2(x, y).normalized * Status.MoveSpeed * TimeScale;
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

            yield return AwaitArrivalAsync(startPos, targetPos, transform, () => _rigidbody2D.velocity = dir * Status.MoveSpeed * TimeScale, token);

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
    [Header("Boss")]
    [SerializeField]
    private Transform _bossBattlePosition;
    [SerializeField]
    private BossBattleAnimationController _bossBattleAnimationController;

    private IEnumerator GoToBoss(CancellationToken token)
    {
        var startPos = transform.position;
        var targetPos = _bossBattlePosition.position;
        if (startPos != targetPos)
        {
            var dir = (targetPos - startPos).normalized;
            yield return AwaitArrivalAsync(startPos, targetPos, transform, () => _rigidbody2D.velocity = dir * Status.MoveSpeed * TimeScale, token);
        }

        _rigidbody2D.velocity = Vector3.zero;
        _bossBattleAnimationController.Play();
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