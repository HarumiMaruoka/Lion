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

            GemCollectorContainer.Instance.Register(gameObject, this);
            GoldCollectorContainer.Instance.Register(gameObject, this);
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
            GemCollectorContainer.Instance.Unregister(gameObject);
            GoldCollectorContainer.Instance.Unregister(gameObject);
        }

        public void Damage(int amount)
        {
            PlayerManager.Instance.HealthManager.Damage(amount);
        }

        public void CollectGold(int amount)
        {

        }

        public void CollectGem(int amount)
        {
            PlayerManager.Instance.ExpLevelManager.AddExp(amount);
        }

        private float _life;
        public event Action<float> OnLifeChanged;
        public float Life
        {
            get => _life;
            set
            {
                _life = Mathf.Clamp(value, 0f, PlayerManager.Instance.Status.HP);
                OnLifeChanged?.Invoke(_life);
            }
        }

        public void Heal(float amount)
        {
            Life += amount;
        }

        public void Revive()
        {
            Life = PlayerManager.Instance.Status.HP;
        }

        public void Damage(float amount)
        {
            Life -= amount;
        }
    }
}
