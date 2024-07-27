using Lion.Damage;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.BloodSickleModules
{
    public class Bullet : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        private float MagicPower => Parameter == null ? 15f : Parameter.PhysicalPower;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamagable damageable))
            {
                damageable.MagicDamage(MagicPower, Parameter?.Actor);
            }
        }
    }
}