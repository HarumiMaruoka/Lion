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
            int instanceID = collision.gameObject.GetInstanceID();
            if (EnemyManager.Instance.EnemyPool.TryGetEnemy(instanceID, out EnemyController enemy))
            {
                enemy.Damage(AllyController.Status.AttackPower);
            }
        }
    }
}