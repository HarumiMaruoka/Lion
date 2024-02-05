using System;
using UnityEngine;

public class GameSpeedManager
{
    private static GameSpeedManager _instance = null;
    public static GameSpeedManager Instance => _instance ??= new GameSpeedManager();
    private GameSpeedManager() { }

    private PauseManager PauseManager => PauseManager.Instance;

    private float _timeScale = 1f;

    public event Action<float> OnTimeScaleChanged;

    public float TimeScale
    {
        get
        {
            if (PauseManager.IsPaused) return 0f;
            else return _timeScale;
        }
        set
        {
            var old = _timeScale;
            _timeScale = value;

            if (old != _timeScale)
            {
                OnTimeScaleChanged?.Invoke(_timeScale);
            }
        }
    }
}