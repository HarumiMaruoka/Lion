using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Ally
{
    public class AttackArea : MonoBehaviour
    {
        public AllyController AllyController { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = EnemyManager.Instance.EnemyPool.FindEnemy(collision.gameObject.GetInstanceID());
            if (enemy == null) return;
            enemy.Damage(AllyController.Status.AttackPower);
        }
    }
}