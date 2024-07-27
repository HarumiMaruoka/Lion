using Lion.Actor;
using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerController : MonoBehaviour, IActor
    {
        public static PlayerController Instance { get; private set; }

        private Vector3 _previousPosition = new Vector3(-1, 0, 0);
        private Vector3 _currentPosition;
        public Vector3 Direction => (_currentPosition - _previousPosition).normalized;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("PlayerController instance already exists. Destroying duplicate.");
            }

            ActorContainer.Instance.Register(gameObject, this);

            ActorManager.Register(this);
        }

        private void Update()
        {

            if (transform.position != _currentPosition)
            {
                _previousPosition = _currentPosition;
                _currentPosition = transform.position;
            }
        }

        private void OnDestroy()
        {
            Instance = null;
            ActorContainer.Instance.Unregister(gameObject);

            ActorManager.Unregister(this);
        }

        public void CollectGold(int amount)
        {

        }

        public void CollectGem(int amount)
        {
            PlayerManager.Instance.LevelManager.ExpLevelManager.AddExp(amount);
        }

        public event Action<float> OnHPChanged
        {
            add => PlayerManager.Instance.HPManager.OnHPChanged += value;
            remove => PlayerManager.Instance.HPManager.OnHPChanged -= value;
        }

        public float MaxHP => PlayerManager.Instance.HPManager.MaxHP;
        public float CurrentHP => PlayerManager.Instance.HPManager.CurrentHP;

        public float PhysicalPower => 0f;
        public float MagicPower => 0f;

        public void Heal(float amount) => PlayerManager.Instance.HPManager.Heal(amount);
        public void Damage(float amount) => PlayerManager.Instance.HPManager.Damage(amount);
    }
}
