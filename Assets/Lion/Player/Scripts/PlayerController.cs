using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerController : MonoBehaviour, IActor
    {
        public static PlayerController Instance { get; private set; }

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

    }
}
