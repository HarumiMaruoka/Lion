using Lion.CameraUtility;
using Lion.Gem;
using Lion.Gold;
using Lion.Mission;
using Lion.UI;
using Lion.Damage;
using System;
using UnityEngine;
using Lion.Player;
using Cysharp.Threading.Tasks;

namespace Lion.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour, IDamagable
    {
        public EnemyData EnemyData { get; set; }
        public event Action OnDead;

        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize()
        {
            _hp = EnemyData.Life;
        }

        private void Update()
        {
            var playerPosition = PlayerController.Instance.transform.position;
            var direction = (playerPosition - transform.position).normalized;
            Rigidbody2D.velocity = direction * EnemyData.MoveSpeed;

            if (IsFreezed) Rigidbody2D.velocity = Vector2.zero;

            if (Camera.main.IsTooFarFromCamera(transform.position)) Die(false, null);
        }

        private void Die(bool isKill, IActor actor)
        {
            if (isKill)
            {
                if (actor == null) actor = PlayerController.Instance;

                DroppedGemPool.Instance.CreateDroppedGem(actor, transform.position, EnemyData.Exp);
                DroppedGoldPool.Instance.CreateDroppedGold(actor, transform.position, EnemyData.Gold);
                MainMission.Instance.KillCount++;
            }

            EnemyManager.Instance.EnemyPool.ReturnEnemy(this);
            OnDead?.Invoke();
        }

        private float _hp = 10f;

        public void PhysicalDamage(float physicalPower, IActor actor)
        {
            var old = _hp;
            _hp -= physicalPower;
            OnDamaged?.Invoke();
            DamageVFXPool.Instance.Create(transform.position, physicalPower);
            if (old > 0 && _hp <= 0) Die(true, actor);
        }

        public void MagicDamage(float magicPower, IActor actor)
        {
            var old = _hp;
            _hp -= magicPower;
            OnDamaged?.Invoke();
            DamageVFXPool.Instance.Create(transform.position, magicPower);
            if (old > 0 && _hp <= 0) Die(true, actor);
        }

        public Action OnDamaged;

        private int _freezeCount = 0;

        public bool IsFreezed => _freezeCount > 0;

        public void Freeze()
        {
            if (_freezeCount == 0)
            {
                Animator.enabled = false; // アニメーションを停止
            }

            _freezeCount++;
        }

        public void Unfreeze()
        {
            _freezeCount--;

            if (_freezeCount == 0)
            {
                Animator.enabled = true; // アニメーションを再開
            }
        }
    }
}