using System;
using UnityEngine;

namespace Lion.Player
{
    public class PlayerController : MonoBehaviour
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
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
