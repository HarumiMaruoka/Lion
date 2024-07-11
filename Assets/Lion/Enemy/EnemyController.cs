using Lion.CameraUtility;
using Lion.Gem;
using Lion.Gold;
using Lion.Mission;
using Lion.UI;
using Lion.Damage;
using System;
using UnityEngine;
using Lion.Player;

namespace Lion.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour, IDamagable
    {
        public EnemyData EnemyData { get; set; }

        public event Action OnDead;

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void Initialize()
        {
            _hp = EnemyData.Life;
        }

        private void Update()
        {
            var playerPosition = PlayerController.Instance.transform.position;
            var direction = (playerPosition - transform.position).normalized;
            _rigidbody2D.velocity = direction * EnemyData.MoveSpeed;

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
            _hp -= physicalPower;
            DamageVFXPool.Instance.Create(transform.position, physicalPower);
            if (_hp <= 0) Die(true, actor);
        }

        public void MagicDamage(float magicPower, IActor actor)
        {
            _hp -= magicPower;
            DamageVFXPool.Instance.Create(transform.position, magicPower);
            if (_hp <= 0) Die(true, actor);
        }
    }
}