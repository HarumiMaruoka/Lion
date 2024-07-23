using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// 射撃オブジェクト。
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class RangeAttackObject : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10;
        [SerializeField]
        private float _lifeTime = 5;

        private void Start()
        {
            var rigidbody2D = GetComponent<Rigidbody2D>();
            var direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            rigidbody2D.velocity = direction * _speed;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
            Destroy(gameObject, _lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == PlayerController.Instance.gameObject)
            {
                PlayerController.Instance.Damage(1f);
                Destroy(gameObject);
            }
        }
    }
}