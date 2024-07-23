using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.GravityHammerModules
{
    public class Hammer : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GravitySpot _gravitySpotPrefab;
        [SerializeField]
        private Transform _gravitySpotSpawnPoint;

        private void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }

        private void CreateGravitySpot() // アニメーションイベントから呼び出す
        {
            var gravitySpot = Instantiate(_gravitySpotPrefab, _gravitySpotSpawnPoint.position, Quaternion.identity);
            gravitySpot.Parameter = Parameter;
        }
    }
}