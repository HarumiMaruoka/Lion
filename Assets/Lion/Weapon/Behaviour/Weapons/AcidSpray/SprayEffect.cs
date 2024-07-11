using Lion.Enemy;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.AcidSprayModule
{
    // 酸の噴射エフェクト
    public class SprayEffect : MonoBehaviour
    {
        [SerializeField]
        private Animator _sprayAnimator;
        [SerializeField]
        private Transform _acidSpotPosition;
        [SerializeField]
        private AcidSpot _acidSpotPrefab;

        public IWeaponParameter Parameter { get; set; }

        private AcidSpot _acidEffect;

        public void CreateAcidSpot() // アニメーションイベントから呼び出す。
        {
            _acidEffect = Instantiate(_acidSpotPrefab, _acidSpotPosition.position, Quaternion.identity);
            _acidEffect.Initialize(Parameter);
            // 酸のエフェクトが消えたら自身も消滅する。
            _acidEffect.OnDestroyed += () => Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                enemy.MagicDamage(Parameter.MagicPower, Parameter.Actor);
            }
        }
    }
}