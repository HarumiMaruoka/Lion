using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.EnergySpikeModules
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public BulletCollider[] Colliders;

        private void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}