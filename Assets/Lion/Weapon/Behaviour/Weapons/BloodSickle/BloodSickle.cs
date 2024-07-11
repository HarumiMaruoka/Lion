using Cysharp.Threading.Tasks;
using Lion.Weapon.Behaviour.BloodSickleModules;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // ブラッドシックル (Blood Sickle):
    // 血の鎌を召喚し、前方の敵を薙ぎ払う。
    // 敵にダメージを与えるごとにプレイヤーの体力がわずかに回復する。
    public class BloodSickle : WeaponBehaviour
    {
        private Func<UniTask>[] _tasks = new Func<UniTask>[2];

        [SerializeField]
        private float _minFireInterval = 0.5f;
        [SerializeField]
        private float _maxFireInterval = 2f;

        private float WaitTime
        {
            get
            {
                if (Parameter == null) return _minFireInterval;
                return Mathf.Clamp(_maxFireInterval - Parameter.AttackSpeed * 0.01f, _minFireInterval, _maxFireInterval);
            }
        }

        private void Start()
        {
            _tasks[0] = Fire;
            _tasks[1] = Wait;

            RunSequence();
        }

        private async void RunSequence()
        {
            int index = 0;
            while (this)
            {
                await _tasks[index]();
                index = (index + 1) % _tasks.Length;
            }
        }

        [SerializeField]
        private Bullet _bulletPrefab;

        private async UniTask Fire()
        {
            var bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.Parameter = Parameter;
        }

        private async UniTask Wait()
        {
            var w = WaitTime;
            for (float t = 0f; t < w; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }
        }
    }
}