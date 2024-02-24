using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    private int _enemyID;
    private float _currentLife;
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

    [SerializeField]
    private float _moveSpeed = 8f;
    [SerializeField]
    private float _stoppingDistance = 2.0f;
    [SerializeField]
    private float _initialLife = 8f;

    private void MoveUpdate()
    {
        var playerPos = PlayerController.Current.transform.position;
        var directionToPlayer = playerPos - transform.position;

        if (directionToPlayer.sqrMagnitude > _stoppingDistance * _stoppingDistance)
            _rigidbody2D.velocity = directionToPlayer.normalized * _moveSpeed * TimeScale;
        else
            _rigidbody2D.velocity = Vector2.zero;
    }

    public void Damage(float value)
    {
        _currentLife -= value;

        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        VFXManager.Current.RequestDamageVFX(value, screenPos);

        if (_currentLife <= 0)
        {
            OnActorDeath();
        }
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

    [SerializeField]
    private float _attackPower = 4f;
    [SerializeField]
    private float _attackInterval = 0.4f;

    private float _attackElapsed = 0f;

    [Header("プレイヤーに攻撃できる距離。")]
    [SerializeField]
    private float _playerAttackableDistance;

    private void AttackUpdate()
    {
        var player = PlayerController.Current;
        if (!player) return;

        // プレイヤーと離れすぎていたら攻撃できない。
        var playerSqrDistance = (transform.position - player.transform.position).sqrMagnitude;
        if (playerSqrDistance > _playerAttackableDistance * _playerAttackableDistance)
        {
            _attackElapsed = 0f;
            return;
        }

        _attackElapsed += Time.deltaTime * TimeScale;
        if (_attackElapsed > _attackInterval)
        {
            _attackElapsed -= _attackInterval;
            player.Damage(_attackPower);
        }
    }

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

}