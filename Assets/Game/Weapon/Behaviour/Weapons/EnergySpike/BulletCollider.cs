using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.EnergySpikeModules
{
    public class BulletCollider : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        private float PhysicalPower => Parameter == null ? 11f : Parameter.PhysicalPower;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                enemy.PhysicalDamage(PhysicalPower, Parameter?.Actor);
            }
        }
    }
}