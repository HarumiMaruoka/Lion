using Lion.CameraUtility;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Gold
{
    public class DroppedGold : MonoBehaviour
    {
        public DroppedGoldPool Pool { get; set; }

        private IActor _collector;

        private int _amount; // 取得することで入手できるゴールドの量。
        private float _period; // 命中までの時間

        private Vector3 _velocity;
        private Vector3 _position;

        private Transform Target => _collector == null ? PlayerController.Instance.transform : _collector.transform;

        public void Initialize(IActor collector, Vector3 position, int amount)
        {
            _collector = collector;
            transform.position = position;
            _position = transform.position;
            _period = 1f;
            _amount = amount;

            // ランダムな方向に初速度を与える
            var x = Mathf.Cos(UnityEngine.Random.Range(0, 2 * Mathf.PI));
            var y = Mathf.Sin(UnityEngine.Random.Range(0, 2 * Mathf.PI));
            var length = UnityEngine.Random.Range(1f, 3f);

            _velocity = new Vector3(x, y, 0) * length;
        }

        private void Update()
        {
            _period -= Time.deltaTime;
            if (_period <= 0f)
            {
                Fire();
            }

            var acceleration = Vector3.zero;

            Vector3 diff;
            if (Target) diff = Target.position - _position;
            else diff = Vector3.zero;

            acceleration += (diff - _velocity * _period) * 2 / (_period * _period);

            _velocity += acceleration * Time.deltaTime;
            _position += _velocity * Time.deltaTime;

            transform.position = _position;
        }

        private void Fire()
        {
            GoldManager.Instance.Earn(_amount);
            _collector?.CollectGold(_amount);
            Pool.Deactivate(this);
        }
    }
}