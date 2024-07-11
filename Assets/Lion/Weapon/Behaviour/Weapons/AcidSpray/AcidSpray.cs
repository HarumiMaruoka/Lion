using Cysharp.Threading.Tasks;
using Lion.Weapon.Behaviour.AcidSprayModule;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Weapon.Behaviour
{
    // アシッドスプレー (Acid Spray):
    // 酸を噴射し、前方の広範囲に渡ってダメージを与える。
    // 酸は一定時間地面に残り、踏んだ敵に持続ダメージを与える。
    public class AcidSpray : WeaponBehaviour
    {
        [SerializeField]
        private float _minWaitTime = 0.5f;

        [SerializeField]
        private float _maxWaitTime = 1.8f;

        // 酸の噴射エフェクト
        public SprayEffect _sprayEffect;

        private Func<UniTask>[] _actionSequence = new Func<UniTask>[2];

        private void Start()
        {
            _actionSequence[0] = Spray;
            _actionSequence[1] = Wait;

            RunSequence();
        }

        private async void RunSequence()
        {
            int index = 0;
            while (this)
            {
                await _actionSequence[index]();
                index = (index + 1) % _actionSequence.Length;
            }
        }

        private async UniTask Spray()
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0f, 360f)));

            var sprayEffect = Instantiate(_sprayEffect, transform.position, rotation);
            sprayEffect.Parameter = Parameter;

            while (sprayEffect)
            {
                await UniTask.Yield();
            }
        }

        private async UniTask Wait()
        {
            var waitDuration = _minWaitTime;
            if (Parameter != null) waitDuration = Mathf.Clamp(_maxWaitTime - Parameter.AttackSpeed / 100f, _minWaitTime, _maxWaitTime);

            for (float t = 0; t < waitDuration; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }
        }
    }
}