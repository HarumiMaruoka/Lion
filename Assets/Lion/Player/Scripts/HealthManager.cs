using System;
using UnityEngine;

namespace Lion.Player
{
    public class HealthManager
    {
        public HealthManager(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        private float _currentHealth;
        private float _maxHealth;

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth == 0)
                {
                    Die();
                }
            }
        }

        public void Damage(int amount)
        {
            CurrentHealth -= amount;
        }

        private void Die()
        {
            Debug.Log("Player died");
        }
    }
}