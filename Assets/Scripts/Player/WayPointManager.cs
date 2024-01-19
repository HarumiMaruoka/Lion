using System;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] _points;

    private int _currentIndex = 0;

    private Transform _currentTarget;

    public Transform CurrentTarget => _currentTarget;

    private void Start()
    {
        if (_points == null || _points.Length == 0)
        {
            Debug.LogWarning("Not assigned points");
            return;
        }

        _currentTarget = _points[_currentIndex];
    }

    public void OnNext()
    {
        _currentIndex++;
        _currentIndex %= _points.Length;
        _currentTarget = _points[_currentIndex];
    }
}