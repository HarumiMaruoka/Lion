using Lion.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Damage
{
    // 継続魔法ダメージ状態を表すクラス。
    public class ContinualMagicDamage : MonoBehaviour
    {
        public float Interval { get; private set; }
        public float Delay { get; private set; }
        public float MagicPower { get; private set; }
        public IDamagable Damagable { get; private set; }
        public GameObject DamageVFXPrefab { get; private set; }
        public IActor Attacker { get; set; }

        private bool _isInitialized = false;

        public void Initialzie(IActor attacker, IDamagable damagable, float interval, float delay, float magicPower, GameObject damageVFXPrefab = null)
        {
            _intervalElapsedTime = 0f;
            _delayElapsedTime = 0f;

            Attacker = attacker;
            Damagable = damagable;
            Interval = interval;
            Delay = delay;
            MagicPower = magicPower;
            DamageVFXPrefab = damageVFXPrefab;

            _isInitialized = true;

            damagable.OnDead += OnTargetDead;
        }

        private float _intervalElapsedTime = 0f;
        private float _delayElapsedTime = 0f;

        public ParticleSystem StateVFX { get; set; }

        private void Update()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("ContinualMagicDamage is not initialized.");
                return;
            }

            _delayElapsedTime += Time.deltaTime;
            _intervalElapsedTime += Time.deltaTime;

            if (_delayElapsedTime > Delay)
            {
                if (StateVFX) StateVFX.Stop();
                Destroy(this);
            }

            if (_intervalElapsedTime >= Interval)
            {
                Damagable.MagicDamage(MagicPower, Attacker);
                CreateDamageVFX();
                _intervalElapsedTime = 0f;
            }
        }

        public void CreateDamageVFX()
        {
            if (DamageVFXPrefab == null) return;
            Instantiate(DamageVFXPrefab, transform.position, Quaternion.identity);
        }

        private void OnTargetDead()
        {
            Damagable.OnDead -= OnTargetDead;
            if (StateVFX) Destroy(StateVFX.gameObject);
            Destroy(this);
        }
    }
}