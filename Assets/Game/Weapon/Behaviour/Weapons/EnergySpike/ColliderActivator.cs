using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.EnergySpikeModules
{
    public class ColliderActivator : MonoBehaviour
    {
        public BulletCollider[] _colliders;

        private void Activation(int index)
        {
            _colliders[index].gameObject.SetActive(true);
        }
    }
}