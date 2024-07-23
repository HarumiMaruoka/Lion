using Lion.Enemy;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.LifeReaperModules
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D _collider2D;

        public IWeaponParameter Parameter { get; set; }

        public IActor Actor => Parameter == null ? PlayerController.Instance : Parameter.Actor;
        private float PhysicalDamage => Parameter == null ? 13f : Parameter.PhysicalPower;

        private float _startAngle = Mathf.PI;
        private float _endAngle = 0f;
        private float _duration = 0.3f;
        private float _timer;

        private void Start()
        {
            var offset = new Vector2(Mathf.Cos(_startAngle), Mathf.Sin(_startAngle)) * 3.5f;
            _collider2D.offset = offset;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            var t = _timer / _duration;
            var angle = Mathf.Lerp(_startAngle, _endAngle, t);

            var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 3.5f;
            _collider2D.offset = offset;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnemyManager.TryGetEnemy(collision.gameObject, out var enemy))
            {
                enemy.PhysicalDamage(PhysicalDamage, Parameter?.Actor);
                Actor.Heal(PhysicalDamage / 10f);
            }
        }
    }
}