using Cysharp.Threading.Tasks;
using Lion.Weapon.Behaviour.ArcaneBoltModule;
using System;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // アーケインボルト (Arcane Bolt):
    // 魔法のエネルギーを集中させたボルトを放ち、敵に当たると爆発して範囲内の敵にダメージを与える。
    // ボルトは追尾性能があり、正確に敵を狙う。
    public class ArcaneBolt : WeaponBehaviour
    {
        [SerializeField]
        private ArcaneBoltBullet _bullet;

        [SerializeField]
        private float _minWaitTime = 0.5f;
        [SerializeField]
        private float _maxWaitTime = 1.5f;

        public float Speed => Parameter == null ? 10f : Parameter.AttackSpeed;

        private Func<UniTask>[] _actionSequence = new Func<UniTask>[2];

        private void Start()
        {
            _actionSequence[0] = Shoot;
            _actionSequence[1] = Wait;

            RunSequence();
        }

        private async UniTask Shoot()
        {
            var bullet = Instantiate(_bullet, transform.position, transform.rotation);
            bullet.Parameter = Parameter;

            while (bullet)
            {
                await UniTask.Yield();
            }
        }

        private async UniTask Wait()
        {
            var waitDuration = _minWaitTime;
            if (Parameter != null) waitDuration = CalculateAdjustedWaitTime();

            for (float t = 0; t < waitDuration; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }
        }

        private float CalculateAdjustedWaitTime()
        {
            return Mathf.Clamp(_maxWaitTime - Speed / 100f, _minWaitTime, _maxWaitTime);
        }

        private async void RunSequence()
        {
            var index = 0;
            while (this)
            {
                await _actionSequence[index]();
                index = (index + 1) % _actionSequence.Length;
            }
        }
    }
}