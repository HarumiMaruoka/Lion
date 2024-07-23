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
            if (EnemyManager.TryGetEnemy(collision.gameObject, out EnemyController enemy))
            {
                enemy.PhysicalDamage(AllyController.Status.AttackPower, AllyController);
            }
        }
    }
}