using System;
using UnityEngine;

public class PauseManager
{
    private static PauseManager _instance = null;
    public static PauseManager Instance => _instance ??= new PauseManager();
    private PauseManager() { }

    private int _pauseCount = 0;

    public int PauseCount => _pauseCount;
    public bool IsPaused => _pauseCount > 0;

    private Action onPaused;
    public event Action OnResumed;

    public event Action OnPaused
    {
        add
        {
            if (IsPaused) value?.Invoke(); // 既にポーズしていれば追加時に実行する。
            onPaused += value;
        }
        remove => onPaused -= value;
    }

    public void Pause()
    {
        if (_pauseCount == 0)
        {
            onPaused?.Invoke();
        }

        _pauseCount++;
    }

    public void Resume()
    {
        _pauseCount--;

        if (_pauseCount == 0)
        {
            OnResumed?.Invoke();
        }
    }
}