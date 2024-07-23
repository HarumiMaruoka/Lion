using System;
using UnityEngine;

namespace Lion.Manager
{
    public class GameSpeedManager
    {
        private float _gameSpeed = 1.0f;
        public float GameSpeed => IsPaused ? 0.0f : _gameSpeed;

        public bool IsPaused { get; private set; } = false;
        public int PauseCount { get; private set; } = 0;

        public void SetGameSpeed(float gameSpeed)
        {
            if (gameSpeed < 0.0f)
            {
                throw new ArgumentOutOfRangeException("Game speed cannot be negative.");
            }
            _gameSpeed = gameSpeed;
        }

        public void Pause()
        {
            if (PauseCount == 0)
            {
                IsPaused = true;
            }
            PauseCount++;
        }

        public void Resume()
        {
            if (PauseCount > 0)
            {
                PauseCount--;
            }
            if (PauseCount == 0)
            {
                IsPaused = false;
            }
        }
    }
}