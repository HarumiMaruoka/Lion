using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour.MetallicSwarmModules
{
    // プレイヤーの周囲を飛び回るドローン。
    // 定期的にミサイルを発射し、最後に自爆する。
    public class Drone : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        private int _missileCount = 10; // ミサイルの発射回数
        private float _missileInterval = 0.5f; // ミサイルの発射間隔
        private int _missileIndex; // ミサイルの発射回数カウンタ
        private float _missileTimer; // ミサイルの発射間隔カウンタ

        private Vector3 Center => transform.parent.position;

        private void Start()
        {
            MissileInit();
            MoveInit();
        }

        private void Update()
        {
            MissleUpdate();
            MoveUpdate();
        }

        [SerializeField]
        private float _speed = 1f;

        private float _radiusX = 2f;
        private float _radiusY = 5f;

        private void MoveInit()
        {
            _angleOffset = UnityEngine.Random.Range(0, Mathf.PI * 2f);
            MoveUpdate();
        }

        private float _angleOffset;

        private void MoveUpdate()
        {
            transform.position = Center + new Vector3(
                Mathf.Sin(_angleOffset + Time.time * _speed) * _radiusX,
                Mathf.Cos(_angleOffset + Time.time * _speed) * _radiusY,
                0
            );

            transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(Mathf.Cos(Time.time * _speed), Mathf.Sin(Time.time * _speed)) * Mathf.Rad2Deg
            );
        }

        private void MissileInit()
        {
            _missileTimer = _missileInterval;
        }

        private void MissleUpdate()
        {
            _missileTimer -= Time.deltaTime;
            if (_missileTimer <= 0)
            {
                Fire();
                _missileTimer = _missileInterval;
            }
        }

        private void Fire()
        {
            _missileIndex++;
            if (_missileIndex > _missileCount)
            {
                Explode();
            }
            else
            {
                LaunchMissile();
            }
        }

        [SerializeField]
        private Missile _missilePrefab;

        private void LaunchMissile()
        {
            var missile = Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            missile.Parameter = Parameter;
        }

        [SerializeField]
        private DroneExplosion _explosionPrefab;

        private void Explode()
        {
            var explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
            explosion.Parameter = Parameter;
            Destroy(gameObject);
        }
    }
}