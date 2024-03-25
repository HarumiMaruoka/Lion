using System;
using System.Threading;
using UnityEngine;
using Character;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    private int _enemyID;
    private Rigidbody2D _rigidbody2D;
    private CancellationTokenSource _cancellationOnDestroy = new CancellationTokenSource();

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    public int EnemyID => _enemyID;
    public event Action<int, EnemyController> OnDead;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(int enemyID, Vector3 position)
    {
        _enemyID = enemyID;
        transform.position = position;
        _attackElapsed = 0f;
        _currentLife = _initialLife;
        _ailments = Ailments.None;
    }

    private void Update()
    {
        MoveUpdate();
        AttackUpdate();

        if (!this.IsInGameZone()) // GameZoneの外に出たら死亡。
        {
            OnDead?.Invoke(_enemyID, this);
        }
    }

    private void OnDisable()
    {
        _cancellationOnDestroy.Cancel();
    }

    private Ailments _ailments = Ailments.None;

    #region Life Control

    private float _currentLife;

    public void Damage(IActor attackActor, float value)
    {
        // ライフを減らす。
        _currentLife -= value;

        // ダメージVFXを表示する。
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        VFXManager.Current.RequestDamageVFX(value, screenPos);

        // ライフが0以下であれば死亡。
        if (_currentLife <= 0)
        {
            OnActorDeath();
        }

        // 必要に応じて状態異常になる。
        if (attackActor == null) return;

        if (attackActor.Ability.HasFlag(Ability.Burn))
        {
            ApplyBurn(3f, 0.5f);
        }

    }

    public void Kill()
    {
        OnActorDeath();
    }

    private void OnActorDeath()
    {
        OnDead?.Invoke(_enemyID, this);
        GemManager.Current.CreateGem(transform.position);
        DropItem();
        DropWeapon();
        DropCharacter();
        DropCookingMaterial();
    }
    #endregion

    #region Movement

    #region Core Modules
    [SerializeField]
    private float _moveSpeed = 8f;
    [SerializeField]
    private float _stoppingDistance = 2.0f;
    [SerializeField]
    private float _initialLife = 8f;

    private Vector3 _targetPosition;

    private void MoveUpdate()
    {
        if (_ailments.HasFlag(Ailments.Confused)) // 混乱時の移動。
        {
            // 目的地を更新しない。
        }
        else
        {
            _targetPosition = PlayerController.Current.transform.position;
        }


        var targetVector = _targetPosition - transform.position;
        if (targetVector.sqrMagnitude > _stoppingDistance * _stoppingDistance)
        {
            var adjustedMoveSpeed = _moveSpeed;
            // 移動速度に関するバフやデバフ効果を適用。
            foreach (var effect in _moveSpeedEffects)
            {
                adjustedMoveSpeed *= effect;
            }

            // 物理システムに移動ベクトルを適用。
            _rigidbody2D.velocity = targetVector.normalized * adjustedMoveSpeed * TimeScale;
        }
        else
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    #endregion

    #region Effect
    private List<float> _moveSpeedEffects = new List<float>();

    private static List<int> _startFrame = new List<int>();
    private static List<int> _endFrame = new List<int>();

    public async void ApplyMoveSpeedEffect(float amount, int flameCount = 1)
    {
        _moveSpeedEffects.Add(amount);

        await UniTask.DelayFrame(flameCount + 1);

        _moveSpeedEffects.Remove(amount);
    }

    public async void ApplyMoveSpeedEffect(float amount, float duration)
    {
        _moveSpeedEffects.Add(amount);

        for (float t = 0f; t < duration; t += Time.deltaTime * TimeScale)
        {
            await UniTask.Yield();
        }

        _moveSpeedEffects.Remove(amount);
    }
    #endregion

    #endregion

    #region Attack

    [SerializeField]
    private float _attackPower = 4f;
    [SerializeField]
    private float _attackInterval = 0.4f;

    private float _attackElapsed = 0f;

    [SerializeField]
    private float _attackableDistance;

    private void AttackUpdate()
    {
        _attackElapsed += Time.deltaTime * TimeScale;

        if (_attackElapsed > _attackInterval)
        {
            PlayerAttack();
            ConfusedAttack();

            _attackElapsed -= _attackInterval;
        }
    }

    private void PlayerAttack()
    {
        if (!PlayerController.Current) return;

        // プレイヤーと離れすぎていたら攻撃できない。
        var sqrDistance = (transform.position - PlayerController.Current.transform.position).sqrMagnitude;
        if (sqrDistance > _attackableDistance * _attackableDistance)
        {
            return;
        }

        PlayerController.Current.Damage(_attackPower);

    }

    private void ConfusedAttack()
    {
        if (!_ailments.HasFlag(Ailments.Confused)) return;

        var nearestEnemy = EnemyManager.Current.GetNearestEnemy(transform.position);
        if (!nearestEnemy) return;

        var sqrDistance = (transform.position - nearestEnemy.transform.position).sqrMagnitude;
        if (sqrDistance > _attackableDistance * _attackableDistance) return;

        nearestEnemy.Damage(null, _attackPower);
    }
    #endregion

    #region Drop Item

    [SerializeField]
    private DropItemInfo[] _dropItems = null;
    [SerializeField]
    private DropWeaponInfo[] _dropWeapons = null;
    [SerializeField]
    private DropCharacterInfo[] _dropCharacters = null;
    [SerializeField]
    private DropCookingMaterialInfo[] _dropCookingMaterials = null;

    public void DropItem()
    {
        foreach (var item in _dropItems)
        {
            ItemManager.Current.DropItem(transform.position, item.ItemID, item.Probability);
        }
    }

    public void DropWeapon()
    {
        foreach (var item in _dropWeapons)
        {
            WeaponManager.Current.DropWeapon(transform.position, item.WeaponType, item.Probability);
        }
    }

    public void DropCharacter()
    {
        foreach (var item in _dropCharacters)
        {
            CharacterManager.Current.DropCharacter(transform.position, item.CharacterID, item.Probability);
        }
    }

    public void DropCookingMaterial()
    {
        foreach (var item in _dropCookingMaterials)
        {
            DroppedCookingMaterialManager.Current.DropCookingMaterial(transform.position, item.CookingMaterialID, item.Probability);
        }
    }

    [Serializable]
    public struct DropItemInfo
    {
        [SerializeField]
        private int _itemID;
        [SerializeField]
        private float _probability;

        public int ItemID => _itemID;
        public float Probability => _probability;
    }

    [Serializable]
    public struct DropWeaponInfo
    {
        [SerializeField]
        private WeaponType _weaponType;
        [SerializeField]
        private float _probability;

        public WeaponType WeaponType => _weaponType;
        public float Probability => _probability;
    }

    [Serializable]
    public struct DropCharacterInfo
    {
        [SerializeField]
        private int _characterID;
        [SerializeField]
        private float _probability;

        public int CharacterID => _characterID;
        public float Probability => _probability;
    }

    [Serializable]
    public struct DropCookingMaterialInfo
    {
        [SerializeField]
        private int _cookingMaterialID;
        [SerializeField]
        private float _probability;

        public int CookingMaterialID => _cookingMaterialID;
        public float Probability => _probability;
    }
    #endregion

    public void ApplyConfusion(float duration)
    {
        StartCoroutine(ConfusionAsync(duration));
    }

    private IEnumerator ConfusionAsync(float duration)
    {
        // 開始処理
        _ailments |= Ailments.Confused;

        // 更新処理
        for (float t = 0f; t < duration; t += Time.deltaTime * TimeScale)
        {
            yield return null;
        }

        // 終了処理
        _ailments &= ~Ailments.Confused;
    }

    public void ApplyBurn(float duration, float cooldown)
    {
        if (gameObject.activeSelf)
            StartCoroutine(BurnAsync(duration, cooldown));
    }

    private IEnumerator BurnAsync(float duration, float cooldown)
    {
        float cooldownElapsed = 0f;
        for (float t = 0f; t < duration; t += Time.deltaTime * TimeScale)
        {
            cooldownElapsed += Time.deltaTime * TimeScale;
            if (cooldownElapsed > cooldown)
            {
                cooldownElapsed = 0f;
                Damage(null, _currentLife / 10f); // 現在ライフの10分の1のダメージ。
            }
            yield return null;
        }
    }
}

[Flags]
public enum Ailments
{
    None = 0,
    Everything = -1,
    /// <summary>
    /// 混乱
    /// ・プレイヤーの方に移動しなくなる。（未実装）
    /// ・関係ない方向に移動するようになる。（未実装）
    /// ・敵同士で攻撃するようになる。（未実装）
    /// </summary>
    Confused = 1,
}