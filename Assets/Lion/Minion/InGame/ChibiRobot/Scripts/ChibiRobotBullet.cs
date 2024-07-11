using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Minion
{
    public class ChibiRobotBullet : MinionBullet
    {
        private float _lifeTime = 2f;
        private float _speed = 10f;

        private void Start()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * _speed;
        }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out EnemyController enemy))
            {
                enemy.PhysicalDamage(Status.Attack, Controller);
            }
        }
    }
}