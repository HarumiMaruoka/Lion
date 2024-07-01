using Lion.CameraUtility;
using Lion.UI;
using System;
using UnityEngine;

namespace Lion.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour
    {
        public EnemyData EnemyData { get; set; }

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void Initialize()
        {
            _hp = EnemyData.HP;
        }

        private void Update()
        {
            var playerPosition = Player.PlayerController.Instance.transform.position;
            var direction = (playerPosition - transform.position).normalized;
            _rigidbody2D.velocity = direction * EnemyData.MoveSpeed;

            if (Camera.main.IsTooFarFromCamera(transform.position)) Die();
        }

        private float _hp = 10f;
        public void Damage(float value)
        {
            _hp -= value;
            DamageVFXPool.Instance.Create(transform.position, value);
            if (_hp <= 0) Die();
        }

        private void Die()
        {
            EnemyManager.Instance.EnemyPool.ReturnEnemy(this);
        }
    }
}