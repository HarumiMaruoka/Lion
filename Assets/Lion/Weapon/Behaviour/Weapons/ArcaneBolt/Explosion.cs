using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.ArcaneBoltModule
{
    public class Explosion : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        public IActor Actor => Parameter == null ? null : Parameter.Actor;
        public float Damage => Parameter == null ? 10f : Parameter.MagicPower;
        public float Radius => Parameter == null ? 3f : Parameter.Range / 10f;

        private void Start()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, Radius);

            foreach (var collider in colliders)
            {
                if (EnemyManager.TryGetEnemy(collider.gameObject, out var enemy))
                {
                    enemy.MagicDamage(Damage, Actor);
                }
            }
        }

        private void OnParticleSystemStopped()
        {
            Destroy(gameObject);
        }
    }
}